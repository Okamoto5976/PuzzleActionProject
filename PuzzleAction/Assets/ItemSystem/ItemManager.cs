using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Pool;

public class ItemManager : MonoBehaviour
{

    public enum EffectType
    {
        Null,
        Heal,
        Damage,
        Buff,
        Debuff,
        Torap
    }
    //[SerializeField] private EntityData entityprefab;
    private ItemPool itempool;
    public GameObject prefab; //アイテムのプレハブ
    //private int nextId = 1; //次のIDを管理する変数
    

    //リスト初期化
    public List<Item> ItemList = new List<Item>();
 

    //Listの中からIDと同じアイテムを探す
    private Item LookForID(int id)
    {
        return ItemList.Find(x => x.Id == id);
    }
    public void ItemUse(int id , Entity entity)  // Entity
    {
        //見つけたアイテムを使用する
        Item item = LookForID(id);
        if (item != null)
        {
            //item.RecieveData(id, entity);
        }
        else
        {
            Debug.LogWarning($"ID{id}のアイテムは見つかりませんでした。");
        }

    }

    //ランダムにアイテムを渡す
    public Item RandomGetItem()
    {
        int index = Random.Range(0, ItemList.Count);
        return ItemList[index];

    }
    public void PrefabCool(EffectType type, ItemRecieveData data)
    {

        Debug.Log("Test");
        //ItemManagerからpool経由してEntityを呼ぶItemManagerでpoolを仲介にenumで種類を渡す
        Entity entity = itempool.Get(type); //ItemManagerからpool経由してEntityを呼ぶItemManagerでpoolを仲介にenumで種類を渡す
        //Playerから渡されたdataの中にある座標の位置に呼び出す
        GameObject obj = Instantiate(prefab, data.pos, Quaternion.LookRotation(data.dir)); //プレイヤーデータから座標と向きを呼び出す
        //Playerから渡されたdataのbaseValueを呼び出したEntityに渡す 
        entity.BaseValue = data.baseValue; //EntityにbaseValueを渡す

    }



}




//public void GetItemData(ItemData data) //座標やエンティティ自身
//{
//    switch (data.ItemType)
//    {
//        case ItemData.Itemtype.Value:
//            //Value type List
//            LookForID(data.ItemID, ValueList);
//            break;
//        case ItemData.Itemtype.Collider:
//            //Collider type List
//            LookForID(data.ItemID, ColliderList);
//            break;
//        case ItemData.Itemtype.Other:
//            //Other type List
//            LookForID(data.ItemID, OtherList);
//            break;
//        default:
//            break;
//    }