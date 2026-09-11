using System.Collections.Generic;
using UnityEngine;

public class Middleman_Treasure : MonoBehaviour
{
    [System.Serializable]
    private struct TreasureDict
    {
        public Enum_TreasureType type;
        public ComponentPoolHandler_Treasure pool;
    }

    [SerializeField] private List<TreasureDict> treasurePools;

    public void InitializePool()
    {
        foreach (var pool in treasurePools)
        {
            pool.pool.Initialize();
        }
    }

    public Treasure GetTreasure(Enum_TreasureType type)
    {
        var pool = treasurePools.Find(x => x.type == type).pool;

        if (pool == null)
        {
            Debug.LogError($"Pool Missing : {type}");
            return null;
        }

        return pool.GetComponentFromPool();
    }
}