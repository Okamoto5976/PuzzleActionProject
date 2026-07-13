using UnityEngine;
using System.Collections.Generic;

public class ItemBox
{
    public Item data;
    public int count;

    public ItemBox(Item data, int count)
    {
        this.data = data;
        this.count = count;
    }
}


public class InventorySystem : MonoBehaviour
{
    private int m_width = 10;
    private int m_height = 3;
    private int Count => m_width * m_height;

    private List<ItemBox> activeInventory = new();
    private List<ItemBox> passiveInventory = new();

    //[SerializeField] private SlotUI[] slots;
    [SerializeField] private SlotUI[] hotbarSlots;

    //Inventory
    //[SerializeField] private GameObject inventoryPanel; 
    [SerializeField] private SlotUI[] activeSlots;
    [SerializeField] private SlotUI[] passiveSlots;

    [SerializeField] private GameObject activePanel;
    [SerializeField] private GameObject passivePanel;
    [SerializeField] private GameObject hotbarPanel;

    [SerializeField] private SaveData saveData;
    [SerializeField] private ItemManager itemManager;
    [SerializeField] private SaveManager m_saveManager;

    private ItemManager m_itemManager;

    private void Awake()
    {
        activeSlots = activePanel.GetComponentsInChildren<SlotUI>(true);

        passiveSlots = passivePanel.GetComponentsInChildren<SlotUI>(true);

        hotbarSlots = hotbarPanel.GetComponentsInChildren<SlotUI>(true);

        m_itemManager = FindAnyObjectByType<ItemManager>();

        //Debug.Log("Active Slots : " + activeSlots.Length);
        //Debug.Log("Passive Slots : " + passiveSlots.Length);
        //Debug.Log("Hotbar Slots : " + hotbarSlots.Length);
    }

