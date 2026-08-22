using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct ShopInventory {
    public List<ShopItem> inventory;
}

[System.Serializable]
public struct ShopItem
{
    public Item data;
    public bool IsSold;

    public void BuyItem()
    {
        IsSold = true;
    }
}


public class ShopManager : MonoBehaviour
{
    [Header("TestItemDataManager")]
    //[SerializeField] private List<Data> t_itemData;


    //private List<Data> t_itemDataList = new();

    [SerializeField] private IntRunTime m_currentLevel;
    [SerializeField] private TemporaryItemManager m_temporaryItemManager;   
    [SerializeField] private EventBusAsset m_onGenerateShopInventories;
    [SerializeField] private InstanceCounter m_shopCount;
    [SerializeField] private IntEventSO m_shopIdEvent;
    [SerializeField] private MessageManager m_messageManager;


    [SerializeField] private List<Goods> m_goodsPrefab;

    private int SlotCount => m_goodsPrefab.Count;


    [SerializeField] private TextMeshProUGUI m_moneyText;

    //仮　MoneyのDataSOを持つ
    //[SerializeField] private IntRunTime m_moneyDataSO;
    //private int m_money;

    //InfoText Prefab
    [SerializeField] private InfoText m_infoTextPrefab;
    [SerializeField] private InventorySystem m_inventorySystem;

    private List<Item> m_passiveItems;
    private List<Item> m_activeItems;

    [SerializeField] private List<ShopInventory> m_shopInventories;


    private int ShopCount => m_shopCount.Count;
    private int _currentShopId = 0;

    //========Debug===============
    [Header("Debug")]
    [SerializeField] private bool m_isDebug;
    [SerializeField] private GameObject m_ShopUI;
    [SerializeField] private int m_shopId = 1;

    [ContextMenu("Debug.Start")]
    public void DebugStart()
    {
        if(m_ShopUI == null)
        {
            Debug.LogError("Debug Serialize ShopUI not found");
            return;
        }

        m_ShopUI.SetActive(true);
        m_shopIdEvent.Raise(m_shopId);
    }

    //仮　Initializeで呼ぶ
    private void Awake()
    {
        //m_money = m_moneyDataSO.Value;

        m_infoTextPrefab.gameObject.SetActive(false);
        //m_moneyText.text = "money :" + m_money.ToString();

        InitSlots();
        InitializeSellableItems();
        m_shopInventories = new();
    }

    private void OnEnable()
    {
        m_onGenerateShopInventories.OnTrigger += InitializeShops;
        m_shopIdEvent.Register(SetDatasToSlots);
    }

    private void OnDisable()
    {
        m_onGenerateShopInventories.OnTrigger -= InitializeShops;
        m_shopIdEvent.Unregister(SetDatasToSlots);
    }

    /// <summary>
    /// Gets shop compatible active and passive items
    /// </summary>
    private void InitializeSellableItems()
    {
        var items = m_temporaryItemManager.GetShopItems();
        m_activeItems = items.Where(x => x.ItemType == ItemType.Active).ToList();
        m_passiveItems = items.Where(x => x.ItemType == ItemType.Passive).ToList();

        Debug.Log($"active : {m_activeItems.Count}, passive : {m_passiveItems.Count}");
    }

    /// <summary>
    /// Initializes UI Slots
    /// </summary>
    private void InitSlots()
    {
        for(int i = 0; i < SlotCount;i++)
        {
            m_goodsPrefab[i].Init(this, i);
        }
    }

    /// <summary>
    /// Creates inventory for shops
    /// </summary>
    private void InitializeShops()
    {
        if(m_isDebug)
        {
            for (int i = 0; i < 3; i++)
            {
                m_shopInventories.Add(GenerateShopInventory(1, 1));
            }

            return;
        }

        for (int i = 0; i < ShopCount; i++)
        {
            m_shopInventories.Add(GenerateShopInventory(1, 1));
        }
    }

