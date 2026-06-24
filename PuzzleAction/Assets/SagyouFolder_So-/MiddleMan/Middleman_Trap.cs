using System.Collections.Generic;
using UnityEngine;

public class Middleman_Trap : MonoBehaviour
{
    [System.Serializable]
    private struct TrapDict
    {
        public Enum_TrapType type;
        public ComponentPoolHandler_Trap pool;
    }

    [SerializeField] private List<TrapDict> trapPools;

    public TrapBase GetTrap(Enum_TrapType trapType)
    {
        var pool = trapPools.Find(x => x.type == trapType).pool;
        if (pool == null)
        {
            Debug.LogError("Missing Pool or Key", this);
            return null;
        }
        return pool.GetComponentFromPool();
    }
}
