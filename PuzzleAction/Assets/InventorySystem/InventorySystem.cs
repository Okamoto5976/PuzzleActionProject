using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
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
    [SerializeField] private InventoryUI m_inventoryUI;
    //Inventory
    public ItemBox[] slots = new ItemBox[30];

    private void Awake()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = null;
        }

        for (int i = 0; i < hotbares.Length; i++)
        {
            hotbares[i] = -1;
        }
    }

    public bool AddItem(Data data, int count)
    {

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].data == data)
            {
                slots[i].count += count;
                Debug.Log("AddUI");
                m_inventoryUI.InstantiateObject(i, slots[i]);


                return true;
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            Debug.Log(slots[i] == null);

            if (slots[i] == null)
            {
                slots[i] = new ItemBox(data, count);
                Debug.Log("AddUI");

                m_inventoryUI.InstantiateObject(i, slots[i]);


                return true;
            }
        }

        //slots[0] = new ItemBox(data, count);
        //m_inventoryUI.InstantiateObject(0, slots[0]);
        return false;
    }

    //削除
    public void RemoveItem(int index)
    {
        slots[index] = null;
    }

    //使用
    public void UseItem(int index)
    {
        ItemBox item = slots[index];
        if (item == null) return;

        item.count--;

        if(item.count <= 0)
        {
            RemoveItem(index);
        }
    }

    //hotber

    public int[] hotbares = new int[3];


    //ホットバーに追加
    public void AddHotber(int hotberNumber, int index)
    {
        hotbares[hotberNumber] = index;
    }

    //使用
    public void Use(int hotberNumber)
    {
        int index = hotbares[hotberNumber];
        if(index < 0)
        {
            return;
        }

        UseItem(index);
    }


    //インベントリ削除時クリア
    public void hotberClear(int index)
    {
        for(int i = 0; i < hotbares.Length;i++)
        {
            if (hotbares[i] == index)
            {
                hotbares[i] = -1;
            }
        }
    }
}