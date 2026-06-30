using UnityEditor.Build.Content;
using UnityEngine;

public class GoalSystem : MonoBehaviour
{
    //[SerializeField] private GameManager gameManager;
    [SerializeField] private Transform m_player;
    [Header("State")]
    [SerializeField] private bool m_keyDoor;
    [SerializeField] private bool m_hasKey;//å„ÅXRuntimeDatabool
    [SerializeField] private float m_goalRadius;

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

        Debug.Log("Goal");
        //this.enabled = false;
        //gameManager.Clear();
    }

    public void SetValue(bool value)
    {
        m_keyDoor = value;
    }
}
