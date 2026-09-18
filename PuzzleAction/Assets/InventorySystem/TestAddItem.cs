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

    [SerializeField] private DropMoneyEventSO m_dropMoneyEventSO;
    [SerializeField] private Vector3 m_pos;
    [SerializeField] private int m_money;

    [ContextMenu("DropMoney")]
    public void DropMoney()
    {
        m_dropMoneyEventSO.Raise(m_pos, m_money);
    }
}
