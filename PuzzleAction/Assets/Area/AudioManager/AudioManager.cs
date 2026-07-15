using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField] private AudioEventSO audioEvent;
    [SerializeField] private AudioSource BGMSource;
    [SerializeField] private AudioSource SESource;
    [SerializeField] private AudioMixer m_audioMix;
    [SerializeField] private Slider m_BGMSlider;
    [SerializeField] private Slider m_SESlider;

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
    private void Start()
    {
        m_audioMix.GetFloat("BGM", out float bgmVolume);
        m_BGMSlider.value = bgmVolume;

        m_audioMix.GetFloat("SE", out float seVolume);
        m_SESlider.value = seVolume;
    }

    public void SetBGM(float volume)
    {
        m_audioMix.SetFloat("BGM", volume);
    }

    public void SetSE(float volume)
    {
        m_audioMix.SetFloat("SE", volume);
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
