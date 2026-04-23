using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] private AudioEventSO audioEvent;
    [SerializeField] private AudioData audioData;

    public void Play()
    {
        audioEvent.Raise(audioData);
    }
}