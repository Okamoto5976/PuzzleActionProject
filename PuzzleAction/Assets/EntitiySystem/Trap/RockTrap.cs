using UnityEngine;

public class RockTrap : TrapBase
{
    [SerializeField] private Rigidbody m_rb;
    private void OnEnable()
    {
        if (m_rb != null)
        {
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }
    }
}
