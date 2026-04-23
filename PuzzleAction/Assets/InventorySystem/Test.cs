using UnityEditor.Rendering;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private InventorySystem inventorySystem;

    [SerializeField] private Data m_posion;
    [SerializeField] private Data m_dog;

    [SerializeField] private bool m_istrigger;

    [SerializeField] private int m_index;
    //[SerializeField] private Sprite m_potion;

    //public void testButton()
    //{
    //    Item _item = new Item("Potion", "HP Heal 50", m_potion);
    //    //inventorySystem.AddItem(_item);
    //}

    public void OnAddItem()
    {

        Debug.Log("AddItem");
        if (m_istrigger)
        {
            inventorySystem.AddItem(m_posion, 1);

        }
        else
        {
            inventorySystem.AddItem(m_dog, 1);
        }
    }

    public void OnRemoveItem()
    {
        inventorySystem.RemoveItem(m_index);
    }

    [SerializeField] private int m_hotberNumber;
    //public void OnHotberItem()
    //{
    //    inventorySystem.AddHotber(m_hotberNumber, m_index);
    //}
}