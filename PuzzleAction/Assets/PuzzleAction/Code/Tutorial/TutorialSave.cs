using UnityEngine;
using System.IO;


public class TutorialData
{
    public bool m_tutorialCompleted;
    public bool m_GoalTutorialCompleted;
}

public class TutorialSave
{
    public void SaveTutorialData(TutorialData data)
    {
        string json = JsonUtility.ToJson(data, true);

        string path = Application.persistentDataPath + "/tutorialData.json";

        File.WriteAllText(path, json);
    }

    public TutorialData LoadTutorialData()
    {
        string path = Application.persistentDataPath + "/tutorialData.json";

        if (!File.Exists(path))
        {
            return null;
        }

        string json = File.ReadAllText(path);

        TutorialData data = JsonUtility.FromJson<TutorialData>(json);

        return data;
    }

    public void DeleteTutorialData()
    {
        string path = Application.persistentDataPath + "/tutorialData.json";

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
