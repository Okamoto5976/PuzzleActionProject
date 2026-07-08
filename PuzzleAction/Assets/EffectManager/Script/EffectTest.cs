using UnityEngine;
using UnityEngine.InputSystem;

public class EffectTest : MonoBehaviour
{
    //[SerializeField]
    //private EffectManager m_effectManager;

    //[SerializeField]
    //private int m_effectIndex;

    [SerializeField] private EffectEventDataSO m_effectEventData;

    [SerializeField] private EffectData m_effectData;

    [SerializeField] private Entity m_player;

    private Effect m_effect;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            m_effect = new Effect
            {
                effectData = m_effectData,
                effectPos = m_player.transform.position,
                effectRot = Quaternion.identity
            };

            m_effectEventData.Raise(m_effect);

            //m_effectManager.PlayEffect(
            //    m_effectIndex,
            //    Vector3.zero);
        }
    }
}