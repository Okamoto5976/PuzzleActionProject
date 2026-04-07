using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneEventScript", menuName = "Scriptable Objects/SceneEventScript")]
public class SceneEventScript : ScriptableObject
{
    public event Action<SceneAsset> OnSceneEvent;

    public void TriggerEvent(SceneAsset sceneAsset)
    {
        OnSceneEvent?.Invoke(sceneAsset);
    }
}
