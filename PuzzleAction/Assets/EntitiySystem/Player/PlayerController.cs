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
    private InputProvider m_input;

    private Vector2 m_move;

    private bool m_isActive;
    private bool m_isEvasion;
    private bool m_isPrevious;
    private bool m_isNext;

    //[SerializeField] private InputActionReference m_moveAction;
    //[SerializeField] private InputActionReference m_evasionAction;
    //[SerializeField] private InputActionReference m_hotberOne;
    //[SerializeField] private InputActionReference m_hotberTwo;
    //[SerializeField] private InputActionReference m_hotberThree;

    [SerializeField] private Vector3Asset m_position;

    [Header("Evasion")]
    [SerializeField] private float m_evasionDuration = 0.2f;

    private List<PassiveModifier> m_modifiers = new();

    [Header("InventorySystem")]
    [SerializeField] private InventorySystem m_inventorySystem;

    //private bool m_isEvaing;
    //private float m_evasionTimer;


    private int m_hotberIndex = 0;

    //---------passive bool---------------

    //private bool 

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        m_input = new InputProvider();

        m_input.Enable();
    }

    private void OnEnable()
    {
        //m_action = new InputSystem_Actions();


        //m_moveAction.action.Enable();
        //m_evasionAction.action.Enable();
        //m_hotberOne.action.Enable();
        //m_hotberTwo.action.Enable();
        //m_hotberThree.action.Enable();


        //m_evasionAction.action.performed += OnEvasionPerformed;
        //m_hotberOne.action.performed += 

        //m_action.Enable();
    }

    private void OnDisable()
    {
        //m_moveAction.action.Disable();
        //m_evasionAction.action.Disable();
        //m_hotberOne.action.Disable();
        //m_hotberTwo.action.Disable();
        //m_hotberThree.action.Disable();

        //m_evasionAction.action.performed -= OnEvasionPerformed;
        m_input.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        
    }

    private void OnEvasionPerformed(InputAction.CallbackContext context)
    {
        //m_isEvaing = true;
        //m_evasionTimer = m_evasionDuration;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();

        m_position.SetValue(transform.position);

        InputMove();

        m_isActive = m_input.IsActive;
        m_isEvasion = m_input.IsEvasion;
        m_isPrevious = m_input.IsPrevious;
        m_isNext = m_input.IsNext;

        InputHotber();

        if(m_isActive)
        {
            OnUseItem();

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
        //if (m_isEvasion)
        //{
        //    m_evasionTimer -= Time.deltaTime;

        //    if (m_evasionTimer <= 0f)
        //    {
        //        m_isEvasion = false;
        //    }
        //}

        // 速度切り替え
        m_currentMoveSpeed = m_isEvasion ? EvasionSpeed : Speed;
    }

    private void InputMove()
    {
        // 移動入力
        Vector2 input = m_input.Move;
        m_moveDir = new Vector3(input.x, 0f, input.y);

        //移動時の左右反転

        if (input.x > 0.1f)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        else if (input.x < -0.1f)
        {
            transform.localScale = new Vector3(-2, 2, 2);
        }
    }

    private void InputHotber()
    {
        if(m_isPrevious)
        {
            m_hotberIndex--;

            if(m_hotberIndex <= -1)
            {
                m_hotberIndex = 2;
            }
        }

        if(m_isNext)
        {
            m_hotberIndex++;

            if (m_hotberIndex >= 3)
            {
                m_hotberIndex = 0;
            }
        }
    }


    private void OnUseItem()
    {
        return;

        ItemRecieveData data = new ItemRecieveData
        {
            entity = this,
            baseValue = STR,
            pos = transform.position,
            dir = transform.forward
        };
        Debug.Log(m_hotberIndex);
        m_inventorySystem.Use(m_hotberIndex , data);
    }
}
