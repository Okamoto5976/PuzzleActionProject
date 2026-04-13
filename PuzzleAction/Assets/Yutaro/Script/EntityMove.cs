using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Rigidbody))]
public class EntityMove:MonoBehaviour
{
    [SerializeField] private float m_speed = 5f;
    [SerializeField] private float m_dashSpeed = 10f;
    private bool m_isDashing;
    private Rigidbody m_rb;
    private Vector3 m_movement;

    [SerializeField] private InputAction m_action;
    [SerializeField] private InputAction m_dashAction;
    private void OnEnable()
    {
        m_action.Enable();
        m_dashAction.Enable();
    }

    private void OnDisable()
    {
        m_action.Disable();
        m_dashAction.Disable();
    }

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //移動処理
        Vector2 input = m_action.ReadValue<Vector2>();
        m_movement = new Vector3(input.x, 0f, input.y);

        //ダッシュ判定（押している間）
        m_isDashing = m_dashAction.IsPressed();
    }

    private void FixedUpdate()
    {
        float m_currentSpeed =m_isDashing?m_dashSpeed:m_speed;

       Vector3 velocity =new Vector3(m_movement.x*m_currentSpeed,m_rb.linearVelocity.y,m_movement.z*m_currentSpeed);

        m_rb.linearVelocity = velocity;
    }
}
