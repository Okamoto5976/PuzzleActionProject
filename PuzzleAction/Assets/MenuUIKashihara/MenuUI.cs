using System;
using UnityEngine;

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

    [Header("MenuScene")]
    [SerializeField] private GameObject[] m_scene;

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
        if (IsOption) IsOption = false;

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

    public void CloseOption()
    {
        IsOption = false;
    }


    public void TransitionGame()
    {
        Debug.Log("Start!!!");
    }

    public void ExitSesssion()
    {
        Debug.Log("Exit");
    }
}
