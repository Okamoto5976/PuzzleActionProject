using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    [HideInInspector] public SaveData data;
    string filepath;
    string fileName = "Data.json";

    void Awake()
    {
        filepath = Application.persistentDataPath + "/" + fileName;   

        if (!File.Exists(filepath))
        {
            data = new SaveData();
            Save(data);
        }
        data = Load(filepath);
        Debug.Log(JsonUtility.ToJson(data,true));
        Debug.Log(filepath);
    }
    void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        StreamWriter wr = new StreamWriter(filepath, false);
        wr.WriteLine(json);
        wr.Close();
    }
    SaveData Load(string path)
    {
        StreamReader rd = new StreamReader(path);
        string json = rd.ReadToEnd();
        rd.Close();
        return JsonUtility.FromJson<SaveData>(json);
    }

    void OnDestroy()
    {
        Save(data);   
    }
}
