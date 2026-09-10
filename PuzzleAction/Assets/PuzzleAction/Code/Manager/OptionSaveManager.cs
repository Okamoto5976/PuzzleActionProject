using System.IO;
using UnityEngine;

public class AudioSaveData
{
    public float m_masterVolume;
    public float m_bgmVolume;
    public float m_seVolume;
}

public class OptionSaveManager
{
    private bool m_isTutorial;

    public void OnAudioSave(AudioSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);

        string path = Application.persistentDataPath + "/audioSave.json";

        File.WriteAllText(path, json);
    }

    public AudioSaveData OnAudioLoad()
    {
        string path = Application.persistentDataPath + "/audioSave.json";
        
        if(!File.Exists(path))
        {
            return null;
        }

        string json = File.ReadAllText(path);

        AudioSaveData data = JsonUtility.FromJson<AudioSaveData>(json);

        return data;
    }

    public void OnAudioDelete()
    {
        string path = Application.persistentDataPath + "/audioSave.json";
        
        if(File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
