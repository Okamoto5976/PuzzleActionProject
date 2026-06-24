using UnityEngine;


[System.Serializable]
public class ItemData
{
    [SerializeField] private int itemID;            
    [SerializeField] private string itemName;       
    [SerializeField] private Sprite itemIcon;         
    [SerializeField] private string description;    
    [SerializeField] private int maxStack;          
    [SerializeField] private float dropRate;        
    [SerializeField] private int itemGrade;         //Rarity
    [SerializeField] private ItemType itemType;
    [SerializeField, Min(0)] private int price;


    public int ItemID { get => itemID; }
    public string ItemName { get => itemName; }
    public Sprite ItemIcon { get => itemIcon; }
    public string Description { get => description; }
    public int MaxStack { get => maxStack; }
    public float DropRate { get => dropRate; }
    public int ItemGrade { get => itemGrade; }
    public ItemType ItemType => itemType;
    public int Price => price;
    public bool IsShopCompatible => price > 0;

}




