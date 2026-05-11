using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(EntityBase))]
public class PlayerController : Entity
{
    [SerializeField] private float m_normalSpeed = 5f;
    [SerializeField] private float m_dashSpeed = 10f;

    [Header("InputSystem")]
    [SerializeField] private InputActionReference m_action;
    [SerializeField] private InputActionReference m_dashAction;

    //[SerializeField] private EntityMove m_move;

    private bool m_isDashing;

    private PlayerState m_state;

    protected override void Awake()
    {
        base.Awake();

        m_state = GetComponent<PlayerState>();

    }

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

    //protected override void FixedUpdate()
    //{
    //    base.FixedUpdate();

    //    //Player独時
    //}

    private void Update()
    {
        if(m_state != null && !m_state.CanMove)
        {
            return;
        }

        //移動処理
        Vector2 input = m_action.action.ReadValue<Vector2>();
        m_movement = new Vector3(input.x, 0f, input.y);

        //ダッシュ判定（押している間）
        m_isDashing = m_dashAction.action.IsPressed();

        m_speed = m_isDashing ? m_dashSpeed : m_normalSpeed;
    }




}
