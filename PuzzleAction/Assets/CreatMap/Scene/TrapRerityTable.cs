using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TrapRarityTable", menuName = "Scriptable Objects/GachaEngine/TrapRarityTable")]
public class TrapRarityTable : ScriptableObject
{
    [System.Serializable]
    public class TrapData
    {
        public Enum_TrapType trapType;
        public RarityEnumAsset rarity;
    }

    [SerializeField]
    private List<TrapData> traps;

    public List<Enum_TrapType> GetTraps(RarityEnumAsset rarity)
    {
        return traps
            .Where(x => x.rarity == rarity)
            .Select(x => x.trapType)
            .ToList();
    }
}