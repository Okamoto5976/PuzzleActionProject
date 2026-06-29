using UnityEditor.Build.Content;
using UnityEngine;

public class GoalSystem : MonoBehaviour
{
    //[SerializeField] private GameManager gameManager;
    [SerializeField] private Transform player;
    [Header("State")]
    [SerializeField] private bool keyDoor;
    [SerializeField] private bool hasKey;//å„ÅXRuntimeDatabool
    [SerializeField] private float goalRadius;

    //Player position runtime
    private void Update()
    {
        if(HitGoal(transform.position, player.position))
        {
            OnGoal();
        }
    }

    public void OnGoal()
    {
        if (keyDoor)
        {
            if (!hasKey)
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
        this.enabled = false;
        //gameManager.Clear();
    }

    public void SetValue(bool value)
    {
        keyDoor = value;
    }

    private bool HitGoal(Vector2 goalPos, Vector2 playerPos)
    {
        float distance = Vector2.Distance(goalPos, playerPos);
        if (distance < goalRadius)
        {
            return true;
        }
        return false;
    }
}
