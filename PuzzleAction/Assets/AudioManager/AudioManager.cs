using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField] private AudioEventSO audioEvent;
    [SerializeField] private AudioSource BGMSource;
    [SerializeField] private AudioSource SESource;

    [SerializeField] private AudioFader audioFader;

   // private Coroutine bgmFadeCoroutine;
    private void Awake()
    {
        //Singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        audioFader.audioSource = BGMSource;
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
        if (data.isLoop)
        {
            PlayBGM(data);

        }
        else
        {
            PlaySE(data);
        }
    }
    private void PlayBGM(AudioData data)
    {
       audioFader.FadeOutAndPlay(data.audioClip, data.clipVolume);
    }
    private void PlaySE(AudioData data)
    {
        SESource.PlayOneShot(data.audioClip, data.clipVolume);
    }
}