    private ShopInventory GenerateShopInventory(int passiveCount, float passiveWeight)
    {
        ShopInventory newInventory = new();
        newInventory.inventory = new();

        System.Random random = new();
        int passiveItemCount = 0;
        if (passiveCount > 0)
        {
            int passiveWeightValue = Random.Range(0, 100) - (int)(passiveWeight * 100);
            if (passiveWeightValue < 0)
            {
                passiveItemCount = (int)((Mathf.Abs(passiveWeightValue) + (100.0 / passiveCount) - 1) / (int)(passiveWeight * 100));
            }
        }
        int activeItemCount = SlotCount - passiveItemCount;
        Debug.Log($"a : {activeItemCount}, p : {passiveItemCount}");

        var items = m_activeItems
            .OrderBy(x => random.Next())
            .Take(activeItemCount)
            .ToList();
        items.AddRange(m_passiveItems
                .OrderBy(x => random.Next())
                .Take(passiveItemCount)
                .ToList());

        for (int i = 0; i < SlotCount; i++)
        {
            ShopItem item = new();
            item.IsSold = false;
            item.data = items[i];
            newInventory.inventory.Add(item);
        }
        return newInventory;
    }

    private void SetDatasToSlots(int id)
    {
        SetShopText();

        SetDatasToSlotsFromInventory(m_shopInventories[id]);
        //textManager start
        _currentShopId = id;

        m_messageManager?.MessageDisplayRandom(Enum_ShopMessageType.Welcome);
    }

    private void SetShopText()
    {
        m_moneyText.text = "money :" + GameManager.Instance.Money.ToString();

    }

    private void SetDatasToSlotsFromInventory(ShopInventory shopInventory)
    {
        for (int i = 0; i < SlotCount; i++)
        {
            m_goodsPrefab[i].SetData(shopInventory.inventory[i]);
        }
    }

    //ショップが開かれる際に呼ばれること
    //Awakeの一部をこちらに移植
    //ItemManagerの関数[ランダムにItemDataを渡す]を呼びDataを受け取る
    //Listに格納
    public void Initialize()
    {
        //for(int i = 0; i < mk_itemNumber; i++)
        //{
        //ItemData data = ItemManager.GetItem();
        //
        //m_list.Add(data);
        //
        //list[index]をm_goodPrefab[index]に
        //m_goodsPrefab[i].Init(data);
        //
        //
        //}
    }

    public bool PurchaseItem(int slotId)
    {
        Debug.Log($"{_currentShopId}, {slotId}, {m_shopInventories[_currentShopId].inventory[slotId].IsSold}");
        var data = m_shopInventories[_currentShopId].inventory[slotId].data;

        int money = GameManager.Instance.Money;

        //少ない　購入出来ない場合
        if (data.Data.Price > money)
        {
            Debug.Log("you do not have money");

            m_messageManager?.MessageDisplayRandom(Enum_ShopMessageType.NoMoney);
            return false;
        }
        else
        {
            Debug.Log("you purchase item");

            int value = -(data.Data.Price);

            if(!GameManager.Instance.ModifyMoney(value))
            {
                Debug.LogError("Modify over");
            }

            m_moneyText.text = "money :" + GameManager.Instance.Money.ToString();//再び最新を表示

            //InventoryManagerにItemを渡す
            m_inventorySystem.AddItem(data, 1);

            var item = m_shopInventories[_currentShopId].inventory[slotId];
            item.IsSold = true;
            m_shopInventories[_currentShopId].inventory[slotId] = item;
            Debug.Log($"{_currentShopId}, {slotId}, {m_shopInventories[_currentShopId].inventory[slotId].IsSold}");

            m_messageManager?.MessageDisplayRandom(Enum_ShopMessageType.Buy);

            return true;
        }
    }

    //説明文表示
    public void OnInfoPanelFromGoods(ItemData data)
    {
        m_infoTextPrefab.gameObject.SetActive(true);
        m_infoTextPrefab.GetItemDataInfo(data);
    }

    //説明文非表示
    public void OffInfoPanelFromGoods()
    {
        m_infoTextPrefab.Reset();
        m_infoTextPrefab.gameObject.SetActive(false);
    }
}
