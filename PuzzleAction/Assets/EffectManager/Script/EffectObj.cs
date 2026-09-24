using UnityEngine;

public class EffectObj : MonoBehaviour
{
    private ReturnObjectToPool m_returnObjPool;

    [SerializeField] private float m_lifeTime;

    public void Initialize()
    {

        CancelInvoke();

        Invoke(nameof(Return), m_lifeTime);
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