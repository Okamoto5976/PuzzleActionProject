using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Entity
{
    [Header("InputSystem")]
    [SerializeField] private InputActionReference m_moveAction;
    [SerializeField] private InputActionReference m_dashAction;

    [SerializeField] private Vector3Asset m_position;

    [Header("Dash")]
    [SerializeField] private float m_dashDuration = 0.2f;

    private bool m_isDashing;
    private float m_dashTimer;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        m_moveAction.action.Enable();
        m_dashAction.action.Enable();

        m_dashAction.action.performed += OnDashPerformed;
    }

    private void OnDisable()
    {
        m_dashAction.action.performed -= OnDashPerformed;

        m_moveAction.action.Disable();
        m_dashAction.action.Disable();
    }

    private void OnDashPerformed(InputAction.CallbackContext context)
    {
        m_isDashing = true;
        m_dashTimer = m_dashDuration;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();

        m_position.SetValue(transform.position);

        // 移動入力
        Vector2 input = m_moveAction.action.ReadValue<Vector2>();
        m_moveDir = new Vector3(input.x, 0f, input.y);

        // 移動方向に向く
        if (m_moveDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(m_moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                Time.deltaTime * 10f);
        }

        // ダッシュ時間管理
        if (m_isDashing)
        {
            m_dashTimer -= Time.deltaTime;

            if (m_dashTimer <= 0f)
            {
                m_isDashing = false;
            }
        }

        // 速度切り替え
        m_currentMoveSpeed = m_isDashing ? DashSpeed : Speed;
    }
}