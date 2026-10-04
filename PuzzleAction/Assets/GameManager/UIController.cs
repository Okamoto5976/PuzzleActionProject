using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    private InputSystem_Actions m_action;

    [SerializeField] private GameObject m_gameOverUI;
    [SerializeField] private GameObject m_gameClearUI;
    [SerializeField] private MenuUI m_menuUI;
    //[SerializeField] private MenuUI m_menuUIObj;

    //[SerializeField] private GameObject m_shopUI;
    [SerializeField] private InventoryUIController m_inventoryUIController;

    //[SerializeField] private GameObject m_inventoryUI;
    [SerializeField] private BoolEventSO m_inventoryEvent;

    [Header("Event")]
    [SerializeField] private BoolEventSO m_gameClearUIEvent;
    [SerializeField] private BoolEventSO m_gameOverUIEvent;
    //[SerializeField] private BoolEventSO m_menuUIEvent;
    //[SerializeField] private BoolEventSO m_optionUIEvent;
    //[SerializeField] private BoolEventSO m_shopUIEvent;
    //[SerializeField] private BoolEventSO m_inventoryUIEvent;

    [SerializeField] private TutorialManager m_tutorialManager;


    public bool IsMenu => m_menuUI.gameObject.activeSelf;
    private bool m_isInventory = false;
    //private bool isInventoryOpen = false;

    //player input stop

    private void OnEnable()
    {
        m_gameClearUIEvent.Register(OnShowGameClearUI);
        m_gameOverUIEvent.Register(OnShowGameOverUI);


        //m_menuUIEvent.Register(MenuUI);
        //m_optionUIEvent.Register(OptionUI);
        //m_shopUIEvent.Register(ShopUI);
        //m_inventoryUIEvent.Register(InventoryUI);
    }

    private void OnDisable()
    {
        m_gameClearUIEvent.Unregister(OnShowGameClearUI);
        m_gameOverUIEvent.Unregister(OnShowGameOverUI);

        //m_menuUIEvent.Unregister(MenuUI);
        //m_optionUIEvent.Unregister(OptionUI);
        //m_shopUIEvent.Unregister(ShopUI);
        //m_inventoryUIEvent.Unregister(InventoryUI);

        m_action.Disable();

    }

    private void Start()
    {
        m_action = new InputSystem_Actions();

        m_action.Player.Menu.performed += ToggleMenu;
        m_action.Player.Inventory.performed += ToggleInventory;

        m_action.Enable();


        m_gameOverUI.SetActive(false);
        //m_menuUI.SetActive(false);
        //m_optionUI.SetActive(false);
        //if (m_shopUI != null) m_shopUI.SetActive(false);
        //m_inventoryUI.SetActive(false);
        m_gameClearUI.SetActive(false);
    }

    private void Update()
    {
        // ESC�Ń��j���[
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    m_IsMenu = !m_IsMenu;

        //    //m_menuUIEvent.Raise(isMenuOpen);

        //    // ���j���[������I�v�V����������
        //    if (!m_IsMenu)
        //    {
        //        //m_optionUIEvent.Raise(false);
        //    }
        //}

        //// TAB�ŃC���x���g��
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    //isInventoryOpen = !isInventoryOpen;

        //    //m_inventoryUIEvent.Raise(isInventoryOpen);
        //}
    }

    //if get key "EscapeKey"
    private void ToggleMenu(InputAction.CallbackContext context)
    {
        if (MainGameManager.Instance.IsGameOver) return;



        if (IsMenu)
        {
            m_menuUI.Close();
            
        }
        else
        {
            m_menuUI.Open();
        }

    }

    private void ToggleInventory(InputAction.CallbackContext callback)
    {
        if (MainGameManager.Instance.IsGameOver) return;

        if (GameManager.Instance.IsStop) return;

        if (GameManager.Instance.IsTutorial)
        {
            if (!m_tutorialManager.m_canTab) return;

            m_tutorialManager.InventoryTutorial();

        }
        Debug.Log("Tab");



        m_inventoryEvent.RaiseEvent(!m_inventoryUIController.IsOpen);

        //if (!m_isInventory)
        //{
        //    //OnShowInventoryUI(true);
        //    m_isInventory = true;
        //}
        //else
        //{
        //    //OnShowInventoryUI(false);
        //    m_inventoryEvent.RaiseEvent(false);
        //    m_isInventory = false;


        //}
    }


    public void OnShowGameOverUI(bool isbool)
    {
        if (m_gameOverUI == null) return;

        m_gameOverUI.SetActive(isbool);
    }

    public void OnShowGameClearUI(bool isbool)
    {
        if (m_gameClearUI == null) return;

        m_gameClearUI.SetActive(isbool);
    }

    //public void OnShowMenuUI(bool isbool)
    //{
    //    if (m_menuUI == null) return;
    //    //menu Isoption = true , menu run method
    //    m_isMenu = isbool;
    //    m_menuUI.SetActive(isbool);
    //}


    public void OnShowShopUI(bool isbool)
    {
        //if (m_shopUI == null) return;

        //m_shopUI.SetActive(isbool);
    }

    //public void OnShowInventoryUI(bool isbool)
    //{
    //    if (m_inventoryUI == null) return;

    //    m_isInventory = isbool;

    //    m_inventoryUI.SetActive(isbool);
    //}
}