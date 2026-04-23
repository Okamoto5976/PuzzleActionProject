using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] GameObject pauseMenuUI;

    bool isPaused = false;

    private void OnEnable()
    {
        pauseAction.action.Enable();
    }

    private void OnDisable()
    {
        pauseAction.action.Disable();
    }


    void Update()
    {
        if (pauseAction.action.WasPressedThisFrame())
        { 
        if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
                
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenuUI.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenuUI.SetActive(false);
    }
}
