using System.Collections.Generic;
using UnityEngine;

public class Middleman_BossEnemy : MiddlemanBase<Enum_BossType, ComponentPoolHandler_BossEnemy, BossEnemyController>
{
    private BossEnemyHP _currentBossEnemyHP;
    private bool _isBossActive = false;

    public float BossHP => _currentBossEnemyHP == null ? 0 : _currentBossEnemyHP.CurrentHP;
    public float BossMaxHP => _currentBossEnemyHP == null ? 0 : _currentBossEnemyHP.MaxHP;

    public bool IsBossActive => _isBossActive;

    public BossEnemyController GetBoss(Enum_BossType bossType)
    {
        var component = GetComponent(bossType);
        _currentBossEnemyHP = component.GetComponent<BossEnemyHP>();
        _isBossActive = true;
        return component;
    }
}