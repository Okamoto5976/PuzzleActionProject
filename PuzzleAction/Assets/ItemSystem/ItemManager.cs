using System.Collections.Generic;
using UnityEngine;
//public enum RarityRate
//{
//    Nomal,
//    Rara,
//    SparRara
//}
//[System.Serializable]
//public class Rate
//{
//    public Item Name;
//    public Item DropRate;
//    public RarityRate rarity;
//    public int weight = 1;
//}
public class ItemManager : MonoBehaviour
{
  //  public List<Rate> Drop = new();
    public List<Item> DropList = new();
    public List<Item> ShopList = new();
    public List<Item> ItemList = new();
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

    //public List<Rate> Items = new();
    ////public InfoText Rate;

    //[Header("")]
    //[Range(0, 100)] public float Nomal = 60f;
    //[Range(0, 100)] public float Rara = 30f;
    //[Range(0, 100)] public float SparRara = 10f;

    //public Rate DrawItem()
    //{

    //    RarityRate rarity = GetRandomRarity();
    //    List<Rate> candidates = Drop.FindAll(Rate => Rate.rarity == rarity);

    //    if (candidates.Count == 0)
    //    {
    //        Debug.Log($"{rarity}のアイテムがありません。再抽選します。");
    //        return DrawItem();
    //    }

    //    int index = Random.Range(0, candidates.Count);
    //    return candidates[index];

    //}

    //private RarityRate GetRandomRarity()
    //{
    //    float rand = Random.Range(0f, 100f);

    //    if (rand < Nomal)
    //        return RarityRate.Nomal;

    //    rand -= Nomal;

    //    if (rand < Rara)
    //        return RarityRate.Rara;

    //    rand -= Rara;

    //    if (rand < SparRara)
    //        return RarityRate.SparRara;

    //    return RarityRate.SparRara;

        
    //}
    public void DropItemSetData(Vector3 pos)
    {
        //get object"DropItem" from pool        
        //set itemData in DropItem

        int index = Random.Range(0, PlayerItems.Count);
        Data data = PlayerItems[index];
        //int dropIndex = Random.Range(0, DropItems.Count);
        //DropItem m_dropItem = DropItems[dropIndex];
        m_dropItem.Initialize(data);
        //set pos DropItem Position
        m_dropItem.gameObject.transform.position = pos;


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
        int ShopIndex = Random.Range(0, ShopList.Count);
        return ShopList[ShopIndex];
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropItemSetData(new Vector3(0, 1, 0));
        }
    }
    //public void Updaet()
    //{
    //    if(Input.GetKeyDown(KeyCode.Q))
    //    {
    //       Rate rate= DrawItem();
    //        Debug.Log($"{rate.Name}({rate.rarity})");
    //    }
        

    //}
}
