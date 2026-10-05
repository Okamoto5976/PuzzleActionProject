using UnityEngine;
using UnityEditor;
using System.IO;


public static class AudioDataCreate
{
    [MenuItem("Assets/Create AudioDataSO", true)]
    private static bool ValidateCreateAudioData()
    {
        return Selection.activeObject is AudioClip;
    }

    [MenuItem("Assets/Create AudioDataSO")]
    private static void CreateAudioData()
    {
        AudioClip clip = Selection.activeObject as AudioClip;

        if(clip == null)
        {
            Debug.LogWarning("AudioClipÇëIëÇµÇƒÇ≠ÇæÇ≥Ç¢ÅB");

            return;
        }

        string clipPath = AssetDatabase.GetAssetPath(clip);

        string directory = Path.GetDirectoryName(clipPath);
        string fileName = Path.GetFileNameWithoutExtension(clipPath);

        string soPath = Path.Combine(
            directory,
            fileName + ".asset"
            );

        if(File.Exists(soPath))
        {
            Debug.LogWarning(
                $"AudioDataSOÇÕä˘ë∂Ç…ë∂ç›ÇµÇ‹Ç∑: {soPath}"
                );

            return;
        }

        AudioData audioData = ScriptableObject.CreateInstance<AudioData>();
        
        audioData.audioClip = clip;

        AssetDatabase.CreateAsset(
            audioData,
            soPath
            );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Selection.activeObject = audioData;

        Debug.Log($"AudioDataSO ÇÅ@çÏê¨ÇµÇ‹ÇµÇΩ: {soPath}");
    }
}
