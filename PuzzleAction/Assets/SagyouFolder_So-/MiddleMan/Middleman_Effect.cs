using System.Collections.Generic;
using UnityEngine;

public class Middleman_Effect : MonoBehaviour
{
    [System.Serializable]
    private struct EffectDict
    {
        public Enum_EffectType type;
        public ComponentPoolHandler_Effect pool;
    }

    [SerializeField] private List<EffectDict> effectPools;

    public EffectObj GetEffect(Enum_EffectType effectType)
    {
        var pool = effectPools.Find(x => x.type == effectType).pool;

        if (pool == null)
        {
            Debug.LogError("Missing Pool or Key", this);
            return null;
        }
        return pool.GetComponentFromPool();
    }
}
