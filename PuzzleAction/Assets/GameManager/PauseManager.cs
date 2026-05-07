using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseAction;

    [Header("Event")]
    [SerializeField] private BoolEventSO m_gameOverUIEvent;
    [SerializeField] private BoolEventSO m_menuUIEvent;
    [SerializeField] private BoolEventSO m_optionUIEvent;
    [SerializeField] private BoolEventSO m_shopUIEvent;
    [SerializeField] private BoolEventSO m_inventoryUIEvent;

    bool isPaused = false;

    private void OnEnable()
    {
        pauseAction.action.Enable();
        pauseAction.action.performed += ToggleMenu;
    }

    private void OnDisable()
    {
        pauseAction.action.performed -= ToggleMenu;
        pauseAction.action.Disable();
    }

    public void ToggleMenu(InputAction.CallbackContext callback)
    {
        Debug.Log("Escape‰Ÿ‚³‚ê‚½");

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

    public void PauseGame()
    {
        Debug.Log("Raise True");
        Time.timeScale = 0f;
        m_gameOverUIEvent.Raise(true);
        m_menuUIEvent.Raise(true);
        m_optionUIEvent.Raise(true);
        m_shopUIEvent.Raise(true);
        m_inventoryUIEvent.Raise(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        m_gameOverUIEvent.Raise(false);
        m_menuUIEvent.Raise(false);
        m_optionUIEvent.Raise(false);
        m_shopUIEvent.Raise(false);
        m_inventoryUIEvent.Raise(false);
    }
}