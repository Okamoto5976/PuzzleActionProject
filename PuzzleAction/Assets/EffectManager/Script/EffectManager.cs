using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private EffectPoolManager m_effectPoolManager;

    [SerializeField]
    private EffectData[] m_effectDatas;

    public void PlayEffect(int index, Vector3 position)
    {
        if (index < 0 || index >= m_effectDatas.Length)
            return;

        EffectData data = m_effectDatas[index];

        GameObject obj =
            m_effectPoolManager.Get(data.EffectPrefab);

        obj.transform.SetPositionAndRotation(
            position,
            Quaternion.identity);

        ParticleSystem particle =
            obj.GetComponent<ParticleSystem>();

        if (particle != null)
        {
            particle.Clear();
            particle.Play();
        }

        AutoReturn autoReturn =
            obj.GetComponent<AutoReturn>();

        if (autoReturn != null)
        {
            autoReturn.Initialize(
                this,
                data.Duration);
        }
    }

    public void ReturnPool(GameObject obj)
    {
        m_effectPoolManager.Return(obj);
    }
}