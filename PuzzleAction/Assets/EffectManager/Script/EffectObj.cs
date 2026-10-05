using UnityEngine;

public class EffectObj : MonoBehaviour
{
    private ReturnObjectToPool m_returnObjPool;

    [SerializeField] private float m_lifeTime;

    private void Awake()
    {
        m_returnObjPool = GetComponent<ReturnObjectToPool>();
    }

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