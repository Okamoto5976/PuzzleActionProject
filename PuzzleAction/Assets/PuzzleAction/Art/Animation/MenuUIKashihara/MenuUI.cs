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

    [SerializeField] private Slider m_bgmSlider;
    [SerializeField] private Slider m_seSlider;

    [SerializeField] private FloatRunTime m_bgmVolume;
    [SerializeField] private FloatRunTime m_seVolume;



    [Header("MenuScene")]
    [SerializeField] private GameObject[] m_scene;

    private void OnEnable()
    {
        TransitionTitle();
    }

    private void Start()
    {
        m_bgmSlider.value = m_bgmVolume.Value;
        m_seSlider.value = m_seVolume.Value;
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
        Time.timeScale = 1.0f;
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

    public void SetBGMVolume(float value)
    {
        AudioManager.instance.SetBGM(value);
    }

    public void SetSEVolume(float value)
    {
        AudioManager.instance.SetSE(value);
    }
}
