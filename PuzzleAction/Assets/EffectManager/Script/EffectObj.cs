using UnityEngine;

public class EffectObj : MonoBehaviour
{
    private ReturnObjectToPool m_returnObjPool;

    public void Initialize(float lifeTime)
    {

        CancelInvoke();

        Invoke(nameof(Return), lifeTime);
    }

    private void Return()
    {
        if (m_returnObjPool == null)
        {
            m_returnObjPool = GetComponent<ReturnObjectToPool>();

        }
        m_returnObjPool.ReturnToPool();
    }
}