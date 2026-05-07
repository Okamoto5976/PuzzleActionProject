using UnityEngine;
using UnityEngine.InputSystem;

public class T_Enemy : MonoBehaviour
{
    [SerializeField] private float m_normalSpeed = 5f;
    [SerializeField] private float m_dashSpeed = 10f;

    private float m_speed;

    [Header("InputSystem")]
    [SerializeField] private InputActionReference m_action;
    [SerializeField] private InputActionReference m_dashAction;

    [SerializeField] private EntityMove m_move;

    private bool m_isDashing;

    private Vector3 m_movement;


    //private void OnEnable()
    //{
    //    m_action.Enable();
    //    m_dashAction.Enable();
    //}

    //private void OnDisable()
    //{
    //    m_action.Disable();
    //    m_dashAction.Disable();
    //}

    private void Update()
    {
        //移動処理
        Vector2 input = m_action.action.ReadValue<Vector2>();
        m_movement = new Vector3(input.x, 0f, input.y);

        //ダッシュ判定（押している間）
        m_isDashing = m_dashAction.action.IsPressed();

        m_speed = m_isDashing ? m_dashSpeed : m_normalSpeed;
    }

    private void FixedUpdate()
    {
        m_move.OnMove(m_movement, m_speed);
    }
}
