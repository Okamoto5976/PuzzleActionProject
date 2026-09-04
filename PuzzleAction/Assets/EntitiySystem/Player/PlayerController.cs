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
    private bool m_isActiveHold;
    private bool m_isActiveRelease;
    private bool m_isEvasion;
    private bool m_isPrevious;
    private bool m_isNext;
    private bool m_isInteract;

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

    [SerializeField] private DisplayManager m_displayManager;

    public Vector3 MoveDirection => m_moveDir;

    private bool m_isEvaing;
    private float m_evasionTimer;

    //private bool m_isEvaing;
    //private float m_evasionTimer;


    [SerializeField] private int m_hotberIndex = 0;

    private bool m_isUsingArrow = false;
    private bool m_isUsingSetItem = false;

    //--------player foward -----------------
    [SerializeField] private GameObject m_playerDirObject;

    [SerializeField] private RectTransform m_reticle;

    //player forward
    public Vector3 Forward => m_playerDirObject.transform.forward;

    private Vector3 m_arrowTemporaryForward;

    //---------passive bool---------------

    //InteractSystem
    private InteractSystem m_interactSystem;
    [SerializeField] private LayerMask m_interactLayer;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        m_reticle.gameObject.SetActive(false);
        m_interactSystem = new();

        m_input = new InputProvider();
        
        m_input.Enable();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {

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

    private void FixedUpdate()
    {
        CallMove();
    }

    private void Update()
    {
        if(m_currentState == EntityState.Dead) return;

        OnUpdateFlag();

        m_position.SetValue(transform.position);


        m_isActive = m_input.IsActive;
        m_isActiveHold = m_input.IsActiveHold;
        m_isActiveRelease = m_input.IsActiveRelease; m_isEvasion = m_input.IsEvasion;
        m_isPrevious = m_input.IsPrevious;
        m_isNext = m_input.IsNext;
        m_isInteract = m_input.IsInteract;

        if(m_isInteract)
        {
            OnInteract();
        }

        InputMove();


        if (m_isActive)
        {
            OnUseItemPressed();
        }

        if (m_isActiveHold)
        {
            OnUseItemHold();
        }

        if (m_isActiveRelease)
        {
            OnUseItemRelease();
        }

        InputHotber();


        // �_�b�V�����ԊǗ�
        //if (m_isEvasion)
        //{
        //    m_evasionTimer -= Time.deltaTime;

        //    if (m_evasionTimer <= 0f)
        //    {
        //        m_isEvasion = false;
        //    }
        //}

        // ���x�؂�ւ�
        //m_currentMoveSpeed = m_isEvasion ? EvasionSpeed : Speed;
    }

    private void InputMove()
    {
        Vector2 input = m_input.Move;
        m_moveDir = new Vector3(input.x, 0f, input.y);

        if(!m_isUsingSetItem)
        {
            OnRotatePlayerDirObject(m_moveDir);

        }


        if (input.x != 0f || input.y != 0f)
        {
            m_anim.SetBool("Run", true);
        }
        else
        {
            m_anim.SetBool("Run", false);
        }


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
        //player use Arrow etc... not change hotber Item
        if (m_isUsingArrow) return;

        if (m_isUsingSetItem) return;

        if (m_isPrevious)
        {
            m_hotberIndex--;

            if (m_hotberIndex <= -1)
            {
                m_hotberIndex = 2;
            }
        }

        if (m_isNext)
        {

            m_hotberIndex++;

            if (m_hotberIndex >= 3)
            {
                m_hotberIndex = 0;
            }
        }

        m_displayManager.SetIndex(m_hotberIndex);
    }

    private ItemRecieveData CreateItemData(Vector3 forward)
    {
        return new ItemRecieveData
        {
            entity = this,
            baseValue = STR,
            pos = transform.position,
            dir = forward,
        };
    }
    private void OnUseItemPressed()
    {
        //Debug.Log("Pressed");

        if(m_inventorySystem.IsCheckCurrentItem(m_hotberIndex, ItemUseType.Arrow))
        {
            m_isUsingArrow = true;

            m_reticle.gameObject.SetActive(true);
            //start to pull the bow
            


        }
        else if(m_inventorySystem.IsCheckCurrentItem(m_hotberIndex, ItemUseType.Set))
        {

        }
        else
        {
            ItemRecieveData data = CreateItemData(Forward);

            m_inventorySystem.UsePressed(m_hotberIndex, data);

        }

    }

    private void OnUseItemHold()
    {
        //Debug.Log("Hold");

        if(m_isUsingArrow)
        {
            OnReticle();
        }
    }

    private void OnUseItemRelease()
    {
        //Debug.Log("Release");

        if(m_isUsingArrow)
        {
            m_isUsingArrow = false;

            m_reticle.gameObject.SetActive(false);


            ItemRecieveData data = CreateItemData(m_arrowTemporaryForward);
            m_inventorySystem.UseRelease(m_hotberIndex, data);

        }
    }

    private void OnReticle()
    {
        //Debug.Log("reticle");

        m_reticle.position = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane plane = new Plane(Vector3.up, new Vector3(0f, transform.position.y, 0f));

        if(plane.Raycast(ray, out float distance))
        {
            Vector3 mousePos = ray.GetPoint(distance);

            Vector3 dir = mousePos - transform.position;
            dir.y = 0f;

            OnRotatePlayerDirObject(dir);

            //temporary save, use when arrow pull
            m_arrowTemporaryForward = Forward;
        }

    }

    //this method is to rotate playerDirObject(arrowDir) 
    private void OnRotatePlayerDirObject(Vector3 moveDir)
    {
        //The arrow rotates only when there is input
        if (moveDir.sqrMagnitude > 0.01f)
        {
            m_playerDirObject.transform.rotation =
                Quaternion.LookRotation(moveDir, m_playerDirObject.transform.up);
        }
    }

    private void OnInteract()
    {
        m_interactSystem.TryInteract(transform.position, m_interactLayer, this);
    }
}
