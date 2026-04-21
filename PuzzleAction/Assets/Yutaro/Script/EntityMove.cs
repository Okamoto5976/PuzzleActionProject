using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class EntityMove:MonoBehaviour
{
    private Rigidbody m_rb;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    public void OnMove(Vector3 movement, float speed)
    {
        //m_isDashing? m_dashSpeed:m_speed;
        float m_currentSpeed = speed;

        Vector3 velocity = new Vector3(movement.x * m_currentSpeed, m_rb.linearVelocity.y, movement.z * m_currentSpeed);

        m_rb.linearVelocity = velocity;
    }
}
