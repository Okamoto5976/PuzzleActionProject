using UnityEditor;
using UnityEngine;

public class GameClearUISystem : MonoBehaviour
{
    [SerializeField] private SceneEventScript m_sceneEvent;

    [SerializeField] private StaticSceneAsset m_nextScene;
    [SerializeField] private StaticSceneAsset m_exitScene;


    public void OnNextStage()
    {
        m_sceneEvent.TriggerEvent(m_nextScene);
    }

    public void OnExit()
    {
        m_sceneEvent.TriggerEvent(m_exitScene);
    }
}
