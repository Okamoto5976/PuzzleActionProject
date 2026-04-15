using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using static ItemData;
using static UnityEngine.Rendering.DebugUI;




[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Datas/ItemData")]
public class ItemData : ScriptableObject
{
    //アイテムタイプ
    public enum itemtype
    {
        healing,    //回復
        powerUp,    //攻撃力アップ
        biled,      //防御力アップ
    }
    [Header("基本情報")]
    [SerializeField] private int itemID;
    [SerializeField] private string itemName;       //アイテム名
    [SerializeField] private Sprite itemIcon;       //アイテムアイコン   
    [SerializeField] private itemtype itemType;     //アイテムタイプ
    [SerializeField] private string description;    //アイテム説明
    [SerializeField] private int value;            //攻撃力、回復力、防御力など
    [SerializeField] private int maxStack;          //最大スタック数

    public int ItemID { get => itemID; }
    public string ItemName { get => itemName; }
    public itemtype ItemType { get => itemType; }
    public Sprite ItemIcon { get => itemIcon; }
    public string Description { get => description; }
    public int Value { get => value; }
    public int MaxStack { get => maxStack; }

    

}



