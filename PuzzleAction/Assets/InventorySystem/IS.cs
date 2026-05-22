using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class T_ItemBox
{
    public Data data;
    public int count;

    public T_ItemBox(Data data, int count)
    {
        this.data = data;
        this.count = count;
    }
}

public class IS : MonoBehaviour
{
    //アイテム移動は無くてもよい　＝＞　位置を変える必要はそこまでない　ホットバーに入れるのも中身を移動するものではなく参照を取るようにする
    //アイテムにはパッシブとアクティブがある　アクティブは個数スタックで持てて　パッシブは一つのみ
    //インベントリではアイテム追加アイテム削除
    //配列にItemBox(中にDataとスタック数）を入れれるようにする
    //ホットバーにはItemBoxのIndexを渡すことで　ホットバーのアイテムを使用＝＞インベントリで使用＝＞ホットバー＆インベントリから削除

    //UI
    //Image(button)のプレハブを作り　中にInventoryIcon（仮）を入れ　Indexが入るようにする
    //buttonを押した際OnclickでIncentoryのindexをみて　ItemDataを取る　＜＝　ホットバー役のボタン
    //buttonを押した際ホットバーに入れる＆アイテム削除を表示　＜＝　インベントリ役のボタン

    private int m_width = 10;
    private int m_height = 3;


    private T_ItemBox[,] inventory;
    [SerializeField] private T_SlotUI[] slots;

    private void Awake()
    {
        //slots = GetComponentInChildren<T_SlotUI>();
    }

    private void Start()
    {
        inventory = new T_ItemBox[m_width, m_height];
    }

    [SerializeField] private Data data;

    private void Update()
    {
        if(Input.GetKey(KeyCode.E))
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

    public bool AddItem(Data data, int count)
    {
        for (int y = 0; y < m_height; y++)
        {
            for (int x = 0; x < m_width; x++)
            {
                if (inventory[x, y] != null && inventory[x,y].data == data)
                {
                    inventory[x, y].count += count;
                    return true;
                }
            }
        }

        for (int y = 0; y < m_height;y++)
        {
            for(int x = 0; x < m_width;x++)
            {
                if (inventory[x,y] == null)
                {
                    inventory[x, y] = new T_ItemBox(data, count);
                    //inventory[x, y] = new T_ItemBox();
                    //inventory[x, y].data = data;
                    //inventory[x, y].count += count;
                    return true;
                }
            }
        }

        return false;
    }

    //ソート　アイテム削除後などに
    private void InventorySort()
    {
        List<T_ItemBox> list = new List<T_ItemBox>();

        for(int y = 0; y < m_height; ++y)
        {
            for(int x = 0; x < m_width; ++x)
            {
                if(inventory[x, y] != null)
                {
                    list.Add(inventory[x, y]);
                }
            }
        }

        for (int y = 0; y < m_height; ++y)
        {
            for (int x = 0; x < m_width; ++x)
            {
                inventory[x, y] = null;
            }
        }

        int index = 0;

        for (int y = 0; y < m_height; ++y)
        {
            for (int x = 0; x < m_width; ++x)
            {
                if(index < list.Count)
                {
                    inventory[x, y] = list[index];
                    index++;
                }
            }
        }
    }

    public void UpdateUI()
    {
        for (int y = 0; y < m_height; ++y)
        {
            for (int x = 0; x < m_width; ++x)
            {
                var slot = slots[x + y *  m_width];

                if (inventory[x, y] != null)
                    slot.SetItem(inventory[x, y]);
                else
                    slot.Clear();
            }
        }
    }
    // ==========Inventory================
    //public ItemBox[] slots = new ItemBox[30];

    //public bool AddItem(Data data, int count)
    //{
    //    for (int i = 0; i < slots.Length; i++)//同じものがある場合スタック
    //    {
    //        if (slots[i] != null && slots[i].data == data)
    //        {
    //            slots[i].count += count;
    //            return true;
    //        }
    //    }

    //    for (int i = 0; i < slots.Length; i++)
    //    {
    //        if (slots[i] == null)
    //        {
    //            slots[i] = new ItemBox(data, count);
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    ////削除
    //public void RemoveItem(int index)
    //{
    //    slots[index] = null;
    //}

    ////使用
    //public void UseItem(int index)
    //{
    //    ItemBox item = slots[index];
    //    if (item == null) return;

    //    item.count--;
    //    //ItemManagerに通知

    //    if (item.count <= 0)
    //    {
    //        RemoveItem(index);
    //        //ホットバーでも削除
    //    }
    //}


    ////========hotber===========

    //public int[] hotbares = new int[3];

    //private void Awake()
    //{
    //    for (int i = 0; i < hotbares.Length; i++)
    //    {
    //        hotbares[i] = -1;
    //    }
    //}

    ////ホットバーに追加（設定）
    //public void AddHotber(int hotberNumber, int index)
    //{
    //    hotbares[hotberNumber] = index;
    //}


    ////使用
    //public void Use(int hotberNumber)
    //{
    //    int index = hotbares[hotberNumber];
    //    if (index < 0)
    //    {
    //        return;
    //    }

    //    UseItem(index);
    //}


    ////インベントリ削除自クリア
    //public void HotberClear(int index)
    //{
    //    for (int i = 0; i < hotbares.Length; i++)
    //    {
    //        if (hotbares[i] == index)
    //        {
    //            hotbares[i] = -1;
    //        }
    //    }
    //}
}
