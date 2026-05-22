using System.ComponentModel;
using UnityEngine;

public enum ItemType
{
    Active,
    Passive
}

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Data")]
public class Data : ScriptableObject
{
    public int ID;
    public string itemName;
    public string info;
    public Sprite icon;
    public bool stackable;

    public ItemType itemType;
}
