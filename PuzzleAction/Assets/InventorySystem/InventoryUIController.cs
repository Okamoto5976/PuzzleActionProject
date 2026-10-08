using UnityEngine;
using TMPro;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private InventorySystem inventorySystem;

    [SerializeField] private GameObject m_trashButton;

    [SerializeField] private GameObject m_selectButton;

    [SerializeField] private GameObject m_hotbarActionPanel;

    [SerializeField] private GameObject m_statusView;

    [SerializeField] private TMP_Text m_nameText;

    [SerializeField] private TMP_Text m_infoText;

    [SerializeField] private TMP_Text m_useTypeText;

    [SerializeField] private GameObject m_activePanel;

    [SerializeField] private GameObject m_passivepanel;

    [SerializeField] private GameObject m_hotbarPanel;

    [SerializeField] private GameObject m_Panel;

    [SerializeField] private TutorialManager m_tutorialManager;


    //なんでもいいからfalse,trueになるオブジェクトを見て、開かれてるか見る
    public bool IsOpen => m_activePanel.gameObject.activeSelf;

    private void Start()
    {
        m_trashButton.SetActive(false);
        m_selectButton.SetActive(false);
        m_hotbarActionPanel.SetActive(false);
        m_statusView.SetActive(false);

        m_nameText.gameObject.SetActive(false);
        m_infoText.gameObject.SetActive(false);
        m_useTypeText.gameObject.SetActive(false);

        m_activePanel.SetActive(false);
        m_passivepanel.SetActive(false);
        m_hotbarPanel.SetActive(false);

        m_Panel.SetActive(false);

       // m_infoPanel.SetActive(false);
    }

    public void SetInventoryVisibility(bool state)
    {
        if (GameManager.Instance.IsTutorial && state == false)
        {
            if (m_tutorialManager.m_isInventoryEvent) return;

        }

        m_activePanel.SetActive(state);
        m_passivepanel.SetActive(state);
        m_hotbarPanel.SetActive(state);
        m_statusView.SetActive(state);

        m_Panel.SetActive(state);

        if (!state)
        {
            HideButtons();
        }
    }
    public void ShowActiveButtons()
    {
        if (m_index == -1) return;

        if (GameManager.Instance.IsTutorial)
        {
            m_tutorialManager.InventoryTutorialNextButton();

        }


        m_trashButton.SetActive(true);
        m_selectButton.SetActive(true);
    }

    public void ShowPassiveButtons()
    {
        if (m_index == -1) return;

        if (GameManager.Instance.IsTutorial)
        {
            if (!m_tutorialManager.m_canClickItem) return;

        }

        m_trashButton.SetActive(true);
        m_selectButton.SetActive(false);
    }

    public void ShowHotbarActonPanel()
    {
        if (m_index == -1)
        {
            m_hotbarActionPanel.SetActive(false);

            return;

        }
        m_hotbarActionPanel.SetActive(true);
    }
    public void ShowItemInfo(Item data)
    {
        //Debug.Log("=== ShowItemInfo ===");
        //Debug.Log($"data = {data}");

        if (data == null)
        {
            Debug.LogError("ShowItemInfoに渡されたdataがNULL！");
            return;
        }

        //Debug.Log($"名前 = {data.ItemName}");
        //Debug.Log($"説明 = {data.info}");

        m_nameText.gameObject.SetActive (true);
        m_infoText.gameObject.SetActive (true);
        m_useTypeText.gameObject.SetActive (true);

        m_nameText.text = data.ItemName;
        m_infoText.text = data.info;

        string description = null;

        switch (data.ItemUseType)
        {       
            case ItemUseType.Instant:
                description = $"Spaceを押すと向いている方向に発動";
                break;
            case ItemUseType.Arrow:
                description = $"Space長押しで距離を伸ばし、離すと発動";
                break;
            case ItemUseType.Set:
                description = $"Space長押しで設置場所を指定、離すと発動";
                break;
            case ItemUseType.Attack:
                description = $"Space長押しで構え、離すと発動";
                break;
        }


        m_useTypeText.text = $"{data.ItemUseType}\n" + description;
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

        //inventorySystem.UseItem(m_index);

        m_hotbarActionPanel.SetActive(false);
    }

    public void SetIndex(int index, bool isPassive)
    {
        //Debug.Log($"SetIndex : {index}");

        m_index = index;
        m_isPassive = isPassive;
    }

    public void HideButtons()
    {
        //Debug.Log("Hide!");

        m_trashButton.SetActive(false);
        m_selectButton.SetActive(false);

        m_hotbarActionPanel.SetActive(false);

        m_nameText.gameObject.SetActive(false);
        m_infoText.gameObject.SetActive(false);
        m_useTypeText.gameObject.SetActive(false);

        m_index = -1;
    }

    //=========hotbar=====================

    [SerializeField] private AudioData m_SE;

    public void OnMoveItemHotber1()
    {
        //Debug.Log($"Hotbar1 index = {m_index}");
        if (m_index == -1) return;

        inventorySystem.AddHotber(0, m_index);

        AudioManager.Instance.PlayAudio(m_SE);
    }

    public void OnMoveItemHotber2()
    {
        //Debug.Log($"Hotbar2 index = {m_index}");

        if (m_index == -1) return;

        inventorySystem.AddHotber(1, m_index);

        AudioManager.Instance.PlayAudio(m_SE);

    }

    public void OnMoveItemHotber3()
    {
        //Debug.Log($"Hotbar3 index = {m_index}");

        if (m_index == -1) return;

        inventorySystem.AddHotber(2, m_index);

        AudioManager.Instance.PlayAudio(m_SE);

    }

    public void OnUseHotbar1()
    {
        //inventorySystem.Use(0);
    }

    public void OnUseHotbar2()
    {
        //inventorySystem.Use(1);
    }

    public void OnUseHotbar3()
    {
        //inventorySystem.Use(2);
    }
}