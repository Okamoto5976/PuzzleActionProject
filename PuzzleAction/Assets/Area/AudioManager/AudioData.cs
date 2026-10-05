using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/Datas/AudioData")]
public class AudioData : ScriptableObject
{
    public AudioClip audioClip; //public float volume; public float pitch;

    [Range(0f, 1f)]
    public float volume;

    public bool isLoop;
}
