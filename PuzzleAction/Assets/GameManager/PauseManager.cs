using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] GameObject pauseMenuUI;

    private UIController m_controller;

    [Header("Event")]
    [SerializeField] private BoolEventSO m_menuUIEvent;

    bool isPaused = false;

    private void OnEnable()
    {
        pauseAction.action.performed += ToggleMenu;
    }

    private void OnDisable()
    {
        pauseAction.action.performed -= ToggleMenu;
    }

    public void ToggleMenu(InputAction.CallbackContext callback)
    {
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
        Time.timeScale = 0f;
        isPaused = true;
        m_menuUIEvent.Raise(true);
        pauseMenuUI.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        m_menuUIEvent.Raise(false);
        pauseMenuUI.SetActive(false);
    }
}
