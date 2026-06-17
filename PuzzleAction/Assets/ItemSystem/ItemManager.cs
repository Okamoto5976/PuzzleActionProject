using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class ItemManager : MonoBehaviour
{
    public List<Item> EnemyDropList = new();
    public List<Item> DropList = new();
    public List<Item> ShopList = new();
    //private int nextId; //次のIDを管理する変数
    DropPool pool;
    //リスト初期化
    public List<Item> ItemList = new List<Item>();

    //Listの中からIDと同じアイテムを探す
    private Item LookForID(int id)
    {
        return ItemList.Find(x => x.Id == id);
    }
   
   
    //public void ItemUse(int id/*, Entity entity*/)  // Entity
    //{
    //    //見つけたアイテムを使用する
    //    Item item = LookForID(id);
    //    if (item != null)
    //    {
            
    //    }
    //    else
    //    {
    //        Debug.LogWarning($"ID{id}のアイテムは見つかりませんでした。");
    //    }

    //}
    public void RandomDropItem(ItemRecieveData r_data , List drop)
    {

        //DropList = DropEnemy;
        int dropIndex = Random.Range(0, DropList.Count);
        Item item = LookForID(dropIndex);
        pool.ItemDrop(dropIndex, r_data);

        //return DropList[dropIndex];
        Debug.Log($"{dropIndex}をドロップしました。");
        DropList.Clear();
    }

  


    //ランダムにアイテムを渡す
    public Item RandomGetItem()
    {
        int index = Random.Range(0, ItemList.Count); //アイテムを抽選する
        return ItemList[index]; // アイテムを渡す

    }
    //アイテムのエフェクトを呼び出す

    public Item RandomShopItem()
    {
        int ShopIndex= Random.Range(0, ShopList.Count);
        return ShopList[ShopIndex];
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