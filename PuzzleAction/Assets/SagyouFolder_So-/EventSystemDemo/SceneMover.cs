using System;
using UnityEngine;

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

    public void MoveSceneTo(SceneAsset sceneAsset)
    {
        Debug.Log($"SCENEMOVER >> moving to {sceneAsset.SceneName}");
    }
}
