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
    [SerializeField] private bool hasKey;

    // ▼仮------------------
    [SerializeField] private int currentFloor;
    // ▲--------------------

    public void OnGoal()
    {
        // セーブ
        if (currentFloor % 5 == 0)
        {
            if (!hasKey)
            {
                Debug.Log("Can't goal");
                return;
            }
        }
        Debug.Log("Goal");

        // ロードシーン
        //SceneManager.LoadScene("");
    }

    public void SaveToJson()
    {
        Debug.Log("Save");
        SaveData data = new SaveData()
        {
            currentFloor = 0
        };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadFromJson()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log($"floor: {data.currentFloor}");
        }
    }
}
