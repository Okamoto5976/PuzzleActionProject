using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/Datas/AudioData")]
public class AudioData : ScriptableObject
{
    public AudioClip audioClip;

    [Range(0f, 1f)]
    public float volume;

    [Range(-3f, 3f)]
    public float pitch = 1f;

    public bool isLoop;
}