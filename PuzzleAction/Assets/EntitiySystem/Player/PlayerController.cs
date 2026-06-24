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

    [SerializeField] private  Vector3Asset m_position;
    //[SerializeField] private EntityMove m_move;

    private bool m_isDashing;

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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        //Player独時
    }

    protected override void Update()
    {
        base.Update();

        //Debug
        m_position.SetValue(transform.position);

        //移動制御
        //if(m_currentState != null && !m_currentState.CanMove)
        //{
        //    return;
        //}

        //移動処理
        Vector2 input = m_moveAction.action.ReadValue<Vector2>();
        m_moveDir = new Vector3(input.x, 0f, input.y);

        //移動方向に向く
        if (m_moveDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(m_moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                Time.deltaTime * 10f);

        }

        //ダッシュ判定（押している間）
        m_isDashing = m_dashAction.action.IsPressed();

        m_currentMoveSpeed = m_isDashing ? DashSpeed : Speed;
    }

}
