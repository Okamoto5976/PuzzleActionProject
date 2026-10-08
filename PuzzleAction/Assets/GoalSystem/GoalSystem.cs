using System.Collections;
using UnityEngine;

public class GoalSystem : MonoBehaviour, IInteractable
{
    private MainGameManager m_mainGameManager;
    //[SerializeField] private Vector3Asset m_playerPos;
    [Header("State")]
    [SerializeField] private bool m_keyDoor;
    //[SerializeField] private bool m_hasKey;//å„ÅXRuntimeDatabool
    //[SerializeField] private float m_goalRadius;

    [SerializeField] private BoolEventSO m_canGoalEvent;

    private bool m_isClear = false;

    private Coroutine m_coroutine;

    private void Start()
    {
        if(GameManager.Instance.Level % 5 == 0)
        {
            m_keyDoor = true;
        }
    }

    public void Initialize(MainGameManager gameManager)
    {
        m_mainGameManager = gameManager;
    }

    public void OnInteract(Entity entity)
    {
        if (m_keyDoor)
        {
            if (!GameManager.Instance.HasKey)
            {
                Debug.Log("Can't goal");
                m_canGoalEvent.Raise(true);

                if(m_coroutine == null)
                {
                    m_coroutine = StartCoroutine(EventCoroutine());

                }

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

    private IEnumerator EventCoroutine()
    {
        yield return new WaitForSeconds(3f);

        m_canGoalEvent.Raise(false);

        m_coroutine = null;
    }
}
