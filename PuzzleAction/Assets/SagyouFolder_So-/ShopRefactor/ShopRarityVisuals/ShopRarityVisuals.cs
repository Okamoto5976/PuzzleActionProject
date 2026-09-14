using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopRarityVisuals", menuName = "Scriptable Objects/ShopRarityVisuals")]
public class ShopRarityVisuals : ScriptableObject
{
    [System.Serializable]
    struct ShopRarityVisual
    {
        [SerializeField] private RarityEnumAsset rarity;
        [SerializeField] private Sprite sprite;

        public readonly Sprite Sprite => sprite;
        public readonly RarityEnumAsset Rarity => rarity;
    }

    [SerializeField] private List<ShopRarityVisual> rarityVisuals;

    public Sprite GetSpriteForRarity(RarityEnumAsset rarity)
    {
        if (rarityVisuals.Exists(x => x.Rarity == rarity))
        {
            return rarityVisuals.Find(x => x.Rarity == rarity).Sprite;
        }

        Debug.LogError($"Rarity: {rarity} not found. Please add", this);
        return null;
    }
}
