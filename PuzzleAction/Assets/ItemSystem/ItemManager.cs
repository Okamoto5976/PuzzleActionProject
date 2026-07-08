using System.Collections.Generic;
using UnityEngine;




public class ItemManager : MonoBehaviour
{
    public List<Data> Drop = new();
    public List<Data> DropList = new();
    public List<Data> ShopList = new();
    public List<Data> ItemList = new();
    public List<float> DropRateList = new();
    //プレイヤーが使えるアイテム
    public List<Data> PlayerItems = new();
    //敵専用アイテム
    public List<Data> EnemyItems = new();
    //private int nextId; //次のIDを管理する変数
    [SerializeField] private Middleman_Trap m_middleman_trap;
    //DropPool I_pool;
    //リスト初期化
    [Header("Debug")]
    [SerializeField] private DropItem m_dropItem;
    [SerializeField] private List<DropItem> DropItems = new();

   

    //Listの中からIDと同じアイテムを探す
    //private Item LookForID(int id)
    //{
    //    return ItemList.Find(x => x.Id == id);
    //}


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

    public void OnUseItem(Item item, ItemRecieveData data)
    {
        if (item.Type == Item.ItemEffectType.Trap)
        {
            if (item is TrapItem trap)
            {
                TrapBase obj = m_middleman_trap.GetTrap(trap.EnumTrap);

                trap.SetTrap(obj);
            }
        }

        item.RecieveData(data);
    }

    
    public void DropItemSetData(Vector3 pos)
    {
        //get object"DropItem" from pool        
        //set itemData in DropItem
        Data data = DrowItem2(PlayerItems);
        Debug.Log($"{data.name}({data.grade})"); 
        //int index = Random.Range(0, PlayerItems.Count);
        //Data data = PlayerItems[index];

        int dropIndex = Random.Range(0, DropItems.Count);
        DropItem m_dropItem = DropItems[dropIndex];
        m_dropItem.Initialize(data);
        //set pos DropItem Position
        //m_dropItem.gameObject.transform.position = pos;


    }

    [Header("")]
    [Range(0, 100)] public float Nomal = 60f;
    [Range(0, 100)] public float Rara = 30f;
    [Range(0, 100)] public float SparRara = 10f;

    public Data DrowItem(List <Data>items)
    {
        
        Grade grade = GetRandomRarity();
        List<Data> candidates = items.FindAll(Data => Data.grade == grade);

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

        if (rand < Nomal)
            return Grade.Nomal;

        rand -= Nomal;

        if (rand < Rara)
            return Grade.Rara;

        rand -= Rara;

        if (rand < SparRara)
            return Grade.SparRara;

        return Grade.SparRara;
    }
    public Data DrowItem2(List <Data>items)
    {
        float totalWeight = 0;

        foreach (var item in items)
        {
            totalWeight += item.Rate;
        }

        float randomValue = Random.Range(0, totalWeight);

        foreach (var item in items)
        {
            if (randomValue < item.Rate)
            {
                return item;
            }

            randomValue -= item.Rate;
        }

        return null;
    }
//ランダムにアイテムを渡す
public Item RandomGetItem()
    {
        
        int index = Random.Range(0, ItemList.Count); //アイテムを抽選する
        return ItemList[index]; // アイテムを渡す

    }
    //アイテムのエフェクトを呼び出す

    public void RandomShopItem()
    {
        Data data = DrowItem(ShopList);
        //int ShopIndex = Random.Range(0, ShopList.Count);
        return ;// ShopList[ShopIndex];
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Data data=　DrowItem2(PlayerItems);
           
             DropItemSetData(new Vector3(0, 1, 0));
        }
    }

}
