using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(EntityBase))]
public class PlayerController : Entity
{
    //[SerializeField] private PlayerData m_playerData;

    //private float m_normalSpeed;
    //private float m_dashSpeed;

    [Header("InputSystem")]
    [SerializeField] private InputActionReference m_moveAction;
    [SerializeField] private InputActionReference m_dashAction;

    [Header("Dash")]
    [SerializeField] private float m_dashTime = 0.15f;

    //[SerializeField] private EntityMove m_move;

    private bool m_isDash;
    private float m_dashTimer;

    //private PlayerState m_state;

    protected override void Awake()
    {
        base.Awake();

       // m_state = GetComponent<PlayerState>();
        
        //m_normalSpeed = m_playerData.NormalSpeed;
        //m_dashSpeed = m_playerData.DashSpeed;
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
        //移動制御
        //if(m_currentState != null && !m_currentState.CanMove)
        //{
        //    return;
        //}

        //移動処理
        Vector2 input = m_moveAction.action.ReadValue<Vector2>();
        //移動方向
        m_moveDir = new Vector3(input.x, 0f, input.y);
        //実際の移動用
        m_moveDir = m_moveDir.normalized;

        //移動方向に向く
        if (m_moveDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(m_moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                Time.deltaTime * 10f);

        }
        //ダッシュ開始
        if (m_dashAction.action.triggered && !m_isDash)
        {
            m_isDash = true;
            m_dashTimer = m_dashTime;
        }
    }

    protected override void FixedUpdate()
    {
        if (m_isStun) return;

        //ダッシュ中はDashSpeed
        float currentSpeed = m_isDash ? DashSpeed : Speed;

        Vector3 velocity = m_rb.linearVelocity;

        velocity.x=m_moveDir.x*currentSpeed;
        velocity.z=m_moveDir.z*currentSpeed;

        m_rb.linearVelocity = velocity;

        //ダッシュ時間
        if (m_isDash)
        {
            m_dashTimer -=Time.fixedDeltaTime;

            if(m_dashTimer<=0f)
            {
                m_isDash = false;
            }
        }
    }

}
