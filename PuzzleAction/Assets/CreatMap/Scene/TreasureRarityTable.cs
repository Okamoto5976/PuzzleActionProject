using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TreasureRarityTable",menuName = "Scriptable Objects/GachaEngine/TreasureRarityTable")]
public class TreasureRarityTable : ScriptableObject
{
    [System.Serializable]
    public class TreasureData
    {
        public Enum_TreasureType treasureType;
        public RarityEnumAsset rarity;
    }

    [SerializeField]
    private List<TreasureData> treasures;

    public List<Enum_TreasureType> GetTreasures(RarityEnumAsset rarity)
    {
        return treasures
            .Where(x => x.rarity == rarity)
            .Select(x => x.treasureType)
            .ToList();
    }
}