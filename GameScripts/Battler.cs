using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Player battler
public class Battler : MonoBehaviour
{
    [SerializeField] private Team team;

    public UnityEvent onTurnFinished, onLowEnergy;

    private Slime currentSlime;
    [SerializeField] private Slime playerSlime;
    [SerializeField] private GameObject playerTeamObj;

    private SlimeAttack currentAttack;

    public void Rest()
    {
        Debug.Log(gameObject.name + " is resting.");
        playerSlime.RestoreEnergy(playerSlime.GetMaxEnergy() / 2);

        PlayTeamMemberTurns();
    }

    public void PlayTurn() // Will run off an event after the player has pressed a button corresponding to an attack (there are 3 attacks)
    {
        // Check if enemy is dead
        playerSlime.SetCanDamage();
        currentSlime = playerSlime;
        currentAttack = playerSlime.GetAttack();

        if (currentAttack.energyCost > currentSlime.GetCurrentEnergy())
        {
            if (gameObject.tag != "Player")
            {
                Rest();
                return;
            }

            onTurnFinished?.Invoke();
            return;
        }

        currentSlime.TakeEnergy(currentAttack.energyCost);

        if (currentAttack.attackType == SlimeAttackType.Damage)
        {
            // Play attack animation
            currentSlime.Attack();
        }
        else if (currentAttack.attackType == SlimeAttackType.Heal)
        {
            if (currentAttack.healStat == "Health") {
                Debug.Log(gameObject.name + " healed " + currentAttack.damage);
                HealHealth(currentAttack.damage);
            }
            else
            {
                Debug.Log(gameObject.name + " restored " + currentAttack.damage);
                RestoreEnergy(currentAttack.damage);
            }

            currentSlime.Eat();
        }
        else // Boost
        {
            if (playerSlime.FindStatMod(currentAttack.boostStat, currentAttack.damage))
            {
                Debug.Log(currentSlime.gameObject.name + " already boosted the player!");
            }
            else
            {
                Debug.Log(gameObject.name + " boosted " + currentAttack.boostStat + " by " + currentAttack.damage);
                currentSlime.ModifyStat(currentAttack.boostStat, currentAttack.damage);
            }
            
            currentSlime.Eat();
        }
    }

    // Will run when onTurnFinished is invoked
    private void PlayTeamMemberTurns()
    {
        if (team != null && team.GetTeamMemberCount() > 0)
        {
            foreach (Slime supportSlime in team.SlimeTeam)
            {
                currentSlime = supportSlime;

                SlimeAttack supportAttack = supportSlime.GetAttack();
                currentAttack = supportAttack;

                if (supportAttack.energyCost > supportSlime.GetCurrentEnergy())
                {
                    Debug.Log(supportSlime.gameObject.name + " does not have enough energy to cast " + supportAttack.attackName);
                    currentSlime.RestoreEnergy(currentSlime.GetMaxEnergy() / 2); // auto rest if no energy left to cast spell
                    continue;
                }

                

                if (supportAttack.attackType == SlimeAttackType.Damage)
                {
                    currentSlime.TakeEnergy(currentAttack.energyCost);
                    currentSlime.Attack();
                }
                else if (supportAttack.attackType == SlimeAttackType.Heal)
                {
                    if (supportAttack.healStat == "Health" && playerSlime.GetCurrentHealth() != playerSlime.GetMaxHealth())
                    {
                        HealHealth(supportAttack.damage);
                    }
                    else if (supportAttack.healStat == "Energy" && playerSlime.GetCurrentEnergy() != playerSlime.GetMaxEnergy())
                    {
                        RestoreEnergy(supportAttack.damage);
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (supportAttack.attackType == SlimeAttackType.Shield)
                {
                    currentSlime.TakeEnergy(currentAttack.energyCost);
                    playerSlime.Shield();
                }
                else // Boost
                {
                    if (playerSlime.FindStatMod(supportAttack.boostStat, supportAttack.damage))
                    {
                        Debug.Log(supportSlime.gameObject.name + " already boosted the player!");
                        continue;
                    }
                    currentSlime.TakeEnergy(currentAttack.energyCost);
                    playerSlime.ModifyStat(supportAttack.boostStat, supportAttack.damage);
                }
            }
        }

        StartCoroutine(TurnCooldown()); // Other battler will initiate
    }

    private IEnumerator TurnCooldown()
    {
        yield return new WaitForSeconds(1f);
        onTurnFinished?.Invoke();
    }

    // Restores health in battle, and temporarily gives you a boost in stats depending on what type of slime you ate
    public void EatTeamMember()
    {
        Slime member = GameObject.FindGameObjectWithTag("Recruitable").GetComponent<Slime>();

        float bonusHealth = Mathf.Round(member.GetMaxHealth() / 10);

        playerSlime.Eat();
        member.GetEaten();

        playerSlime.ImproveStat("Health", bonusHealth);
        playerSlime.ImproveStat("Strength", member.GetStrength());
        playerSlime.ImproveStat("Energy", member.GetMaxEnergy() / 2);
        playerSlime.IncreaseSize();
    }

    public void RecruitTeamMember()
    {
        GameObject newMember = Instantiate(GameObject.FindGameObjectWithTag("Recruitable"), playerTeamObj.transform);
        Slime member = newMember.GetComponent<Slime>();

        newMember.transform.rotation = Quaternion.identity;
        newMember.tag = "Untagged";

        member.Convert();
        team.AddTeamMember(member);
    }

    public void HealHealth(float amount)
    {
        playerSlime.Heal(amount);
    }

    public void RestoreEnergy(float amount)
    {
        playerSlime.RestoreEnergy(amount);
    }

    // Only run this if it is the player
    public void OnAttackAnimationFinishedTrigger()
    {
        playerSlime.AttackFinishedTrigger();
        PlayTeamMemberTurns();
    }
}
