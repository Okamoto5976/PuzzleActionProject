using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioEventSO audioEvent;
    [SerializeField] private AudioSource BGMSource;
    [SerializeField] private AudioSource SESource;
    [SerializeField] private AudioMixer m_audioMix;
    //[SerializeField] private FloatRunTime m_bgmVolume;
    //[SerializeField] private FloatRunTime m_seVolume;

    [SerializeField] private AudioFader audioFader;

    //--option save set--------------------
    private OptionSaveManager m_optionSaveManager = new();
    private float m_masterVolume;
    private float m_bgmVolume;
    private float m_seVolume;


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
        var data = m_optionSaveManager.OnAudioLoad();

        if (data != null)
        {
            SetMaster(data.m_masterVolume);
            SetBGM(data.m_bgmVolume);
            SetSE(data.m_seVolume);
        }
        else
        {
            SetMaster(0.8f);
            SetBGM(0.8f);
            SetSE(0.8f);
        }
    }

    public void SetMaster(float volume)
    {
        float db = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;

        m_audioMix.SetFloat("Master", db);
        m_masterVolume = volume;
        //m_bgmVolume.SetValue(volume);

        OnSaveAudio();
    }


    public void SetBGM(float volume)
    {
        float db = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;

        m_audioMix.SetFloat("BGM", db);
        m_bgmVolume = volume;
        //m_bgmVolume.SetValue(volume);

        OnSaveAudio();

    }

    public void SetSE(float volume)
    {
        float db = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;

        m_audioMix.SetFloat("SE", db);
        m_seVolume = volume;
        //m_seVolume.SetValue(volume);

        OnSaveAudio();

    }

    public void OnSaveAudio()
    {
        AudioSaveData data = new()
        {
            m_masterVolume = m_masterVolume,
            m_bgmVolume = m_bgmVolume,
            m_seVolume = m_seVolume
        };
        
        m_optionSaveManager.OnAudioSave(data);
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
