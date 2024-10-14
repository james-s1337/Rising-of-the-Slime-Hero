using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SlimeAttack
{
    public string attackName;
    public float damage; // 4 - 50
    public SlimeAttackType attackType;
    public float energyCost; // 5 - 80

    public string boostStat; // Only for boosting attacks
    public string healStat; // Only for healing attacks
}

public enum SlimeAttackType
{
    Damage,
    Heal,
    Boost,
    Shield
}
