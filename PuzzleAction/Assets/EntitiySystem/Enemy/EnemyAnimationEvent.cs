using UnityEngine;
using UnityEngine.Events;


public class EnemyAnimationEvent : MonoBehaviour
{
    public UnityEvent m_dieEvent;
    public UnityEvent m_attackEvent;


    public void OnDieAnimEvent()
    {
        if(m_dieEvent != null)
        {
            m_dieEvent.Invoke();
        }
    }

    public void OnAttackAnimEvent()
    {
        if(m_attackEvent != null)
        {
            m_attackEvent.Invoke();
        }
    }
}
