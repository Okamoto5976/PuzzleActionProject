using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerController : Entity
{
    public enum Passive
    {
        PriceDown,
        
    }

    public class PassiveModifier
    {
        public Passive m_passive;
    }

    [Header("InputSystem")]
    [SerializeField] private InputActionReference m_moveAction;
    [SerializeField] private InputActionReference m_evasionAction;

    [SerializeField] private Vector3Asset m_position;

    [Header("Evasion")]
    [SerializeField] private float m_evasionDuration = 0.2f;

    private List<PassiveModifier> m_modifiers = new();

    private bool m_isEvaing;
    private float m_evasionTimer;

    //---------passive bool---------------

    //private bool 

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        m_moveAction.action.Enable();
        m_evasionAction.action.Enable();

        m_evasionAction.action.performed += OnEvasionPerformed;
    }

    private void OnDisable()
    {
        m_evasionAction.action.performed -= OnEvasionPerformed;

        m_moveAction.action.Disable();
        m_evasionAction.action.Disable();
    }

    private void OnEvasionPerformed(InputAction.CallbackContext context)
    {
        m_isEvaing = true;
        m_evasionTimer = m_evasionDuration;
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

        if(input.x != 0f ||  input.y != 0f)
        {
            m_anim.SetBool("Run", true);
        }
        else
        {
            m_anim.SetBool("Run", false);
        }

        //移動時の左右反転

        if (input.x > 0.1f)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        else if (input.x < -0.1f)
        {
            transform.localScale = new Vector3(-2, 2, 2);
        }

        /*
        移動時の左右奥手前反転
        if(input.x>0.1f)
        {
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if(input.x>-0.1f)
        {
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);

        }
        else if (input.y > 0.1f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (input.y > -0.1f)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        }
        */

        /*
        移動方向に向く
        if (m_moveDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(m_moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                Time.deltaTime * 10f);
        }
        */

        // ダッシュ時間管理
        if (m_isEvaing)
        {
            m_evasionTimer -= Time.deltaTime;

            if (m_evasionTimer <= 0f)
            {
                m_isEvaing = false;
            }
        }

        // 速度切り替え
        m_currentMoveSpeed = m_isEvaing ? EvasionSpeed : Speed;
    }
}
