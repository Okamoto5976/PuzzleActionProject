using System.Collections.Generic;
using UnityEngine;


public class ItemManager : MonoBehaviour
{
    //public List <Item> DropList=new();
    public List<Item> ItemList = new();
    //private int nextId; //次のIDを管理する変数
    [SerializeField] private Middleman_Trap m_middleman_trap;
    //DropPool I_pool;
    //リスト初期化
    [Header("Debug")]
    [SerializeField] private DropItem m_dropItem;
    [SerializeField] private List<DropItem> DropItems = new();

    //GachaEngine.RarityWithWeight
    //Listの中からIDと同じアイテムを探す
    public Item GetItem(int id)
    {
        Item data = ItemList.Find(x => x.ID == id);

        return data;
    }

    private Item LookForID(int id)
    {
        return ItemList.Find(x => x.ID == id);
    }


    public void ItemUse(int id, Entity entity)  // Entity
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
        //Debug.Log("OnUseItem");

        if (item.Type == Item.ItemEffectType.Trap)
        {
            if (item is TrapItem trap)
            {
                TrapBase obj = m_middleman_trap.GetTrap(trap.EnumTrap);

                if (obj == null)
                {
                    Debug.Log("object Null!!");
                }

                trap.SetTrap(obj);
            }
        }

        item.RecieveData(data);
    }

    public void OnAddPassive(Item item, PlayerController player)
    {
        item.AddPassive(player);
    }

    public void OnRemovePassive(Item item, PlayerController player)
    {
        item.RemovePassive(player);
    }


    [SerializeField] private ComponentPoolHandler_Item m_itemPool;
    public void DropItemSetData(Vector3 pos, Item data)
    {
        //get object"DropItem" from pool        
        //set itemData in DropItem
        //Data data = DropItem(PlayerItems)
        //int index = Random.Range(0, PlayerItems.Count);
        //Item data = PlayerItems[index];
        //int dropIndex = Random.Range(0, DropItems.Count);
        //DropItem m_dropItem = DropItems[dropIndex];
        //m_dropItem.Initialize(data);
        //set pos DropItem Position
        DropItem obj = m_itemPool.GetComponentFromPool();

        obj.Initialize(data);

        //m_dropItem.gameObject.transform.position = pos;
        obj.gameObject.transform.position = pos;


        //foreach (var obj in DropItems)
        //{
        //    if (obj.name.Equals(data.name, System.StringComparison.OrdinalIgnoreCase))
        //    {
        //        DropItem m_dropIndex = obj;
        //        m_dropIndex.Initialize(data);
        //        //set pos DropItem Position
        //        m_dropIndex.gameObject.transform.position = pos;
        //        //m_dropIndex.gameObject.SetActive(true);
        //        //m_dropPool.ItemGet
        //    }

        //}
    }


    public Item DropItem(RarityEnumAsset rarity)
    {
        List<Item> candidates = GetItemsByRarity(rarity);

        if (candidates.Count == 0)
        {
            Debug.LogWarning($"{rarity}のアイテムがありません。");
            return null;
        }
        //candidates.Clear();
        return GetRandomItemFromList(candidates);
    }

    private void LogRarityError(RarityEnumAsset rarity, List<Item> items)
    {
        string msg = $"Could not find item with rarity: {rarity}\n";
        foreach (var item in items)
        {
            msg += $"{item.ItemName} : {item.Data.Rarity}\n";
        }
        Debug.LogWarning(msg, this);
    }
    /// <summary>
    /// Get a list of items with matching rarity
    /// </summary>
    /// <param name="rarity">Rarity to query</param>
    /// <returns>List of items with matching rarity</returns>
    public List<Item> GetItemsByRarity(RarityEnumAsset rarity)
    {
        var candidates = ItemList.FindAll(item => item.Data.Rarity == rarity);
        if (candidates.Count == 0)
        {
            LogRarityError (rarity, ItemList);
        }
        return candidates;
    }
    /// <summary>
    /// Get a random item from items with matching rarity
    /// </summary>
    /// <param name="rarity">Rarity to query</param>
    /// <param name="items">list of items</param>
    /// <returns>a item with matching rarity</returns>
    public Item GetRandomItemFromListByRarity(RarityEnumAsset rarity, List<Item> items)
    {
        var candidates = items.FindAll(x => x.Data.Rarity == rarity);
        if (candidates.Count == 0)
        {
            LogRarityError(rarity, ItemList);
        }
        return GetRandomItemFromList(candidates);
    }
    /// <summary>
    /// Get an item from list of items
    /// </summary>
    /// <param name="items">list of items</param>
    /// <returns>a random item from list</returns>
    public Item GetRandomItemFromList(List<Item> items)
    {
        if (items.Count == 0)
        {
            Debug.LogWarning("List is empty", this);
        }
        return items[Random.Range(0, items.Count)];
    }

    /// <summary>
    /// Get a random item from global item list
    /// </summary>
    /// <returns>a random item from ItemList</returns>
    public Item GetRandomItem()
    {
        return GetRandomItemFromList(ItemList);
    }
    /// <summary>
    /// Get a list of items with IsShopCompatible
    /// </summary>
    /// <returns>a list of items with IsShopCompatible</returns>
    public List<Item> GetShopItems()
    {
        return ItemList.FindAll(x => x.Data.IsShopCompatible);
    }
    /// <summary>
    /// Get a random item with IsShopCompatible
    /// </summary>
    /// <returns>a random item with IsShopCompatible</returns>
    public Item GetRandomShopItem()
    {
        return GetRandomItemFromList(GetShopItems());
    }
    /// <summary>
    /// Get a random item with IsShopCompatible and matching rarity
    /// </summary>
    /// <param name="rarity">rarity to query</param>
    /// <returns>a random item with IsShopCompatible and matching rarity</returns>
    public Item GetRandomShopItemByRarity(RarityEnumAsset rarity)
    {
        return GetRandomItemFromListByRarity(rarity, GetShopItems());
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //DropItemSetData(new Vector3(0, 1, 0));
        }
    }
}
