using UnityEngine;

public class GoalSystem : MonoBehaviour
{
    private MainGameManager m_mainGameManager;
    //[SerializeField] private Vector3Asset m_playerPos;
    [Header("State")]
    [SerializeField] private bool m_keyDoor;
    [SerializeField] private bool m_hasKey;//å„ÅXRuntimeDatabool
    //[SerializeField] private float m_goalRadius;

    private bool m_isClear = false;

    public void Initialize(MainGameManager gameManager)
    {
        m_mainGameManager = gameManager;
    }

    public void OnGoal()
    {
        if (m_keyDoor)
        {
            if (!m_hasKey)
            {
                Debug.Log("Can't goal");
                return;
            }
            else
            {
                Debug.Log("Unlock");
            }
        }

        //this.enabled = false;
        if (m_isClear) return;
        Debug.Log("Goal");

        m_isClear = true;
        m_mainGameManager.GameClear();
    }

    public void SetValue(bool value)
    {
        m_keyDoor = value;
    }
}
