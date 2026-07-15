using UnityEngine;

public class GameOverUISystem : MonoBehaviour
{
    [SerializeField] private SceneEventScript m_sceneEvent;

    [SerializeField] private StaticSceneAsset m_exitScene;


    public void OnExit()
    {
        m_sceneEvent.TriggerEvent(m_exitScene);
    }
}
