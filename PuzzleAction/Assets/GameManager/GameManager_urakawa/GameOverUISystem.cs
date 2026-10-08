using UnityEngine;

public class GameOverUISystem : MonoBehaviour
{
    [SerializeField] private SceneEventScript m_sceneEvent;

    [SerializeField] private StaticSceneAsset m_exitScene;

    [SerializeField] private AudioData m_se;

    public void OnExit()
    {
        //m_sceneEvent.TriggerEvent(m_exitScene);
        AudioManager.Instance.PlayAudio(m_se);

        LoadManager.m_instance.LoadScene(m_exitScene.Value);
    }
}
