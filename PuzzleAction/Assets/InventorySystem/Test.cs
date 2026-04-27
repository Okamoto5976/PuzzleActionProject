using UnityEditor.Rendering;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private InventorySystem inventorySystem;

    [SerializeField] private Data m_posion;
    [SerializeField] private Data m_dog;

    [SerializeField] private bool m_istrigger;

    //[SerializeField] private Sprite m_potion;

    //public void testButton()
    //{
    //    Item _item = new Item("Potion", "HP Heal 50", m_potion);
    //    //inventorySystem.AddItem(_item);
    //}

    public void OnItem()
    {

        Debug.Log("AddItem");
        if (m_istrigger)
        {
            inventorySystem.OnItem(m_posion, 1);

        }
        else
        {
            inventorySystem.OnItem(m_dog, 1);
        }
    }

    //=========remove button=============


    private int m_index = -1;

    public void OnRemoveItem()
    {
        if (m_index == -1) return;

        Debug.Log(m_index);
        inventorySystem.RemoveItem(m_index);
        inventorySystem.UpdateUI();
    }

    public void SetIndex(int index)
    {
        m_index = index;
    }

    //=========hotber=====================

    [SerializeField] private int[] m_hotberNumber;

    public void OnMoveItemHotber1()
    {
        if (m_index == -1) return;

        inventorySystem.AddHotber(m_hotberNumber[0], m_index);
    }

    public void OnMoveItemHotber2()
    {
        if (m_index == -1) return;

        inventorySystem.AddHotber(m_hotberNumber[1], m_index);
    }

    public void OnMoveItemHotber3()
    {
        if (m_index == -1) return;

        inventorySystem.AddHotber(m_hotberNumber[2], m_index);
    }
}