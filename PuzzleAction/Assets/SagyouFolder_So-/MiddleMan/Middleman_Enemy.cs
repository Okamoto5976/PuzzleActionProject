using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class Middleman_Enemy : MonoBehaviour
{
    [System.Serializable]
    private struct EnemyDict
    {
        public Enum_EnemyType type;
        public ComponentPoolHandler_Enemy pool;
    }

    [SerializeField] private List<EnemyDict> enemyPools;

    public DummyEnemyScript GetEnemy(Enum_EnemyType enemyType)
    {
        var pool = enemyPools.Find(x => x.type == enemyType).pool;
        if (pool == null)
        {
            Debug.LogError("Missing Pool or Key", this);
            return null;
        }
        return pool.GetComponentFromPool();
    }
}
