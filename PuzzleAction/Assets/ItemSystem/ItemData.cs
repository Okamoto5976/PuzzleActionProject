using UnityEngine;


//[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Datas/ItemData")]
[System.Serializable]
public class ItemData //: ScriptableObject
{
    //アイテムタイプ
    [Header("基本情報")]
    [SerializeField] private int itemID;            //アイテムID
    [SerializeField] private string itemName;       //アイテム名
    [SerializeField] private Sprite itemIcon;       //アイテムアイコン   
    [SerializeField] private Object itemPrefab;     //アイテムのプレハブ
    [SerializeField] private string description;    //アイテム説明
    [SerializeField] private int maxStack;          //最大スタック数
    [SerializeField] private float dropRate;        //ドロップ率
    [SerializeField] private int itemGrade;         //アイテムのグレード（例：1=一般、2=レア、3=エピックなど）
    [SerializeField] private GameObject dropPrefab; //ドロップアイテムのプレハブ
    [SerializeField] private int dropsize;          //ドロップサイズ


    public int ItemID { get => itemID; }// アイテムのID
    public string ItemName { get => itemName; }// アイテムの名前
    public Sprite ItemIcon { get => itemIcon; } // アイテムのアイコン
    public string Description { get => description; }// アイテムの説明
    public int MaxStack { get => maxStack; }// 最大スタック数
    public float DropRate { get => dropRate; }// ドロップ率（例：0.1=10%の確率でドロップ）
    public int ItemGrade { get => itemGrade; }// アイテムのグレード（例：1=一般、2=レア、3=エピックなど）

    public GameObject DropPrefab { get => dropPrefab; }
    public int DropSize { get => dropsize; } // ドロップ確率の重み


}




