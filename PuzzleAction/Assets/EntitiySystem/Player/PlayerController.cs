using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerController : Entity
{
    //---------input------------------
    private InputProvider m_input;
    
    private Vector2 m_move;

    private bool m_isActive;
    private bool m_isActiveHold;
    private bool m_isActiveRelease;
    private bool m_isPrevious;
    private bool m_isNext;
    private bool m_isInteract;
    private bool m_isGetDropItem;

    //--------component--------------
    [Header("Component")]
    private PlayerItemController m_playerItemController;

    [SerializeField] private InventorySystem m_inventorySystem;

    [SerializeField] private DisplayManager m_displayManager;



    [SerializeField] private Vector3Asset m_position;
    public Vector3 m_pullOffSet;

    [SerializeField] private SpriteRenderer m_spriteRenderer;

    private int m_hotberIndex = 0;



    [Header("Evasion")]
    [SerializeField] private float m_evasionDuration = 0.2f;
    private float m_evasionTimer;

    private EntityPassiveBuffSystem m_passiveSystem;

    public Vector3 MoveDirection => m_moveDir;



    //when open shop, can not Input
    private bool m_ignoreInput = false;
    [SerializeField] private BoolEventSO m_canInputEvent;

    //--------player foward -----------------
    [Header("PlayerFoward")]
    [SerializeField] private GameObject m_playerDirObject;
    public Vector3 Forward => m_playerDirObject.transform.forward;


    //--------ItemController----------------
    [Header("ItemController")]
    public RectTransform m_reticle;
    public AimTrail m_aimTrail;
    public LayerMask m_itemLayerMask;

    public GameObject m_trapPreview;




    //InteractSystem
    [Header("Interact")]
    private InteractSystem m_interactSystem;
    [SerializeField] private LayerMask m_interactLayer;


    //------SearchItem-------------------------
    [Header("UI")]
    public RectTransform m_textPanel;
    [SerializeField] private TMPro.TMP_Text m_nameText;
    [SerializeField] private TMPro.TMP_Text m_itemDescriptionText;

    //if inventory max 
    [SerializeField] private GameObject m_textErrorMessage;

    //----passive effect---------------------
    [HideInInspector] public bool m_isCoupon;
    [HideInInspector] public bool m_isMemberShip;
    [HideInInspector] public bool m_isWinnerTrophy;
    [HideInInspector] public bool m_isLoserTrophy;
    [HideInInspector] public bool m_isNormalTrophy;
    [HideInInspector] public bool m_titleTrophy;


    protected override void Awake()
    {
        base.Awake();

        m_playerItemController = new(this, m_inventorySystem);

        if(m_buffSystem != null)
        {
            m_buffSystem.SetPlayer(m_displayManager);
        }

        m_passiveSystem = GetComponent<EntityPassiveBuffSystem>();
    }


    protected override void Start()
    {
        base.Start();

        ReticleActive(false);


        m_interactSystem = new();

        m_input = new InputProvider();
        
        m_input.Enable();

        m_nameText.text = "";
        m_itemDescriptionText.text = "";
        ItemDescriptionPanelActive(false);
        TextErrorMessageActive(false);
    }

    private void OnEnable()
    {
        m_canInputEvent.Register(SetCanInput);
    }

    private void OnDisable()
    {
        m_canInputEvent.Unregister(SetCanInput);

        m_input.Disable();
    }

    private void FixedUpdate()
    {
        m_isActive = m_input.IsActive;

        InputMove();

    }

    private void Update()
    {
        if(m_currentState == EntityState.Dead) return;

        UpdateFlag();

        m_position.SetValue(transform.position);

        if (m_ignoreInput) return;
             
        m_isActive = m_input.IsActive;
        m_isActiveHold = m_input.IsActiveHold;
        m_isActiveRelease = m_input.IsActiveRelease;
        m_isPrevious = m_input.IsPrevious;
        m_isNext = m_input.IsNext;
        m_isInteract = m_input.IsInteract;
        m_isGetDropItem = m_input.IsGetDropItem;

        if (m_isGetDropItem)
        {
            m_playerItemController.GetItem();
        }

        if (m_isInteract)
        {
            OnInteract();
        }

        if (m_input.IsEvasion)
        {
            OnEvadeInput();
        }

        DoEvading();


        if (m_isActive)
        {
            m_playerItemController.UseItemPressed(m_hotberIndex);
        }

        if (m_isActiveHold)
        {
            m_playerItemController.UseItemHold();
        }

        if (m_isActiveRelease)
        {
            m_playerItemController.UseItemRelease(m_hotberIndex);
        }

        InputHotber();

        m_playerItemController.SearchItem();
    }

    /// <summary>
    /// written by so-
    /// </summary>
    private void OnEvadeInput()
    {
        if (IsEvading) return;
        IsEvading = true;
        m_evasionTimer = m_evasionDuration;
        SetIsInvincible(true);
    }

    /// <summary>
    /// written by so-
    /// </summary>
    private void DoEvading()
    {
        if (!IsEvading) return;

        m_evasionTimer -= Time.deltaTime;

        if (m_evasionTimer > 0f) return;

        IsEvading = false;
        SetIsInvincible(false);
    }

    private void InputMove()
    {
        if (m_currentState == EntityState.Dead) return;
        if (m_currentState == EntityState.Attack) return;

        if(!m_canMove ||
            IsStun)
        {
            Move(Vector3.zero, 0f);
            return;
        }

        if(IsKnockBack)
        {
            Move(m_knockBackVelocity, m_knockbackPower * 5f);
            return;
        }

        Vector2 input = m_input.Move;
        m_moveDir = new Vector3(input.x, 0f, input.y);

        if(!m_playerItemController.m_isUsingSetItem)
        {
            OnRotatePlayerDirObject(m_moveDir);

        }

        float slowMultiplier = 1f - Swamp * (1f - SlowRes);
        slowMultiplier = Mathf.Clamp(slowMultiplier, 0.25f, 1f);

        float finalSpeed = (Speed * slowMultiplier) - Slow;

        if (m_isEvading)
        {
            Move(m_evadeDirection, finalSpeed * 1.5f);
        }
        else
        {
            Move(m_moveDir, finalSpeed);
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
            m_spriteRenderer.flipX = false;
        }
        else if (input.x < -0.1f)
        {
            m_spriteRenderer.flipX = true;
        }
    }

    private void InputHotber()
    {
        //player use Arrow etc... not change hotber Item
        if (m_playerItemController.m_isUsingArrow) return;

        if (m_playerItemController.m_isUsingSetItem) return;

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

    [SerializeField] private float testknockback;

    [ContextMenu("ApplyKnockBack")]
    public void ApplyKnockBack()
    {
        DamageData data = new();
        {
            data.Attack = 0f;
            data.AttackDir = new Vector3(1, 0, 0);
            data.CriticalRate = 0f;
            data.CriticalDamage = 0f;
            data.BreakRate = 0;
            data.Knockback = testknockback;
            data.StunDuration = 0;
        }


        TakeDamage(data);
    }

    #region Object_SetActive_Method
    public void ReticleActive(bool active)
    {
        m_reticle.gameObject.SetActive(active);
    }

    public void SetItemPreview(bool active)
    {
        m_trapPreview.SetActive(active);
    }

    public void AimTrailActive(bool active)
    {
        m_aimTrail.gameObject.SetActive(active);
    }

    public void TextErrorMessageActive(bool active)
    {
        m_textErrorMessage.SetActive(active);
    }

    public void ItemDescriptionPanelActive(bool active)
    {
        m_textPanel.gameObject.SetActive(active);
    }

    public void ItemTextName(string name)
    {
        m_nameText.text = name;
    }

    public void ItemTextDescription(string description)
    {
        m_itemDescriptionText.text = description;
    }
    #endregion

    //this method is to rotate playerDirObject(arrowDir) 
    public void OnRotatePlayerDirObject(Vector3 moveDir)
    {
        //The arrow rotates only when there is input
        if (moveDir.sqrMagnitude > 0.01f)
        {
            m_playerDirObject.transform.rotation = Quaternion.LookRotation(moveDir, m_playerDirObject.transform.up);
        }
    }

    private void OnInteract()
    {
        m_interactSystem.TryInteract(transform.position, m_interactLayer, this);
    }

    public void AddPassive(List<StatusModifier> modifiers, Passive type)
    {
        //Debug.LogWarning(type);
        m_passiveSystem.AddPassive(modifiers, type);
    }

    public void RemovePassive(Passive type)
    {
        m_passiveSystem.RemoveBuff(type);
    }

    public bool CheckTrophy()
    {
        if(!m_isWinnerTrophy) return false;

        if (!m_isLoserTrophy) return false;

        if(!m_isNormalTrophy) return false;

        return true;
    }

    public void SetCanInput(bool ignoreInput)
    {
        m_ignoreInput = ignoreInput;

        if (!ignoreInput)
        {
            m_input.OnInputClear();
        }
    }

}
