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

    [Header("MenuScene")]
    [SerializeField] private GameObject[] m_scene;

    public void TransitionTitle()
    {
        m_scene[(int)Scene.Option].SetActive(false);
        m_scene[(int)Scene.Title].SetActive(true);
    }

    public void TransitionOption()
    {
        m_scene[(int)Scene.Title].SetActive(false);
        m_scene[(int)Scene.Option].SetActive(true);
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
}
