using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Slime : MonoBehaviour
{
    public string slimeName;

    [SerializeField] private SlimeStats slimeStats;
    public SlimeAttack slimeAttack { get => slimeStats.attack; set => slimeStats.attack = value; }

    private Animator anim;

    public UnityEvent<float> onHealthUpdate, onEnergyUpdate;
    public UnityEvent onMoveUpdate, onHurt, onDeath, onHeal, onBoost, onBlock;

    [SerializeField] private ParticleSystem hurtParticles;
    [SerializeField] private ParticleSystem boostParticles;
    [SerializeField] private ParticleSystem healParticles;
    [SerializeField] private ParticleSystem growthParticles;
    [SerializeField] private ParticleSystem blockParticles;

    [SerializeField] private GameObject shieldSprite;

    [SerializeField] private float maxHealth;
    [SerializeField] private float maxEnergy;

    [SerializeField] private float energy;
    [SerializeField] private float health;

    [SerializeField] private float strength;

    [SerializeField] private List<float> strengthMods;
    [SerializeField] private List<float> speedMods;

    [SerializeField] private string enemyTag; // Either Player or Enemy

    public bool isDead { get; private set; }

    public bool canBeDamaged { get; private set; }
    public Slime enemySlime { get; private set; }

    private void Awake()
    {
        maxHealth = slimeStats.maxHealth;
        maxEnergy = slimeStats.maxEnergy;
        strength = slimeStats.strength;

        strengthMods = new List<float>();
        speedMods = new List<float>();

        SetCanDamage();

        anim = GetComponent<Animator>();
    }

    public void Convert()
    {
        enemyTag = "Enemy";
    }

    private void GetEnemy()
    {
        enemySlime = GameObject.FindGameObjectWithTag(enemyTag).GetComponent<Slime>();
    }

    public void Heal(float amount)
    {
        Debug.Log(gameObject.name + " healed by " + amount);
        health = Mathf.Clamp(health + amount, 0, maxHealth);

        if (healParticles && healParticles.isPlaying == false)
        {
            healParticles.Play();
            onHeal?.Invoke();
        }
    }

    public void RestoreEnergy(float amount)
    {
        Debug.Log(gameObject.name + " restored by " + amount);
        energy = Mathf.Clamp(energy + amount, 0, maxEnergy);

        if (healParticles && healParticles.isPlaying == false)
        {
            healParticles.Play();
            onHeal?.Invoke();
        }
        if (gameObject.tag == "Enemy" || gameObject.tag == "Player")
        {
            onEnergyUpdate?.Invoke(energy / maxEnergy);
        }
    }

    public float GetCurrentEnergy()
    {
        return energy;
    }

    public void ResetVitals()
    {
        // Check if wave is over first
        health = maxHealth;
        energy = maxEnergy;

        strengthMods.Clear();
        speedMods.Clear();

        if (gameObject.tag == "Enemy" || gameObject.tag == "Player")
        {
            onHealthUpdate?.Invoke(health / maxHealth);
            onEnergyUpdate?.Invoke(energy / maxEnergy);
        }

        GetEnemy();
    }

    public void ImproveStat(string stat, float amount)
    {
        if (stat == "Strength")
        {
            strength += amount;
        }
        else if (stat == "Speed") {
            slimeStats.speed += amount;
        }
        else if (stat == "Health")
        {
            maxHealth += amount;

            if (gameObject.tag == "Enemy" || gameObject.tag == "Player")
            {
                onHealthUpdate?.Invoke(health / maxHealth);
            }
        }
        else // Energy
        {
            maxEnergy += amount;

            if (gameObject.tag == "Enemy" || gameObject.tag == "Player")
            {
                onEnergyUpdate?.Invoke(energy / maxEnergy);
            }
        }
    }

    public void IncreaseSize()
    {
        float sizeIncrease = 0.5f;
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + sizeIncrease, gameObject.transform.localScale.y + sizeIncrease, 1);

        growthParticles.Play();
        onHeal?.Invoke();
    }

    public void LearnAttack(SlimeAttack newAttack)
    {
        slimeAttack = newAttack;
        onMoveUpdate?.Invoke();
    }

    public void ModifyStat(string stat, float mod)
    {
        if (stat == "Strength")
        {
            strengthMods.Add(mod);
        }
        else // Speed
        {
            speedMods.Add(mod);
        }

        if (boostParticles && boostParticles.isPlaying == false)
        {
            boostParticles.Play();
            onBoost?.Invoke();
        }
    }

    public bool FindStatMod(string stat, float mod)
    {
        if (stat == "Strength")
        {
            return strengthMods.Contains(mod);
        }
        else // Speed
        {
            return speedMods.Contains(mod);
        }
    }

    public float GetStrength()
    {
        float str = strength;

        foreach (float mod in strengthMods)
        {
            str += mod;
        }
        return str;
    }

    public float GetSpeed()
    {
        return slimeStats.speed;
    }

    public void SetCanDamage()
    {
        canBeDamaged = true;
        if (gameObject.tag == "Enemy" || gameObject.tag == "Player")
        {
            shieldSprite.SetActive(false);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead)
        {
            return;
        }

        if (!canBeDamaged)
        {
            // Play shield animation
            Debug.Log(gameObject.name + " blocked the incoming attack!");
            if (blockParticles && blockParticles.isPlaying == false)
            {
                blockParticles.Play();
                onBlock?.Invoke();
            }
            return;
        }

        Debug.Log(gameObject.name + " took " + amount + " damage");
        health = Mathf.Clamp(health - amount, 0, maxHealth);

        if (gameObject.tag == "Enemy" || gameObject.tag == "Player")
        {
            onHealthUpdate?.Invoke(health / maxHealth);
        }
        if (hurtParticles && hurtParticles.isPlaying == false) {
            hurtParticles.Play();
            onHurt?.Invoke();
            StartCoroutine(FlashRed());
        }
        if (health <= 0)
        {
            Die();
        }
    }

    public IEnumerator FlashRed()
    {
        Color currentColor = gameObject.GetComponent<SpriteRenderer>().color;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        yield return new WaitForSeconds(0.15f);
        gameObject.GetComponent<SpriteRenderer>().color = currentColor;
    }

    public void TakeEnergy(float amount)
    {
        energy = Mathf.Clamp(energy - amount, 0, maxEnergy);

        if (gameObject.tag == "Enemy" || gameObject.tag == "Player")
        {
            onEnergyUpdate?.Invoke(energy / maxEnergy);
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " Died.");
        anim.SetBool("performingAction", true);
        anim.SetBool("dead", true);

        onDeath?.Invoke();
        isDead = true;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void Shield()
    {
        canBeDamaged = false;
        shieldSprite.SetActive(true);
    }

    public SlimeAttack GetAttack()
    {
        return slimeAttack;
    }

    public float GetCurrentHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetMaxEnergy()
    {
        return maxEnergy;
    }

    #region Animation Stuff
    public void Attack()
    {
        anim.SetBool("performingAction", true);
        anim.SetBool("attack", true);
    }

    public void Idle()
    {
        if (gameObject.tag == "Player")
        {
            gameObject.transform.rotation = Quaternion.identity;
        }
        anim.SetBool("performingAction", false);
        anim.SetBool("move", false);
        anim.SetBool("attack", false);
    }

    public void Eat()
    {
        gameObject.transform.Rotate(new Vector3(0, 180.0f, 0));
        anim.SetBool("performingAction", true);
        anim.SetBool("move", true);
    }

    public void AttackFinishedTrigger()
    {
        Idle();
    }

    public void OnAnimationDamageTrigger()
    {
        float damage = slimeAttack.damage + GetStrength();
        // Get enemy slime, damage it
        enemySlime.TakeDamage(damage);
    }

    public void GetEaten()
    {
        onHurt?.Invoke();
        Die();
    }
    #endregion
}
