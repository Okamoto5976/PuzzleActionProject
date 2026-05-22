using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotUI : MonoBehaviour
{
    [SerializeField] private Image m_icon;
    private int m_index;
    public GameObject m_InventoryPanel;

    [SerializeField] private Test m_testButton;

    [SerializeField] private bool isPassive;

    [SerializeField] private TMP_Text m_countText;

    private Data m_data;

    public void SetItem(ItemBox item, int index)
    {
        m_icon.sprite = item.data.icon;
        m_index = index;
        m_icon.enabled = true;
        m_data = item.data;

        if (m_countText != null)
        {
            if (item.count > 1)
            {
                m_countText.text = item.count.ToString();
            }
            else
            {
                m_countText.text = "";
            }
        }
    }

    public void Clear()
    {
        m_icon.enabled = false;

        if (m_countText != null)
        {
            m_countText.text = "";
        }

        m_index = -1;

        m_data = null;
    }

    public void OnInventoryPanel()
    {
        if (m_index == -1 || m_data == null)
        {
            return;
        }

       m_testButton.SetIndex(m_index, isPassive);

       m_testButton.ShowItemInfo(m_data);

        if(isPassive)
        {
            m_testButton.ShowPassiveButtons();
        }
        else
        {
            m_testButton.ShowActiveButtons();
        }
    }

}