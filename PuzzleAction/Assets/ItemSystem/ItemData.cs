using UnityEngine;

//public enum EfectType
//{
//    Null,
//    Heal,
//    Damage,
//    Buff,
//    Debuff,
//    Torap
//}
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
    //[SerializeField] private EfectType efectType;    //アイテムの効果タイプ
    public int ItemID { get => itemID; }
    public string ItemName { get => itemName; }
    public Sprite ItemIcon { get => itemIcon; }
    public string Description { get => description; }
    public int MaxStack { get => maxStack; }
    public float DropRate { get => dropRate; }
    //public EfectType EfectType { get => efectType; }
    public　GameObject DropPrefab { get => dorpPrefab; }
    public int dropWeight; // ドロップ確率の重み


}




