using UnityEngine;

public class ButtonSystem : MonoBehaviour
{
    [SerializeField] private EventSO m_eventSO;

    /// <summary>
    /// EventSO‚Å‹N“®‚³‚¹‚½ŠÖ”‚ÌŒÄ‚Ño‚µ
    /// </summary>
    public void CallMethod()
    {
        m_eventSO.Raise();
    }
}
