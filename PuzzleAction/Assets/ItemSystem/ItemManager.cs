using System.Collections.Generic;
using UnityEngine;


public class ItemManager : MonoBehaviour
{

    //public List <Item> DropList=new();
    public List<Item> ShopList = new();
    public List<Item> ItemList = new();
    public List<float> DropRateList = new();
    //プレイヤーが使えるアイテム
    public List<Item> PlayerItems = new();
    //敵専用アイテム
    public List<Item> EnemyItems = new();
    //private int nextId; //次のIDを管理する変数
    [SerializeField] private Middleman_Trap m_middleman_trap;
    //DropPool I_pool;
    //リスト初期化
    [Header("Debug")]
    [SerializeField]private DropItem m_dropItem;
    [SerializeField] private List<DropItem> DropItems = new();

    //Listの中からIDと同じアイテムを探す
    public Item GetItem(int id)
    {
        Item data = ItemList.Find(x=>x.ID == id);

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
        Debug.Log("OnUseItem");

        if(item.Type == Item.ItemEffectType.Trap)
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



    public void DropItemSetData(Vector3 pos)
    {
        //get object"DropItem" from pool        
        //set itemData in DropItem
        //Data data = DrowItem(PlayerItems)
        int index = Random.Range(0, PlayerItems.Count);
        Item data = PlayerItems[index];
        //int dropIndex = Random.Range(0, DropItems.Count);
        //DropItem m_dropItem = DropItems[dropIndex];
        m_dropItem.Initialize(data);
        //set pos DropItem Position
        m_dropItem.gameObject.transform.position = pos;

        foreach (var obj in DropItems)
        {
            if (obj.name.Equals(data.name, System.StringComparison.OrdinalIgnoreCase))
            {
                DropItem m_dropIndex = obj;
                m_dropIndex.Initialize(data);
                //set pos DropItem Position
                m_dropIndex.gameObject.transform.position = pos;
            }

        }
    }

    [Header("排出確率")]

    [Range(0, 100)] public float Comon = 50f;
    [Range(0, 100)] public float UnComon = 30f;
    [Range(0, 100)] public float Rara = 15f;
    [Range(0, 100)] public float Legend = 5f;

    public Item DrowItem(List<Item> items)
    {
        Grade grade = GetRandomRarity();
        List<Item> candidates = items.FindAll(Data => Data.grade == grade);

        if (candidates.Count == 0)
        {
            Debug.Log($"{grade}のアイテムがありません。再抽選します。");
            return DrowItem(items);
        }

        int index = Random.Range(0, candidates.Count);
        //candidates.Clear();
        return candidates[index];
    }

    private Grade GetRandomRarity()
    {

        float rand = Random.Range(0f, 100f);

        if (rand < Comon)
            return Grade.Comon;

        rand -= Comon;

        if (rand < UnComon)
            return Grade.UnComon;

        if (rand < Rara)
            return Grade.Rara;

        rand -= Rara;

        if (rand < Legend)
            return Grade.Legend;

        return Grade.Legend;
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
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //DropItemSetData(new Vector3(0, 1, 0));
        }
    }
}
