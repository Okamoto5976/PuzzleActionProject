using UnityEngine;
using UnityEngine.UI;
<<<<<<< HEAD
using TMPro;
=======

>>>>>>> parent of 56b0578 (remove: Shop&Inventory)

public class SlotUI : MonoBehaviour
{
    [SerializeField] private Image m_icon;
    private int m_index;
    public GameObject m_InventoryPanel;

<<<<<<< HEAD
    [SerializeField] private Test m_testButton;

    [SerializeField] private bool isPassive;

    [SerializeField] private TMP_Text m_countText;

    private Data m_data;
=======

    [SerializeField] private Test m_testButton;

>>>>>>> parent of 56b0578 (remove: Shop&Inventory)

    public void SetItem(ItemBox item, int index)
    {
        m_icon.sprite = item.data.icon;
        m_index = index;
        m_icon.enabled = true;
<<<<<<< HEAD
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
=======
>>>>>>> parent of 56b0578 (remove: Shop&Inventory)
    }

    public void Clear()
    {
        m_icon.enabled = false;
<<<<<<< HEAD

        if (m_countText != null)
        {
            m_countText.text = "";
        }

        m_index = -1;

        m_data = null;
=======
>>>>>>> parent of 56b0578 (remove: Shop&Inventory)
    }

    public void OnInventoryPanel()
    {
<<<<<<< HEAD
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

=======
       m_testButton.SetIndex(m_index);
       m_testButton.ShowActionPanel();
>>>>>>> parent of 56b0578 (remove: Shop&Inventory)
    }

}