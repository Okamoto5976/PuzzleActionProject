using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject m_gameOverUI;
    [SerializeField] private GameObject m_menuUI;
    [SerializeField] private GameObject m_optionUI;
    [SerializeField] private GameObject m_shopUI;
    [SerializeField] private GameObject m_inventoryUI;
    [SerializeField] private GameObject m_gameClearUI;

    [Header("Event")]
    [SerializeField] private BoolEventSO m_gameOverUIEvent;
    [SerializeField] private BoolEventSO m_menuUIEvent;
    [SerializeField] private BoolEventSO m_optionUIEvent;
    [SerializeField] private BoolEventSO m_shopUIEvent;
    [SerializeField] private BoolEventSO m_inventoryUIEvent;
    [SerializeField] private BoolEventSO m_gameClearUIEvent;

    private bool isMenuOpen = false;
    private bool isInventoryOpen = false;

    private void OnEnable()
    {
        m_menuUIEvent.Register(MenuUI);
        m_gameOverUIEvent.Register(GameOverUI);
        m_optionUIEvent.Register(OptionUI);
        m_shopUIEvent.Register(ShopUI);
        m_inventoryUIEvent.Register(InventoryUI);
        m_gameClearUIEvent.Register(ShowGameClearUI);
    }

    private void OnDisable()
    {
        m_gameOverUIEvent.Unregister(GameOverUI);
        m_menuUIEvent.Unregister(MenuUI);
        m_optionUIEvent.Unregister(OptionUI);
        m_shopUIEvent.Unregister(ShopUI);
        m_inventoryUIEvent.Unregister(InventoryUI);
        m_gameClearUIEvent.Unregister(ShowGameClearUI);
    }

    private void Start()
    {
        m_gameOverUI.SetActive(false);
        m_menuUI.SetActive(false);
        m_optionUI.SetActive(false);
        m_shopUI.SetActive(false);
        m_inventoryUI.SetActive(false);
        m_gameClearUI.SetActive(false);
    }

    private void Update()
    {
        // ESCでメニュー
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuOpen = !isMenuOpen;

            m_menuUIEvent.Raise(isMenuOpen);

            // メニュー閉じたらオプションも閉じる
            if (!isMenuOpen)
            {
                m_optionUIEvent.Raise(false);
            }
        }

        // TABでインベントリ
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isInventoryOpen = !isInventoryOpen;

            m_inventoryUIEvent.Raise(isInventoryOpen);
        }
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

    private void ShopUI(bool isbool)
    {
        //m_shopUI.SetActive(isbool);
    }

    private void InventoryUI(bool isbool)
    {
        m_inventoryUI.SetActive(isbool);
    }

    private void ShowGameClearUI(bool isShow)
    {
        m_gameClearUI.SetActive(isShow);
    }

    // ショップ開く
    public void OnShopUI()
    {
        m_shopUIEvent.Raise(true);
    }

    // インベントリ開く
    public void OnInventoryUI()
    {
        m_inventoryUIEvent.Raise(true);
    }

    // オプション開く
    public void OpenOption()
    {
        m_menuUI.SetActive(false);
        m_optionUI.SetActive(true);
    }

    // メニューに戻る
    public void BackMenu()
    {
        m_optionUI.SetActive(false);
        m_menuUI.SetActive(true);
    }
}