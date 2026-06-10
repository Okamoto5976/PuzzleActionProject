using System;
using System.Collections.Generic;
using Unity.Multiplayer.Center.Common;
using UnityEngine;
using static UnityEditor.PlayerSettings;


public class ItemManager : MonoBehaviour
{
    [SerializeField] public string EnemyName;
    public List<Item> DropList = new();
    private List<Item> DeforutDropList = new List<Item>();
    DropPool pools;
    
    public List<Item> ShopList = new();
    private int nextId; //次のIDを管理する変数
    public struct ItemRecieveData
    {
        //public Entity entity;
        public float baseValue; //Entity用　例）矢の攻撃力＋Entityの攻撃力
        public Vector3 pos;
        public Vector3 dir;//向き
        public Vector2 size;
    }
    ItemData data;
    //リスト初期化
    public List<Item> ItemList = new List<Item>();

    //Listの中からIDと同じアイテムを探す
    private Item LookForID(int id)
    {
        return ItemList.Find(x => x.Id == id);
    }
    public void ItemUse(int id/*, Entity entity*/)  // Entity
    {
        //見つけたアイテムを使用する
        Item item = LookForID(id);
        if (item != null)
        {
            //item.RecieveData(id, );
        }
        else
        {
            Debug.LogWarning($"ID{id}のアイテムは見つかりませんでした。");
        }

    }
    public void Drop(ItemRecieveData r_data, ItemManager DropIndex)
    {
        //DropList = EnemyDropList;
        List<DropPool> candidates = new List<DropPool>();
        Vector3 DropPos = r_data.pos;
        int dropIndex = UnityEngine.Random.Range(0, DropList.Count);
        //DropPool Get(dropIndex);
    


        //GameObject item = Instantiate(data.Prefab, DropPos, Quaternion.identity);


    }

    public void drop(GameObject item)
    {
        //if (pools[selectied.data.Prefab].Count > 0)
        //{
        //    GameObject item = pools[data.Prefab].Dequeue();
        //    item.transform.position = pos;
        //    item.SetActive(false);
        //    Debug.Log($"{item.name}をドロップしました。");
        //}
    }
    //ランダムにアイテムを渡す
    public Item RandomGetItem()
    {
        int index = UnityEngine. Random.Range(0, ItemList.Count); //アイテムを抽選する
        return ItemList[index]; // アイテムを渡す

    }
    //アイテムのエフェクトを呼び出す

    public Item RandomShopItem()
    {
        int ShopIndex= UnityEngine.Random.Range(0, ShopList.Count);
        return ShopList[ShopIndex];
    }
    public Item RandomDropItem()
    {
        //DropList = EnemyDropList;
        int DorpIndex= UnityEngine.Random.Range(0,DropList.Count);
        return DropList[DorpIndex];
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