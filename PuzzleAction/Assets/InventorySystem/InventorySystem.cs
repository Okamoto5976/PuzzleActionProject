using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

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

    [SerializeField] private List<Image> mainHotbarImages;

    private ItemManager m_itemManager;

    private void Awake()
    {
        activeSlots = activePanel.GetComponentsInChildren<SlotUI>(true);

        passiveSlots = passivePanel.GetComponentsInChildren<SlotUI>(true);

        hotbarSlots = hotbarPanel.GetComponentsInChildren<SlotUI>(true);

        m_itemManager = FindAnyObjectByType<ItemManager>();
    }

    private void Start()
    {
        for (int i = 0; i < hotbars.Length; i++)
        {
            hotbars[i] = -1;
        }
        m_saveManager.LoadFromJson();

        UpdateUI();
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
        else// Passiveアイテム
        {
            return AddPassiveItem(data, count);
        }


    }

    private bool AddActiveItem(Item data, int count)
    {
        //同じアイテムを探す
        foreach (ItemBox item in activeInventory)
        {
            if (item != null && item.data == data)
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
        //Debug.Log("===== UpdateUI =====");
        //Debug.Log($"UpdateUi Start = [{hotbars[0]},{hotbars[1]}, {hotbars[2]}]");

        if (hotbars[0] == 0)
        {
            //Debug.LogWarning("hotbars[0] became 0 !");
            //Debug.Log(System.Environment.StackTrace);
        }

        //Debug.Log($"hotbars = [{hotbars[0]}, {hotbars[1]}, {hotbars[2]}]");

        for (int i = 0; i < activeInventory.Count; i++) 
        {
            if (activeInventory[i] != null)
            {
                //Debug.Log($"{i} : {activeInventory[i].data.ItemName}");
            }
        }
        //Debug.Log($"hotbars = [{hotbars[0]}, {hotbars[1]}, {hotbars[2]}]");

        // Active inventory
        for (int i = 0; i < activeSlots.Length; i++)
        {
            if (i >= activeInventory.Count || activeInventory[i] == null)
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
            //Debug.Log($"Hotbar Check i={i} index={index}");

            // 空なら消す
            if (index < 0)
            {
                //Debug.Log($"Hotbar[{i}] Clear");
                hotbarSlots[i].Clear();
                continue;
            }

            // インデックス範囲外
            if (index >= activeInventory.Count || activeInventory[index] == null)
            {
                hotbarSlots[i].Clear();
                hotbarClear(i);
                continue;
            }

            // 表示更新
            hotbarSlots[i].SetItem(activeInventory[index], index);
        }
        OnUpdateMainHotber();
    }

    public void RemoveActiveItem(int index)
    {
        if (index >= activeInventory.Count) return;

        ItemBox item = activeInventory[index];

        item.count--;
        // 0以下なら完全削除
        if (item.count <= 0)
        {
            activeInventory[index] = null;
            for (int i = 0; i < hotbars.Length; i++)
            {
                if (hotbars[i] == index)
                {
                    hotbarClear(i);
                }
            }

            UpdateUI();
        }
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
        if (index < 0 || index >= activeInventory.Count) return;

        activeInventory[index] = null;

        for (int i = 0; i < hotbars.Length; i++)
        {
            if (hotbars[i] == index)
            {
                hotbarClear(i);
            }
        }

        UpdateUI();
    }



    // 使用
    private void UseItem(int index, ItemRecieveData data)
    {
        if (index < 0 || index >= activeInventory.Count) return;

        if (activeInventory[index] == null) return;

        ItemBox item = activeInventory[index];

        item.count--;

        Debug.Log(item.data.ItemName + " を使用");

        //m_itemManager.OnUseItem(item.data, data);
        //ItemManager
        //if(m_itemManager == null)
        //{
        //    Debug.Log("ItemManager ねえよ");
        //}
        
        //if(item.data == null)
        //{
        //    Debug.Log("item data ねえよ");

        //}

        //if (data == null)
        //{
        //    Debug.Log("data ねえよ");

        //}

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
    private int[] hotbars = new int[] { -1, -1, -1 };

    [SerializeField] private DisplayManager m_displayManager;


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
        Debug.Log($"AddHotber called : {hotberNumber}, {index}");
        hotbars[hotberNumber] = index;

        hotbarSlots[hotberNumber].SetItem(activeInventory[index], index);
        m_displayManager.SetHotberImage(hotberNumber, activeInventory[index].data.icon);

        UpdateUI();
    }

    public bool IsCheckCurrentItem(int hotbarNumber, ItemUseType type)
    {
        int index = hotbars[hotbarNumber];

        if (index < 0 || index >= activeInventory.Count) return false;

        if (activeInventory[index] == null) return false;

        ItemBox item = activeInventory[index];

        return item.data.ItemUseType == type;

    }

    //使用
    public void UsePressed(int hotbarNumber, ItemRecieveData data)
    {
        int index = hotbars[hotbarNumber];
        if (index < 0 || index >= activeInventory.Count) return;

        UseItem(index, data);
    }

    //public void UseHold(int hotbarNumber, ItemRecieveData data)
    //{
    //    int index = hotbars[hotbarNumber];
    //    if (index < 0) return;

    //    UseItem(index, data);

    //}

    public void UseRelease(int hotbarNumber, ItemRecieveData data)
    {
        int index = hotbars[hotbarNumber];
        if (index < 0 || index >= activeInventory.Count) return;


        UseItem(index, data);

    }


    //インベントリ削除時クリア
    public void hotbarClear(int hotbarNumber)
    {
        //Debug.Log($"hotbarClear called : {hotbarNumber}");

        m_displayManager.ResetHotberImage(hotbarNumber);

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
        Debug.Log($"LoadInventory = [{hotbars[0]}, {hotbars[1]}, {hotbars[2]}]");
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

    private void OnUpdateMainHotber()
    {


        return;
        Debug.Log("===== OnUpdateMainHotber START =====");

        //int count = Mathf.Min(
        //    hotbars.Length,
        //    mainHotbarImages.Length
        //);

        int count = hotbars.Length;

        Debug.Log(count);

        for (int i = 0; i < count; i++)
        {
            if (mainHotbarImages[i] == null)
            {
                Debug.Log("なにもない");
            }

            Image hotbarImage = mainHotbarImages[i];

            if (hotbarImage == null)
            {
                continue;
            }

            if (hotbars[i] == -1)
            {
                hotbarImage.sprite = null;
                //hotbarImage.enabled = false;
                continue;
            }

            int inventoryIndex = hotbars[i];

            if (inventoryIndex < 0 ||
                inventoryIndex >= activeInventory.Count)
            {
                hotbarImage.sprite = null;
                //hotbarImage.enabled = false;
                continue;
            }

            ItemBox item = activeInventory[inventoryIndex];

            if (item == null || item.data == null)
            {
                hotbarImage.sprite = null;
                //hotbarImage.enabled = false;
                continue;
            }

            hotbarImage.sprite = item.data.icon;
            //hotbarImage.enabled = true;

            Debug.Log(
                $"MainHotbar[{i}]に " +
                $"{item.data.ItemName} のImageを更新"
            );
        }
        Debug.Log("===== OnUpdateMainHotber END =====");
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
    }
}