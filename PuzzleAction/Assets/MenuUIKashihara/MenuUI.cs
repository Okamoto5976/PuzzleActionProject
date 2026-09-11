using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [System.Serializable]
    public enum Scene
    {
        Title,
        Option,
        [NonSerialized] Count
    }

    public bool IsOption { get; private set; }

    [SerializeField] private Slider m_masterSlider;
    [SerializeField] private Slider m_bgmSlider;
    [SerializeField] private Slider m_seSlider;

    //[SerializeField] private FloatRunTime m_bgmVolume;
    //[SerializeField] private FloatRunTime m_seVolume;

    //-----audio save set-------------
    private OptionSaveManager m_optionSaveManager = new();


    [Header("MenuScene")]
    [SerializeField] private GameObject[] m_scene;

    private void OnEnable()
    {
        TransitionTitle();
    }

    private void Start()
    {
        var data = m_optionSaveManager.OnAudioLoad();

        if(data != null)
        {
            m_masterSlider.value = data.m_masterVolume;
            m_bgmSlider.value = data.m_bgmVolume;
            m_seSlider.value = data.m_seVolume;
        }
        else
        {
            m_masterSlider.value = 0.8f;
            m_bgmSlider.value = 0.8f;
            m_seSlider.value = 0.8f;
        }

    }

    public void TransitionTitle()
    {
        m_scene[(int)Scene.Option].SetActive(false);
        m_scene[(int)Scene.Title].SetActive(true);
        IsOption = false;

    }

    public void TransitionOption()
    {
        m_scene[(int)Scene.Title].SetActive(false);
        m_scene[(int)Scene.Option].SetActive(true);
        IsOption = true;
    }

    public void Back()
    {
        Debug.Log("Back");
        gameObject.SetActive(false);

        GameManager.Instance.OnSetStop(false);

    }



    //impossible
    //public void Transition(Scene scene)
    //{
    //for (int i = 0; i < (int)Scene.Count; i++)
    //{
    //    if (i == (int)scene) continue;
    //    m_scene[i].SetActive(false);
    //}
    //m_scene[(int)scene].SetActive(true);
    //}


    public void TransitionGame()
    {
        Debug.Log("Start!!!");
    }

    public void ExitSesssion()
    {
        Debug.Log("Exit");
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.instance.SetMaster(value);
    }

    public void SetBGMVolume(float value)
    {
        AudioManager.instance.SetBGM(value);
    }

    public void SetSEVolume(float value)
    {
        AudioManager.instance.SetSE(value);
    }
}
