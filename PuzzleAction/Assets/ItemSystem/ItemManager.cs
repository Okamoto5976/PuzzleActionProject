using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

  
    //[SerializeField] private EntityData entityprefab;

    //private int nextId = 1; //次のIDを管理する変数
    

    //リスト初期化
    public List<Item> ItemList = new List<Item>();
 

    //Listの中からIDと同じアイテムを探す
    private Item LookForID(int id)
    {
        return ItemList.Find(x => x.Id == id);
    }
    //public void ItemUse(int id , Entity entity)  // Entity
    //{
    //    //見つけたアイテムを使用する
    //    Item item = LookForID(id);
    //    if (item != null)
    //    {
    //        //item.RecieveData(id, );
    //    }
    //    else
    //    {
    //        Debug.LogWarning($"ID{id}のアイテムは見つかりませんでした。");
    //    }

    //}

    //ランダムにアイテムを渡す
    public Item RandomGetItem()
    {
        int index = Random.Range(0, ItemList.Count);
        return ItemList[index];

    }
    //アイテムのエフェクトを呼び出す
   

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