using UnityEngine;

public class AutoReturn : MonoBehaviour
{
    private EffectManager m_manager;

    public void Initialize(
        EffectManager manager,
        float lifeTime)
    {
        m_manager = manager;

        CancelInvoke();

        Invoke(nameof(Return), lifeTime);
    }

    private void Return()
    {
        m_manager.ReturnPool(gameObject);
    }
}