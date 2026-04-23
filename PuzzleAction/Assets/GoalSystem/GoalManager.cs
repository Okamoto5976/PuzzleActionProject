using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public int currentFloor;
}

public class GoalManager : MonoBehaviour
{
    [SerializeField] private bool keyDoor;
    [SerializeField] private bool hasKey;//å„ÅXRuntimeDatabool

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
                Debug.Log("âè˘");
            }
        }

        Debug.Log("Goal");
    }

    public void SetValue(bool value)
    {
        keyDoor = value;
    }
}
