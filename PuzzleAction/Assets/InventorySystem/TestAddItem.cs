using UnityEngine;

public class TestAddItem : MonoBehaviour
{
    [SerializeField] private ItemManager m_itemManager;
    [SerializeField] private InventorySystem m_inventorySystem;

    [SerializeField] private int m_index;
    [SerializeField] private int m_count;

    [ContextMenu("AddItem")]
    public void AddItemToInventory()
    {
        var item = m_itemManager.GetItem(m_index);

        m_inventorySystem.AddItem(item, m_count);
    }
}
