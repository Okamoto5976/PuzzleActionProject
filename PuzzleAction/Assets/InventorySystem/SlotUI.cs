using UnityEngine;
using UnityEngine.UI;


public class SlotUI : MonoBehaviour
{
    [SerializeField] private Image m_icon;
    private int m_index;
    public GameObject m_InventoryPanel;


    [SerializeField] private Test m_testButton;

    public void SetItem(ItemBox item, int index)
    {
        m_icon.sprite = item.data.icon;
        m_index = index;
        m_icon.enabled = true;
    }

    public void Clear()
    {
        m_icon.enabled = false;
    }

    public void OnInventoryPanel()
    {
       m_testButton.SetIndex(m_index);
    }

}