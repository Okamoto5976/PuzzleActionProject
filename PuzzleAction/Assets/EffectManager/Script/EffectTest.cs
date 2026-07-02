using UnityEngine;
using UnityEngine.InputSystem;

public class EffectTest : MonoBehaviour
{
    [SerializeField]
    private EffectManager m_effectManager;

    [SerializeField]
    private int m_effectIndex;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            m_effectManager.PlayEffect(
                m_effectIndex,
                Vector3.zero);
        }
    }
}