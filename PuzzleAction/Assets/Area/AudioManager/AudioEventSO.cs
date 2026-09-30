using UnityEngine;
using System;


[CreateAssetMenu(fileName = "AudioEventSO", menuName = "Scriptable Objects/AudioEventSO")]
public class AudioEventSO : ScriptableObject
{
    public event Action<AudioData> OnEvent;

    //AudioClip‚ğó‚¯æ‚èA“o˜^‚³‚ê‚Ä‚¢‚éˆ—‚ğÀs‚·‚é
    public void Raise(AudioData data)
    {
        OnEvent?.Invoke(data);
    }
    //AudioÄ¶ˆ—‚ğ“o˜^‚·‚é
    public void Register(Action<AudioData> action)
    {
        OnEvent += action;
    }
    //AudioÄ¶ˆ—‚ğ‰ğœ‚·‚é
    public void Unregister(Action<AudioData> action)
    {
        OnEvent -= action;
    }
}
