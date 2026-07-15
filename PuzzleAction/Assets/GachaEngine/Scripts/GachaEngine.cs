using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GachaEngine", menuName = "Scriptable Objects/GachaEngine/GachaEngine")]
public class GachaEngine : ScriptableObject
{
    [System.Serializable]
    struct RarityWithWeight
    {
        [SerializeField] private RarityEnumAsset rarity;
        [SerializeField] private int weight;

        public readonly RarityEnumAsset Rarity => rarity;
        public readonly int Weight => weight;
    }


    [SerializeField] private List<RarityWithWeight> rarities;

    private int totalRarityWeight = 0;
    private int[] rarityWeights;
    private List<RarityWithWeight> sortedRarities;
    private List<RarityEnumAsset> rarityEnumAssets;

    public int TotalRarityWeight => totalRarityWeight;
    public List<RarityEnumAsset> Rarities => rarityEnumAssets;

    private void OnValidate()
    {
        sortedRarities = rarities.OrderBy(x => x.Weight).ToList();
        totalRarityWeight = 0;
        rarityWeights = new int[sortedRarities.Count];
        for (int i = 0; i < sortedRarities.Count; i++)
        {
            totalRarityWeight += sortedRarities[i].Weight;
            rarityWeights[i] = totalRarityWeight;
        }

        rarityEnumAssets = rarities.Select(x => x.Rarity).ToList();
    }

    private int GetUpperbound(int[] array, int target)
    {
        int lo = 0; int hi = array.Length - 1;
        int res = array.Length;

        while (lo <= hi)
        {
            int mid = (lo + hi) >> 1;
            if (array[mid] > target)
            {
                res = mid;
                hi = mid - 1;
            }
            else
            {
                lo = mid + 1;
            }
        }

        return res;
    }

    public RarityEnumAsset Collapse(int rarityWeight)
    {
        var index = GetUpperbound(rarityWeights, rarityWeight);
        return sortedRarities[index].Rarity;
    }

    public RarityEnumAsset Collapse()
    {
        var rarity = Random.Range(0, totalRarityWeight);
        return Collapse(rarity);
    }
}
