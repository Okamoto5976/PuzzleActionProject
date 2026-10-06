using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    [SerializeField] private AudioData m_footSE;
    [SerializeField] private AudioData m_footAnotherSE;

    private bool m_value;

    public void OnWalkAnimEvent()
    {
        if(m_value)
        {
            m_value = false;
            AudioManager.Instance.PlayAudio(m_footSE);
        }
        else
        {
            m_value = true;
            AudioManager.Instance.PlayAudio(m_footAnotherSE);

        }
    }

}
