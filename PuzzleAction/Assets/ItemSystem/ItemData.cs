using UnityEngine;


//[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Datas/ItemData")]
[System.Serializable]
public class ItemData //: ScriptableObject
{
    //アイテムタイプ
    [Header("基本情報")]
    [SerializeField] private int itemID;
    [SerializeField] private string itemName;       //アイテム名
    [SerializeField] private Sprite itemIcon;       //アイテムアイコン   
    [SerializeField] private Object itemPrefab;     //アイテムのプレハブ
    [SerializeField] private string description;    //アイテム説明
    [SerializeField] private int maxStack;          //最大スタック数
    [SerializeField] private float dropRate;        //ドロップ率
    [SerializeField] private GameObject dorpPrefab; //ドロップアイテムのプレハブ
    [SerializeField] private ItemType itemType;
    [SerializeField, Min(0)] private int price;

    public int ItemID { get => itemID; }
    public string ItemName { get => itemName; }
    public Sprite ItemIcon { get => itemIcon; }
    public string Description { get => description; }
    public int MaxStack { get => maxStack; }
    public float DropRate { get => dropRate; }

    public ItemType ItemType => itemType;
    public int Price { get => price; }

    public GameObject DropPrefab { get => dorpPrefab; }
    public int dropWeight; // ドロップ確率の重み

    public bool IsShopCompatible => price != 0;
}




