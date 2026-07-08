using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    private Middleman_Effect m_effectPool;

    //[SerializeField]
    //private EffectData[] m_effectDatas;

    [SerializeField] EffectEventDataSO m_effectEventSO;

    private void OnEnable()
    {
        m_effectEventSO.Register(PlayEffect);
    }

    private void OnDisable()
    {
        m_effectEventSO.Unregister(PlayEffect);
    }

    public void PlayEffect(Effect data)
    {
        //if (index < 0 || index >= m_effectDatas.Length)
        //    return;

        //Enum_EffectType type = Enum_EffectType.Hit;

        //EffectData data = m_effectDatas[index];

        //GameObject obj =
        //    m_effectPool.Get(data.EffectPrefab);

        EffectObj obj = m_effectPool.GetEffect(data.effectData.Type);

        obj.transform.SetPositionAndRotation(
            data.effectPos,
            data.effectRot);

        obj.gameObject.SetActive(true);

        ParticleSystem particle =
            obj.GetComponent<ParticleSystem>();

        if (particle != null)
        {
            particle.Clear();
            particle.Play();
        }

        obj.Initialize(data.effectData.Duration);


        //AutoReturn autoReturn =
        //    obj.GetComponent<AutoReturn>();

        //if (autoReturn != null)
        //{
        //    autoReturn.Initialize(
        //        this,
        //        data.Duration);
        //}
    }

    //public void ReturnPool(GameObject obj)
    //{
    //    m_effectPool.Return(obj);
    //}
}