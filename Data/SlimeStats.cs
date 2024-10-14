using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Slime Stats")]
public class SlimeStats : ScriptableObject
{
    [Header("Combat Stats")]
    public float maxHealth;
    public float maxEnergy;
    public float strength;
    public float speed;

    public SlimeAttack attack;
}

// When saving each slime stat, save all the stats in one string, in order (ex. "100-100-5-7-2" -> "Health-Energy-Strength-Speed")
// Slime moves will include single target moves, AOE moves (some can damage allies), stat boosting moves, and energy-draining moves (some steal energy)
// Will fight in each area in waves, health is replenished after every wave
