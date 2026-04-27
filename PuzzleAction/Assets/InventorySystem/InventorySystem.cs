using UnityEngine;
using System.Collections.Generic;

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

    private List<ItemBox> inventory = new();
    [SerializeField] private SlotUI[] slots;
    //Inventory

    private void Awake()
    {
       //slots = GetComponentInChildren<T_SlotUI>();
    }

    private void Start()
    {
        inventory = new();
    }
    [SerializeField] private Data data;

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if(AddItem(data, 1))
            {
                Debug.Log("OK");
            }
            else
            {
                Debug.Log("NO");
            }
        }
    }

    public void OnItem(Data data, int count)
    {
        if(AddItem(data, count))
        {
            UpdateUI();
        }
    }

    public bool AddItem(Data data, int count)
    {
        if (inventory.Count > Count) return false;

        inventory.Add(new ItemBox(data, count));
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
        for (int i = 0; i < Count; i++)
        {
            if (i >= inventory.Count)
            {
                slots[i].Clear();
            }
            else
            {
                slots[i].SetItem(inventory[i], i);
            }
        }
    }


    //削除
    public void RemoveItem(int index)
    {
        if (index >= inventory.Count) return;
        inventory.RemoveAt(index);
        for (int i = 0; i < hotbares.Length; i++)
        {
            if (hotbares[i] < index) continue;
            if (hotbares[i] > index)
            {
                hotbares[i]--;
                continue;
            }
            if (hotbares[i] == index)
            {
                hotberClear(i);
            }
        }
    }

    // 使用
    public void UseItem(int index)
    {
        if (index >= inventory.Count) return;
        ItemBox item = inventory[index];

        item.count--;

        if (item.count <= 0)
        {
            RemoveItem(index);
        }
    }

    //hotber

    //public int[] hotbares = new int[3];
    public int[]hotbares = new int[3];


    //ホットバーに追加
    public void AddHotber(int hotberNumber, int index)
    {
        hotbares[hotberNumber] = index;
    }

    //使用
    public void Use(int hotberNumber)
    {
        int index = hotbares[hotberNumber];
        if (index < 0) return;

        UseItem(index);
    }


    //インベントリ削除時クリア
    public void hotberClear(int hotbarNumber)
    {
        hotbares[hotbarNumber] = -1;
    }
}