using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioEventSO audioEvent;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioFader audioFader;
    [SerializeField] private AudioClip BGM;

   
   // private Coroutine bgmFadeCoroutine;
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
        audioSource.loop = false;
        audioSource.clip = null;
        audioSource.volume = data.clipVolume;
        //audioSource.loop = data.isLoop;
        if (data.isLoop)
        {
            audioSource.loop = true;
            audioSource.clip = data.audioClip;
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(data.audioClip, data.clipVolume);
        }
    }

    public void FeadOut()
    {
        audioFader.FadeOutAndPlay(BGM, 0.8f);
    }
       // audioSource.PlayOneShot(data.audioClip, data.clipVolume);
       // if (data.isLoop)
      //  audioSource.loop = true;
        //else
        //    audioSource.loop = false;
   // private void HandleLifeTime(AudioData data)
   // {
   //     // if (data.lifeTime > 0f)
   //     //éwíËïbêîçƒê∂Ç≥ÇπÇÈ  
   // }
}
