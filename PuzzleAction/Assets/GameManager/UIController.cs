using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    private InputSystem_Actions m_action;

    [SerializeField] private GameObject m_gameOverUI;
    [SerializeField] private GameObject m_gameClearUI;
    [SerializeField] private MenuUI m_menuUI;

    [SerializeField] private InventoryUIController m_inventoryUIController;

    [SerializeField] private BoolEventSO m_inventoryEvent;

    [Header("Event")]
    [SerializeField] private BoolEventSO m_gameClearUIEvent;
    [SerializeField] private BoolEventSO m_gameOverUIEvent;

    [SerializeField] private TutorialManager m_tutorialManager;


    [Header("GameOverEvent")]
    [SerializeField] private TMP_Text m_levelText;
    [SerializeField] private string m_levelName;
    [SerializeField] private TMP_Text m_moneyText;
    [SerializeField] private string m_moneyName;

    public bool IsMenu => m_menuUI.gameObject.activeSelf;
    private bool m_isInventory = false;
    //private bool isInventoryOpen = false;

    //player input stop

    private void OnEnable()
    {
        m_gameClearUIEvent.Register(OnShowGameClearUI);
        m_gameOverUIEvent.Register(OnShowGameOverUI);

    }

    private void OnDisable()
    {
        m_gameClearUIEvent.Unregister(OnShowGameClearUI);
        m_gameOverUIEvent.Unregister(OnShowGameOverUI);

        m_action.Disable();

    }

    private void Start()
    {
        m_action = new InputSystem_Actions();

        m_action.Player.Menu.performed += ToggleMenu;
        m_action.Player.Inventory.performed += ToggleInventory;

        m_action.Enable();

        m_gameOverUI.SetActive(false);

        m_gameClearUI.SetActive(false);
    }

    private void Update()
    {
       
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

    }


    public void OnShowGameOverUI(bool isbool)
    {
        if (m_gameOverUI == null) return;

        m_gameOverUI.SetActive(isbool);

        m_levelText.text = m_levelName + GameManager.Instance.Level.ToString() + "ŠK‘w";
        m_moneyText.text = m_moneyName + GameManager.Instance.Money.ToString();

    }

    public void OnShowGameClearUI(bool isbool)
    {
        if (m_gameClearUI == null) return;

        m_gameClearUI.SetActive(isbool);
    }

}