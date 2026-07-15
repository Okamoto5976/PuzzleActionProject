using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMover : MonoBehaviour
{
    [SerializeField] private SceneEventScript m_sceneEventScript;


    private void OnEnable()
    {
        m_sceneEventScript.OnSceneEvent += MoveSceneTo;
    }

    private void OnDisable()
    {
        m_sceneEventScript.OnSceneEvent -= MoveSceneTo;
    }

    public void MoveSceneTo(StaticSceneAsset sceneAsset)
    {
        Debug.Log($"SCENEMOVER >> moving to {sceneAsset.Value}");
        SceneManager.LoadScene(sceneAsset.Value);
    }
}
