using UnityEditor.Rendering;
using UnityEngine;
using TMPro;

public class Test : MonoBehaviour
{
    [SerializeField] private InventorySystem inventorySystem;

    [SerializeField] private Data m_posion;
    [SerializeField] private Data m_dog;

    [SerializeField] private bool m_istrigger;

    [SerializeField] private GameObject m_trashButton;

    [SerializeField] private GameObject m_selectButton;

    [SerializeField] private GameObject m_hotbarActionPanel;

    [SerializeField] private TMP_Text m_nameText;

    [SerializeField] private TMP_Text m_infoText;


    private void Start()
    {
        m_trashButton.SetActive(false);
        m_selectButton.SetActive(false);
        m_hotbarActionPanel.SetActive(false);

        m_nameText.gameObject.SetActive(false);
        m_infoText.gameObject.SetActive(false);

       // m_infoPanel.SetActive(false);
    }

    public void ShowActiveButtons()
    {
        if (m_index == -1) return;

        m_trashButton.SetActive(true);
        m_selectButton.SetActive(true);
    }

    public void ShowPassiveButtons()
    {
        if (m_index == -1) return;

        m_trashButton.SetActive(true);
        m_selectButton.SetActive(false);
    }

    public void ShowHotbarActonPanel()
    {
        if (m_index == -1) return;

        m_hotbarActionPanel.SetActive(true);
    }
    public void ShowItemInfo(Data data)
    {
        Debug.Log(data);

        m_nameText.gameObject.SetActive (true);
        m_infoText.gameObject.SetActive (true);

        m_nameText.text = data.ItemName;

        m_infoText.text = data.info;
    }

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

    private bool m_isPassive;
    public void OnRemoveItem()
    {
        if (m_index == -1) return;

        Debug.Log(m_index);

        if (m_isPassive)
        {
            inventorySystem.RemovePassiveItem(m_index);
        }
        else
        {
            inventorySystem.RemoveActiveItem(m_index);
        }

            HideButtons();
    }

    public void OnUseItem()
    {
        if (m_index == -1) return;

        inventorySystem.UseItem(m_index);

        m_hotbarActionPanel.SetActive(false);
    }

    public void SetIndex(int index, bool isPassive)
    {
        m_index = index;
        m_isPassive = isPassive;
    }

    public void HideButtons()
    {
        m_trashButton.SetActive(false);
        m_selectButton.SetActive(false);

        m_index = -1;
    }

    //=========hotbar=====================

    public void OnMoveItemHotber1()
    {
        if (m_index == -1) return;

        inventorySystem.AddHotber(0, m_index);
    }

    public void OnMoveItemHotber2()
    {
        if (m_index == -1) return;

        inventorySystem.AddHotber(1, m_index);
    }

    public void OnMoveItemHotber3()
    {
        if (m_index == -1) return;

        inventorySystem.AddHotber(2, m_index);
    }

    public void OnUseHotbar1()
    {
        inventorySystem.Use(0);
    }

    public void OnUseHotbar2()
    {
        inventorySystem.Use(1);
    }

    public void OnUseHotbar3()
    {
        inventorySystem.Use(2);
    }

    //private void Update()
    //{
    //   if (Input.GetKeyDown(KeyCode.Space))
    //   {
    //        SetIndex(0);
    //    }
    //}
}