using Unity.Mathematics;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private EffectEventDataSO m_effectEventDataSO;


    [SerializeField] private EffectPoolManager m_effectPoolManager;

    private void OnEnable()
    {
        m_effectEventDataSO.Register(SetEffect);
    }
    private void OnDisable()
    {
        m_effectEventDataSO.Unregister(SetEffect);
    }
    public void SetEffect(Effect _data)
    {
        if (_data.m_effectPrefab == null) return;

        Vector3 position = _data.m_effectPos;
        Quaternion rotation = _data.m_effectRot;

        //var effectÇ…poolÇ©ÇÁÇŸÇµÇ¢effectèÓïÒÇí«â¡Ç∑ÇÈ


        //effect.GetComponent<AutoReturn>().Initialize(m_effectPoolManager,effect.m_lifeTime);
    }
}
