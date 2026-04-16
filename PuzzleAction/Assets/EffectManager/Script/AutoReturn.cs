using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements.Experimental;

public class AutoReturn : MonoBehaviour
{
    private float m_lifeTime;
    private EffectPoolManager m_pool;

    public void Initialize(EffectPoolManager pool,float time)
    {
        m_lifeTime = time;
        m_pool = pool;

        Invoke(nameof(Return), m_lifeTime);
    }
    private void Return()
    {
        //指定したオブジェクトをpoolに戻す
    }
}
