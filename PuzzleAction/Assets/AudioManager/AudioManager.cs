using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioEventSO audioEvent;
    [SerializeField] private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        audioEvent.Register(PlayAudio);
    }
    private void OnDisable()
    {
        audioEvent.Unregister(PlayAudio);
    }
    //EventSOÇ©ÇÁìnÇ≥ÇÍÇΩAudioClipÇçƒê∂Ç∑ÇÈ
    private void PlayAudio(AudioData data)
    {

        SetTransform(data);
        SetVolume(data);
        PlayClip(data);
        //HandleLifeTime(data);
    }

    private void SetTransform(AudioData data)
    {
        audioSource.transform.position = data.position;
        audioSource.transform.rotation = data.rotation;
    }
    private void SetVolume(AudioData data)
    {
        audioSource.volume = data.clipVolume;
    }
    private void PlayClip(AudioData data)
    {
        audioSource.PlayOneShot(data.audioClip, data.clipVolume);
    }
   // private void HandleLifeTime(AudioData data)
   // {
   //     // if (data.lifeTime > 0f)
   //     //éwíËïbêîçƒê∂Ç≥ÇπÇÈ  
   // }
}