    private void Start()
    {
        for (int i = 0; i < hotbars.Length; i++)
        {
            hotbars[i] = -1;
        }
        m_saveManager.LoadFromJson();
    }
    //[SerializeField] private Data data;

    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.E))
    //    {
    //        if(AddItem(data, 1))
    //      {
    //            Debug.Log("OK");
    //        }
    //        else
    //        {
    //            Debug.Log("NO");
    //        }
    //    }
    //}

    public void OnItem(Item data, int count)
    {
        if (AddItem(data, count))
        {
           
        }
    }
    public void Save()
    {
        SaveInventory();
        m_saveManager.SaveToJson();
        Debug.Log("セーブしました");
    }

    public bool AddItem(Item data, int count)
    {
       // Activeアイテム
       if (data.ItemType == ItemType.Active)
        {
            return AddActiveItem(data, count);
        }

        // Passiveアイテム
        else
        {
            return AddPassiveItem(data, count);
        }
    }

    private bool AddActiveItem(Item data, int count)
    {
        //同じアイテムを探す
        foreach (ItemBox item in activeInventory)
        {
            if (item.data == data)
            {
                item.count += count;

                UpdateUI();

                return true;
            }
        }

        // 空き無し
        if (activeInventory.Count >= 30)
        {
            return false;
        }
        // 新規追加
        activeInventory.Add(new ItemBox(data, count));

        UpdateUI();

        return true;
    }

    private bool AddPassiveItem(Item data, int count)
    {
        // Passiveはスタックしない
        if (passiveInventory.Count >= 30)
        {
            return false;
        }

        passiveInventory.Add(new ItemBox(data, 1));

        UpdateUI();

        return true;
    }



    public void UpdateUI()
    {
        Debug.Log("UpdateUI");
        Debug.Log($"hotbars = [{hotbars[0]}, {hotbars[1]}, {hotbars[2]}]");

        // Active inventory
        for (int i = 0; i < activeSlots.Length; i++)
        {
            if (i >= activeInventory.Count)
            {
                activeSlots[i].Clear();
            }
            else
            {
                activeSlots[i].SetItem(activeInventory[i], i);
            }
        }

        // Passive inventory
        for (int i = 0; i < passiveSlots.Length; i++)
        {
            if (i >= passiveInventory.Count)
            {
                passiveSlots[i].Clear();
            }
            else
            {
                passiveSlots[i].SetItem(passiveInventory[i], i);
            }
        }

// Hotbar
for (int i = 0; i < hotbars.Length; i++)
{
    int index = hotbars[i];

    // 空なら消す
    if (index < 0)
    {
        hotbarSlots[i].Clear();
        continue;
    }

    // インデックス範囲外
    if (index >= activeInventory.Count)
    {
        hotbarSlots[i].Clear();
        hotbarClear(i);
        continue;
    }

    // 表示更新
    hotbarSlots[i].SetItem(activeInventory[index], index);
}
    }

    public void RemoveActiveItem(int index)
    {
        if (index >= activeInventory.Count) return;

        ItemBox item = activeInventory[index];

        item.count--;
        // 0以下なら完全削除
        if (item.count <= 0)
        {
            activeInventory.RemoveAt(index);
        }

        UpdateUI();
    }

    public void RemovePassiveItem(int index)
    {
        if (index >= passiveInventory.Count) return;

        passiveInventory.RemoveAt(index);

        UpdateUI();
    }


    //削除
    public void RemoveItem(int index)
    {
        if (index >= activeInventory.Count) return;

        activeInventory.RemoveAt(index);

        for (int i = 0; i < hotbars.Length; i++)
        {
            if (hotbars[i] < index) continue;

            if (hotbars[i] > index)
            {
                hotbars[i]--;
                continue;
            }

            if (hotbars[i] == index)
            {
                hotbarClear(i);
            }
        }

        UpdateUI();
    }

    // 使用
    public void UseItem(int index, ItemRecieveData data)
    {
        if (index >= activeInventory.Count) return;

        ItemBox item = activeInventory[index];

        item.count--;

        Debug.Log(item.data.ItemName + " を使用");
        //ItemManager
        m_itemManager.OnUseItem(item.data, data);

        // 0以下なら削除
        if (item.count <= 0)
        {
            RemoveItem(index);
            return;
        }

        UpdateUI();
    }

    //hotber

    //public int[] hotbares = new int[3];
    public int[]hotbars = new int[3];


    //ホットバーに追加
    public void AddHotber(int hotberNumber, int index)
    {
        // 1. 引数の hotberNumber が配列の範囲内かチェック
        if (hotberNumber < 0 || hotberNumber >= hotbarSlots.Length) return;

        // 2. 引数の index が現在のインベントリの範囲内かチェック
        if (index < 0 || index >= activeInventory.Count)
        {
            Debug.LogWarning($"インデックス {index} はインベントリの範囲外です。リセットします。");
            hotbarClear(hotberNumber);
            hotbarSlots[hotberNumber].Clear();
            return;
        }

        hotbars[hotberNumber] = index;

        hotbarSlots[hotberNumber].SetItem(activeInventory[index], index);

        //追加
        Debug.Log($"AddHotber : Slot={hotberNumber}, Index={index}");
        Debug.Log($"hotbars =[{ hotbars[0]}, { hotbars[1]}, { hotbars[2]}]");

    }

    //使用
    public void Use(int hotberNumber, ItemRecieveData data)
    {
        int index = hotbars[hotberNumber];
        if (index < 0) return;

        UseItem(index, data);
    }


    //インベントリ削除時クリア
    public void hotbarClear(int hotbarNumber)
    {
        hotbars[hotbarNumber] = -1;
    }

    //playerからホットバーのアイテム使用ボタンが押される
    //入っていないならreturn
    //入っているならItemManagerに使用するように呼ぶ
    //Playerからデータを渡されるため、データをItemManagerに渡す。
    public void UseItemToPlayer(int number) //PlayerからのDataを引数に
    {
        //

        //UseItem();ホットバーのアイテムを
    }

    //SaveDataを渡す処理
    public void SaveInventory()
    {
        saveData.activeItems.Clear();
        saveData.passiveItems.Clear();

        foreach (ItemBox item in activeInventory)
        {
            SaveItemData saveItem = new SaveItemData();

            saveItem.id = item.data.ID;
            saveItem.count = item.count;

            saveData.activeItems.Add(saveItem);
        }

        foreach (ItemBox item in passiveInventory)
        {
            SaveItemData saveItem = new SaveItemData();

            saveItem.id = item.data.ID;
            saveItem.count = item.count;

            saveData.passiveItems.Add(saveItem);
        }

        Debug.Log("=== Active ===");

        foreach (SaveItemData item in saveData.activeItems)
        {
            Debug.Log($"ID:{item.id} Count:{item.count}");
        }

        Debug.Log("=== Passive ===");

        foreach (SaveItemData item in saveData.passiveItems)
        {
            Debug.Log($"ID:{item.id} Count:{item.count}");
        }
    }

    public void LoadInventory()
    {
        activeInventory.Clear();
        passiveInventory.Clear();

        foreach (SaveItemData saveItem in saveData.activeItems)
        {
            Debug.Log($"ロード中 ID:{saveItem.id}");

            Item data = itemManager.GetItem(saveItem.id);

            if (data != null)
            {
                Debug.Log($"取得成功 {data.ItemName}");

                activeInventory.Add(
                    new ItemBox(data, saveItem.count)
                );
            }
        }

        foreach (SaveItemData saveItem in saveData.passiveItems)
        {
            Item data = itemManager.GetItem(saveItem.id);

            if (data != null)
            {
                passiveInventory.Add(
                    new ItemBox(data, saveItem.count)
                );
            }
        }

        UpdateUI();
    }

    //ソート　アイテム削除後などに
    private void InventorySort()
    {
        //List<ItemBox> list = new List<ItemBox>();

        //for(int y = 0; y < m_height; ++y)
        //{
        //    for(int x = 0; x < m_width;  ++x)
        //    {
        //        if(inventory[x, y] != null)
        //        {
        //            list.Add(inventory[x, y]);
        //        }
        //    }
        //}

        //for (int y = 0;y < m_height; ++y)
        //{
        //    for (int x = 0; x < m_width; ++x)
        //    {
        //        inventory[x, y] = null;
        //    }
        //}

        //int index = 0;

        //for (int y = 0; y<m_height; ++y)
        //{
        //    for (int x = 0; x<m_width; ++x)
        //    {
        //        if (index < list.Count)
        //        {
        //            inventory[x, y] = list[index];
        //            index++;
        //        }
        //    }
        //}
    }}