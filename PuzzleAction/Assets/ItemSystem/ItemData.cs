using UnityEngine;

public enum Type
{
    PlayerItem,
    EnemyItem
}

public enum Grade
{
    Comon,
    UnComon,
    Rara,
    Legend
}

//when player use item, check this type. if arrow , player pull the bow animation
public enum ItemUseType
{ 
    None,
    Arrow,
    Set,
}


[System.Serializable]
public class ItemData
{
    [SerializeField] private int itemID;            
    [SerializeField] private string itemName;       
    [SerializeField] private Sprite itemIcon;         
    [SerializeField] private string description;    
    [SerializeField] private int maxStack = 99;          
    [SerializeField] private float dropRate;        
    //[SerializeField] private int itemGrade;         //Rarity
    [SerializeField] private ItemType itemType;
    [SerializeField] private ItemUseType m_itemUseType;
    [SerializeField, Min(0)] private int price;
    [SerializeField] private Type Type;
    [SerializeField] private Grade itemGrade;

    public int ItemID { get => itemID; }
    public string ItemName { get => itemName; }
    public Sprite ItemIcon { get => itemIcon; }
    public string Description { get => description; }
    public int MaxStack { get => maxStack; }
    public float DropRate { get => dropRate; }
    //public int ItemGrade { get => itemGrade; }
    public ItemType ItemType => itemType;
    public ItemUseType ItemUseType => m_itemUseType;
    public int Price => price;
    public bool IsShopCompatible => price > 0;

    public Type type { get => Type; }
    public Grade ItemGrade { get => itemGrade; }
}




