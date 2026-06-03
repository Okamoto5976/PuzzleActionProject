using System.ComponentModel;
using UnityEngine;

public enum ItemType
{
    Active,
    Passive
}

[CreateAssetMenu(fileName = "Data")]
public class Data : Item
{
    public int ID => m_data.ItemID;
    public string ItemName => m_data.ItemName;
    public string info => m_data.Description;
    public Sprite icon => m_data.ItemIcon;
    public bool stackable => m_data.MaxStack > 1;

    public ItemType ItemType => m_data.ItemType;
}
