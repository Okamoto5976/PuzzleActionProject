using System.Collections.Generic;
using UnityEngine;

public class Middleman_BossEnemy : MonoBehaviour
{
    [System.Serializable]
    private struct BossDict
    {
        public Enum_BossType type;
        public ComponentPoolHandler_BossEnemy pool;
    }

    [SerializeField] private List<BossDict> m_bossPools;

    public void InitializePool()
    {
        foreach (var boss in m_bossPools)
        {
            boss.pool.Initialize();
        }
    }

    public BossEnemyController GetBoss(Enum_BossType type)
    {
        var pool = m_bossPools.Find(x => x.type == type).pool;

        if (pool == null)
        {
            Debug.LogError($"BossPool Missing : {type}");
            return null;
        }

        return pool.GetComponentFromPool();
    }
}