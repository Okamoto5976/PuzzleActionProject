using UnityEngine;
using System.IO;

public class PlayerData
{
    public int m_hp;
}

public class PlayerSave
{
    public void SavePlayerData(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);

        string path = Application.persistentDataPath + "/playerSave.json";

        File.WriteAllText(path, json);
    }

    public PlayerData LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/playerSave.json";

        if (!File.Exists(path))
        {
            return null;
        }

        string json = File.ReadAllText(path);

        PlayerData data = JsonUtility.FromJson<PlayerData>(json);

        return data;
    }

    public void DeletePlayerData()
    {
        string path = Application.persistentDataPath + "/playerSave.json";

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
