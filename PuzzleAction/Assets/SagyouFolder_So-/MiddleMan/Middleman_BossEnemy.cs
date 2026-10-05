using System.Collections.Generic;
using UnityEngine;

public class Middleman_BossEnemy : MiddlemanBase<Enum_BossType, ComponentPoolHandler_BossEnemy, BossEnemyController>
{
    private BossEnemyController _currentBossEnemyController;
    private float _bossMaxHP;
    private bool _isBossActive = false;

    public float BossHP => _currentBossEnemyController == null ? 0 : _currentBossEnemyController.HP;
    public float BossMaxHP => _bossMaxHP;

    public bool IsBossActive => _isBossActive;

    public BossEnemyController GetBoss(Enum_BossType bossType)
    {
        _currentBossEnemyController = GetComponent(bossType);
        _bossMaxHP = _currentBossEnemyController.HP;
        _isBossActive = true;
        return _currentBossEnemyController;
    }
}