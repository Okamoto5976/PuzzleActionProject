using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource BGMSource;
    [SerializeField] private AudioSource SESource;
    [SerializeField] private AudioMixer m_audioMix;
    [SerializeField] private AudioFader audioFader;

    private AudioData m_nowBGMData;

    private OptionSaveManager m_optionSaveManager = new();

    private float m_masterVolume;
    private float m_bgmVolume;
    private float m_seVolume;

    private float m_bgmPitch = 1f;
    private float m_sePitch = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioFader.audioSource = BGMSource;
    }

    private void Start()
    {
        var data = m_optionSaveManager.OnAudioLoad();

        if (data != null)
        {
            SetMaster(data.masterVolume);
            SetBGM(data.bgmVolume);
            SetSE(data.seVolume);
        }
        else
        {
            SetMaster(0.8f);
            SetBGM(0.8f);
            SetSE(0.8f);
        }

        SetBGMPitch(1f);
        SetSEPitch(1f);
    }

    public void SetMaster(float volume)
    {
        float db = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;

        m_audioMix.SetFloat("Master", db);
        m_masterVolume = volume;

        Save();

    }

    public void SetBGM(float volume)
    {
        float db = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;

        m_audioMix.SetFloat("BGM", db);
        m_bgmVolume = volume;
        Save();

    }

    public void SetSE(float volume)
    {
        float db = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;

        m_audioMix.SetFloat("SE", db);
        m_seVolume = volume;

        Save();
    }

    private void Save()
    {
        AudioSaveData data = new AudioSaveData()
        {
            masterVolume = m_masterVolume,
            bgmVolume = m_bgmVolume,
            seVolume = m_seVolume,
        };

        m_optionSaveManager.OnAudioSave(data);
    }

    // BGMÇÃPitchïœçX
    public void SetBGMPitch(float pitch)
    {
        m_bgmPitch = pitch;
        BGMSource.pitch = pitch;
    }

    // SEÇÃPitchïœçX
    public void SetSEPitch(float pitch)
    {
        m_sePitch = pitch;
        SESource.pitch = pitch;
    }

    public void PlayAudio(AudioData data)
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

    public AudioData GetNowBGM()
    {
        return m_nowBGMData;
    }

    private void PlayBGM(AudioData data)
    {
        m_nowBGMData = data;
        BGMSource.pitch = data.pitch * m_bgmPitch;
        audioFader.FadeOutAndPlay(data.audioClip, data.volume);
    }

    private void PlaySE(AudioData data)
    {
        SESource.pitch = data.pitch * m_sePitch;
        SESource.PlayOneShot(data.audioClip, data.volume);
    }

    public void InstancePlayAudio(AudioData data)
    {
        m_nowBGMData = data;
        audioFader.InstancePlay(data.audioClip, data.volume);
    }

    public void StopBGM(float fadeDuration = 1f)
    {
        m_nowBGMData = null;
        audioFader.FadeOutAndStop(fadeDuration);
    }
}

//using UnityEngine;
//using UnityEngine.Audio;

//public class AudioManager : MonoBehaviour
//{
//    public static AudioManager Instance;

//    [SerializeField] private AudioSource BGMSource;
//    [SerializeField] private AudioSource SESource;
//    [SerializeField] private AudioMixer m_audioMix;
//    //[SerializeField] private FloatRunTime m_bgmVolume;
//    //[SerializeField] private FloatRunTime m_seVolume;

//    [SerializeField] private AudioFader audioFader;

//    private AudioData m_nowBGMData;

//    //--option save set--------------------
//    private OptionSaveManager m_optionSaveManager = new();
//    private float m_masterVolume;
//    private float m_bgmVolume;
//    private float m_seVolume;
//    private float m_bgmpitch = 1f;
//    private float m_sepitch = 1f;


//   // private Coroutine bgmFadeCoroutine;
//    private void Awake()
//    {
//        //Singleton
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }
//        Instance = this;
//        DontDestroyOnLoad(gameObject);
//        audioFader.audioSource = BGMSource;
//    }

//    private void Start()
//    {
//        var data = m_optionSaveManager.OnAudioLoad();

//        if (data != null)
//        {
//            SetMaster(data.m_masterVolume);
//            SetBGM(data.m_bgmVolume);
//            SetSE(data.m_seVolume);
//            SetBGMPitch(1f);
//            SetSEPitch(1f);
//        }
//        else
//        {
//            SetMaster(0.8f);
//            SetBGM(0.8f);
//            SetSE(0.8f);
//            SetBGMPitch(0.8f);
//            SetSEPitch(0.8f);
//        }
//    }

//    public void SetMaster(float volume)
//    {
//        float db = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;

//        m_audioMix.SetFloat("Master", db);
//        m_masterVolume = volume;
//        //m_bgmVolume.SetValue(volume);

//        OnSaveAudio();
//    }


//    public void SetBGM(float volume)
//    {
//        float db = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;

//        m_audioMix.SetFloat("BGM", db);
//        m_bgmVolume = volume;
//        //m_bgmVolume.SetValue(volume);

//        OnSaveAudio();

//    }

//    public void SetSE(float volume)
//    {
//        float db = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;

//        m_audioMix.SetFloat("SE", db);
//        m_seVolume = volume;
//        //m_seVolume.SetValue(volume);

//        OnSaveAudio();

//    }

//    public void SetBGMPitch(float pitch)
//    {
//       m_bgmpitch = pitch;

//        BGMSource.pitch = pitch;

//    }

//    public void SetSEPitch(float pitch)
//    {
//        m_sepitch = pitch;

//        SESource.pitch = pitch;

//    }

//    public void OnSaveAudio()
//    {
//      //  AudioData data = new()
//      //  {
//      //      m_masterVolume = m_masterVolume,
//      //      m_bgmVolume = m_bgmVolume,
//      //      m_seVolume = m_seVolume,
//      //      m_bgmPitch = m_bgmpitch,
//      //      m_sePitch = m_sepitch
//      //
//      //  };
//      //  
//      //  m_optionSaveManager.OnAudioSave(data);
//    }

//    //EventSOÇ©ÇÁìnÇ≥ÇÍÇΩAudioClipÇçƒê∂Ç∑ÇÈ
//    public void PlayAudio(AudioData data)
//    {
//        if (data.isLoop)
//        {
//            PlayBGM(data);

//        }
//        else
//        {
//            PlaySE(data);
//        }
//    }

//    public AudioData GetNowBGM()
//    {
//        return m_nowBGMData;
//    }

//    private void PlayBGM(AudioData data)
//    {
//        m_nowBGMData = data;

//        BGMSource.pitch = data.pitch * m_bgmpitch;

//        audioFader.FadeOutAndPlay(data.audioClip, data.volume);
//    }
//    private void PlaySE(AudioData data)
//    {
//        SESource.pitch = data.pitch * m_sepitch;
//        SESource.PlayOneShot(data.audioClip, data.volume);
//    }

//    public void InstancePlayAudio(AudioData data)
//    {
//        m_nowBGMData = data;
//        audioFader.InstancePlay(data.audioClip, data.volume);
//    }
//}
