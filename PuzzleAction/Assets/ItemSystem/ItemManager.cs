using System.Collections.Generic;
using UnityEngine;



public class ItemManager : MonoBehaviour
{
    [SerializeField] public string EnemyName;
    public List <Item> EnemyDropList = new();
    public List<Item> DropList = new();
    private List<Item> DeforutDropList = new List<Item>();
    
    public List<Item> ShopList = new();
    //private int nextId; //次のIDを管理する変数
   
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
            
        }
        else
        {
            Debug.LogWarning($"ID{id}のアイテムは見つかりませんでした。");
        }

    }
    public void RandomDropItem(string id, ItemRecieveData r_data)
    {
        DropList = EnemyDropList;
        List<DropPool> candidates = new List<DropPool>();
        int dropIndex = Random.Range(0, DropList.Count);
        //ItemDorp(r_data.pos, dropIndex);
        //return DropList[dropIndex];
        GetItem(id);
        //GameObject item = pools[PoolItem.prefab].Dequeue();
        //item.SetActive(true);


    }

    private void GetItem(string id)
    {
        throw new System.NotImplementedException();
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