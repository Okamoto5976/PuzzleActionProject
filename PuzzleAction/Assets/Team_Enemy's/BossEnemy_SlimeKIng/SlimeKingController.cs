using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlimeKingController
{
    [Header("Summon")]
    public int SummonCount = 6;
    public int MaxAliveSummons = 12;

    [Header("Range")]
    public float SummonRadius = 5f;

    [Header("Candidates")]
    public List<Enum_EnemyType> SummonTypes = new()
    {
        Enum_EnemyType.Slime_Blue,
        Enum_EnemyType.Slime_Normal,
        Enum_EnemyType.Slime_Red
    };
}