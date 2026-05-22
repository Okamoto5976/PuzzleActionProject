using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class ItemBox
{
    public Data data;
    public int count;

    public ItemBox(Data data, int count)
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
    private void Awake()
    {
        activeSlots = activePanel.GetComponentsInChildren<SlotUI>(true);

        passiveSlots = passivePanel.GetComponentsInChildren<SlotUI>(true);

        hotbarSlots = hotbarPanel.GetComponentsInChildren<SlotUI>(true);

        Debug.Log("Active Slots : " + activeSlots.Length);
        Debug.Log("Passive Slots : " + passiveSlots.Length);
        Debug.Log("Hotbar Slots : " + hotbarSlots.Length);
    }

    private void Start()
    {
        for (int i = 0; i < hotbars.Length; i++)
        {
            hotbars[i] = -1;
        }
    }
    [SerializeField] private Data data;

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

    public void OnItem(Data data, int count)
    {
        AddItem(data, count);
    }

    public bool AddItem(Data data, int count)
    {
       // Activeアイテム
       if (data.itemType == ItemType.Active)
        {
            return AddActiveItem(data, count);
        }

        // Passiveアイテム
        else
        {
            return AddPassiveItem(data, count);
        }
    }

    private bool AddActiveItem(Data data, int count)
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

    private bool AddPassiveItem(Data data, int count)
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

    public void UpdateUI()
    {
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
    public void UseItem(int index)
    {
        if (index >= activeInventory.Count) return;

        ItemBox item = activeInventory[index];

        item.count--;

        Debug.Log(item.data.itemName + " を使用");

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
    }

    //使用
    public void Use(int hotberNumber)
    {
        int index = hotbars[hotberNumber];
        if (index < 0) return;

        UseItem(index);
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

}