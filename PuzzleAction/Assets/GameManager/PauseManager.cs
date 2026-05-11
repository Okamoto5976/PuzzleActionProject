using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private InputActionReference inventoryAction;

    [Header("Event")]
    [SerializeField] private BoolEventSO m_menuUIEvent;
    [SerializeField] private BoolEventSO m_optionUIEvent;
    [SerializeField] private BoolEventSO m_inventoryUIEvent;

    private bool isPaused = false;
    private bool isInventoryOpen = false;
    private bool isOptionOpen = false;

    private void OnEnable()
    {
        pauseAction.action.Enable();
        inventoryAction.action.Enable();

        pauseAction.action.performed += ToggleMenu;
        inventoryAction.action.performed += ToggleInventory;
    }

    private void OnDisable()
    {
        pauseAction.action.performed -= ToggleMenu;
        inventoryAction.action.performed -= ToggleInventory;

        pauseAction.action.Disable();
        inventoryAction.action.Disable();
    }

    // ESC
    private void ToggleMenu(InputAction.CallbackContext callback)
    {
        // オプション開いてるなら戻る
        if (isOptionOpen)
        {
            CloseOption();
            return;
        }

        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    // TAB
    private void ToggleInventory(InputAction.CallbackContext callback)
    {
        isInventoryOpen = !isInventoryOpen;

        m_inventoryUIEvent.Raise(isInventoryOpen);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        m_menuUIEvent.Raise(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;

        m_menuUIEvent.Raise(false);
        m_optionUIEvent.Raise(false);
    }

    // ボタンから呼ぶ
    public void OpenOption()
    {
        isOptionOpen = true;

        m_menuUIEvent.Raise(false);
        m_optionUIEvent.Raise(true);
    }

    public void CloseOption()
    {
        isOptionOpen = false;

        m_optionUIEvent.Raise(false);
        m_menuUIEvent.Raise(true);
    }
}