using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private BoolEventSO m_gameOverUIEvent;

    private void OnEnable()
    {
        m_gameOverUIEvent.Register(OnGameOver);
    }

    private void OnDisable()
    {
        m_gameOverUIEvent.Unregister(OnGameOver);
    }

    private void OnGameOver(bool isShow)
    {
        Debug.Log("UIŽó‚¯Žæ‚Á‚½");

        gameObject.SetActive(isShow);
    }
}