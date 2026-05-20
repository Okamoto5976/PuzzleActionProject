using System;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Coin,
    Potion,
    Weapon
}
public class Entity : MonoBehaviour
{
    public ItemType Type { get; private set; }
    public float BaseValue { get; private set; }

    // ‰Šú‰»ˆ—
    public void Initialize(ItemType type, float baseValue)
    {
        Type = type;
        BaseValue = baseValue;
        gameObject.SetActive(true);
    }

    // ƒv[ƒ‹‚É–ß‚·‚Æ‚«
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    internal void BuffSet(BuffItem.BuffType buffType, float value, float buffDuration)
    {
        throw new NotImplementedException();
    }
}