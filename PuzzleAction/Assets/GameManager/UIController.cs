using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject m_gameOverUI;
    [SerializeField] private GameObject m_menuUI;
    [SerializeField] private GameObject m_optionUI;
    [SerializeField] private GameObject m_shopUI;
    [SerializeField] private GameObject m_inventoryUI;


    [Header("Event")]
    [SerializeField] private BoolEventSO m_gameOverUIEvent;
    [SerializeField] private BoolEventSO m_menuUIEvent;
    [SerializeField] private BoolEventSO m_optionUIEvent;
    [SerializeField] private BoolEventSO m_shopUIEvent;
    [SerializeField] private BoolEventSO m_inventoryUIEvent;


    private bool m_isshopUI;
    private bool m_isinventoryUI;

    private void OnEnable()
    {
        m_menuUIEvent.Register(MenuUI);
        m_gameOverUIEvent.Register(GameOverUI);
        m_optionUIEvent.Register(OptionUI);
        m_shopUIEvent.Register(ShopUI);
        m_inventoryUIEvent.Register(InventoryUI);   
    }

    private void OnDisable()
    {
        m_gameOverUIEvent.Unregister(GameOverUI);
        m_menuUIEvent.Unregister(MenuUI);
        m_optionUIEvent.Unregister(OptionUI);
        m_shopUIEvent.Unregister(ShopUI);
        m_inventoryUIEvent.Unregister(InventoryUI);
    }
    private void Start()
    {
        m_gameOverUI.SetActive(false);
        m_menuUI.SetActive(false);
        m_optionUI.SetActive(false);
        m_shopUI.SetActive(false);
        m_inventoryUI.SetActive(false);
    }

    private void GameOverUI(bool isbool)
    {
        m_gameOverUI.SetActive(isbool);
    }

    private void MenuUI(bool isbool)
    {
        m_menuUI.SetActive(isbool);
    }

    private void OptionUI(bool isbool)
    {
        m_optionUI.SetActive(isbool);
    }
    //ショップとインベントリ
    private void ShopUI(bool isbool)
    {
        m_shopUI.SetActive(isbool);
    }
    public void OnShopUI()
    {
        m_shopUIEvent.Raise(true);
        m_isshopUI = true;
    }

    private void InventoryUI(bool isbool)
    {
        m_inventoryUI.SetActive(isbool);
    }
    public void OnInventoryUI()
    {
        m_inventoryUIEvent.Raise(true);
        m_isinventoryUI = true;
    }

}
