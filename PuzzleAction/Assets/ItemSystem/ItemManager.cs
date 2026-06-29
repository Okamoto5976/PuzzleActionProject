using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<Item> EnemyDropList = new();
    public List<Item> DropList = new();
    public List<Item> ShopList = new();
    //private int nextId; //次のIDを管理する変数
    [SerializeField] private Middleman_Trap m_middleman_trap;
    DropPool I_pool;
    //リスト初期化
    public List<Item> ItemList = new();

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

    public void OnUseItem(Item item, ItemRecieveData data)
    {
        if(item.Type == Item.ItemEffectType.Trap)
        {
            if (item is TrapItem trap)
            {
                TrapBase obj = m_middleman_trap.GetTrap(trap.EnumTrap);

                trap.SetTrap(obj);
            }
        }

        item.RecieveData(data);
    }

    public void RandomDropItem(ItemRecieveData r_data , List<ItemData> Items)
    {
        DropList =  EnemyDropList;
        int dropIndex =Random.Range(0, DropList.Count);
        ItemData SelectidItem = Items[dropIndex];
        int Pickupid = SelectidItem.ItemID;
        I_pool.ItemDrop(Pickupid, r_data);
        //return DropList[dropIndex];
        Debug.Log($"{SelectidItem.ItemName}をドロップしました。");
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