using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerController : Entity
{
    [Header("InputSystem")]
    private InputProvider m_input;
    
    private Vector2 m_move;

    private bool m_isActive;
    private bool m_isActiveHold;
    private bool m_isActiveRelease;
    private bool m_isPrevious;
    private bool m_isNext;
    private bool m_isInteract;
    private bool m_isGetDropItem;

    [SerializeField] private Vector3Asset m_position;
    [SerializeField] private Vector3 m_pullOffSet;
    private Vector3 m_setOffSet;

    [Header("Evasion")]
    [SerializeField] private float m_evasionDuration = 0.2f;
    private float m_evasionTimer;

    private EntityPassiveBuffSystem m_passiveSystem;

    [Header("InventorySystem")]
    [SerializeField] private InventorySystem m_inventorySystem;

    [SerializeField] private DisplayManager m_displayManager;

    public Vector3 MoveDirection => m_moveDir;


    [SerializeField] private int m_hotberIndex = 0;

    private bool m_isUsingArrow = false;
    private bool m_isUsingSetItem = false;
    private bool m_isUsingAttackItem = false;

    //when open shop, can not Input
    private bool m_ignoreInput = false;
    [SerializeField] private BoolEventSO m_canInputEvent;

    //--------player foward -----------------
    [SerializeField] private GameObject m_playerDirObject;
    [SerializeField] private ParticleSystem m_trailParticle;

    [SerializeField] private RectTransform m_reticle;

    public Vector3 Forward => m_playerDirObject.transform.forward;

    private Vector3 m_arrowTemporaryForward;

    [Header("Set Trap on Space & Mouse")]
    [SerializeField] private float m_trapPlaceRange = 5f;
    [SerializeField] private GameObject m_trapPreview;
    [SerializeField] private GameObject m_trapRangeCircle;
    private Vector3 m_trapSetPosition;

    //InteractSystem
    private InteractSystem m_interactSystem;
    [Header("Interact")]
    [SerializeField] private LayerMask m_interactLayer;
    [Header("Item Search")]
    [SerializeField] private LayerMask m_itemSerachLayer;
    [SerializeField] private float m_itemSearchRange = 5f;
    [Header("UI")]
    [SerializeField] private GameObject m_textPanel;
    [SerializeField] private TMPro.TMP_Text m_nameText;
    [SerializeField] private TMPro.TMP_Text m_itemDescriptionText;

    //if inventory max 
    [SerializeField] private GameObject m_textErrorMessage;

    protected override void Awake()
    {
        base.Awake();

        if(m_buffSystem != null)
        {
            m_buffSystem.SetPlayer(m_displayManager);
        }

        m_passiveSystem = GetComponent<EntityPassiveBuffSystem>();
    }


    protected override void Start()
    {
        base.Start();

        m_reticle.gameObject.SetActive(false);
        m_interactSystem = new();

        m_input = new InputProvider();
        
        m_input.Enable();

        m_nameText.text = "";
        m_itemDescriptionText.text = "";
        m_textPanel.SetActive(false);
        m_textErrorMessage.SetActive(false);
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
        CallMove();
    }

    private void Update()
    {
        if(m_currentState == EntityState.Dead) return;

        OnUpdateFlag();

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
            //Debug.Log("PlayerController");
            OnuseItemGet();
        }

        if (m_isInteract)
        {
            OnInteract();
        }

        if (m_input.IsEvasion)
        {
            OnEvadeInput();
        }

        InputMove();
        DoEvading();


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

        if(m_isGetDropItem)
        {
            OnuseItemGet();
        }

        InputHotber();

        SearchItem();
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

    private ItemRecieveData CreateItemData(
        Vector3 forward,
        float power,
        Vector3 offset = new Vector3())
    {
        return new ItemRecieveData
        {
            entity = this,
            power = power,
            pos = m_trapSetPosition,
            dir = forward,
            offset = offset,

        };
    }

    //private ItemRecieveData CreatePullItemData(Vector3 forward, float power)
    //{
    //    return new ItemRecieveData
    //    {
    //        entity = this,
    //        power = power,
    //        pos = transform.position,
    //        dir = forward,
    //        offset = m_pullOffSet,
    //    };
    //}

    private void OnUseItemPressed()
    {
        //Debug.Log("Pressed");

        if(m_inventorySystem.IsCheckCurrentItem(m_hotberIndex, ItemUseType.Arrow))
        {
            m_isUsingArrow = true;

            m_power = 0f;
            m_reticle.gameObject.SetActive(true);

            m_trailParticle.gameObject.SetActive(true);

            //start to pull the bow
            


        }
        else if(m_inventorySystem.IsCheckCurrentItem(m_hotberIndex, ItemUseType.Set))
        {
            m_isUsingSetItem = true;
            m_power = 0f;

            m_reticle.gameObject.SetActive(true);

            m_trapPreview.gameObject.SetActive(true);
            m_trapRangeCircle.transform.position = gameObject.transform.position;
            m_trapRangeCircle.transform.localScale = new Vector3(m_trapPlaceRange * 2, 0.5f, m_trapPlaceRange * 2);
            m_trapRangeCircle.gameObject.SetActive(true);
            
        }
        else if(m_inventorySystem.IsCheckCurrentItem(m_hotberIndex, ItemUseType.Attack))
        {
            m_isUsingAttackItem = true;
            m_power = 0f;
            m_reticle.gameObject.SetActive(true);
        }
        else
        {
            ItemRecieveData data = CreateItemData(Forward, 0f, m_pullOffSet);

            m_inventorySystem.UsePressed(m_hotberIndex, data);

        }

    }

    private float m_power;

    private void OnUseItemHold()
    {
        //Debug.Log("Hold");

        if (m_isUsingArrow)
        {
            OnReticle();



            m_power += Time.deltaTime;

            m_power = Mathf.Min(m_power, 3f);

            var main = m_trailParticle.main;
            main.startSpeed = m_power * 8 / m_rb.mass;
        }
        else if (m_isUsingSetItem)
        {
            OnReticle();
            m_trapPreview.transform.position = m_trapSetPosition;
        }
        else if (m_isUsingAttackItem)
        {
            OnReticle();


            m_power += Time.deltaTime;

            m_power = Mathf.Min(m_power, 3f);
        }
    }

    private void OnUseItemRelease()
    {
        //Debug.Log("Release");

        if(m_isUsingArrow)
        {
            m_isUsingArrow = false;

            m_reticle.gameObject.SetActive(false);
            m_trailParticle.gameObject.SetActive(false);

            ItemRecieveData data = CreateItemData(m_arrowTemporaryForward, m_power * 8, m_pullOffSet);
            m_inventorySystem.UseRelease(m_hotberIndex, data);

        }
        else if(m_isUsingSetItem)
        {
            m_isUsingSetItem = false;

            m_reticle.gameObject.SetActive(false);

            m_trapPreview.SetActive(false);
            m_trapRangeCircle.gameObject.SetActive(false);

            ItemRecieveData data = CreateItemData(m_arrowTemporaryForward, 0f, m_setOffSet);
            m_inventorySystem.UseRelease(m_hotberIndex, data);
        }
        else if (m_isUsingAttackItem)
        {
            m_isUsingAttackItem = false;

            m_reticle.gameObject.SetActive(false);

            ItemRecieveData data = CreateItemData(m_arrowTemporaryForward, m_power);
            m_inventorySystem.UseRelease(m_hotberIndex, data);
        }
    }

    private void OnuseItemGet()
    {
        if (m_selecrItem == null) return;

        if(ReceiveItem(m_selecrItem.ItemData))
        {
            m_selecrItem.ItemGet();
            m_selecrItem = null;
        }
        else
        {
            m_textErrorMessage.SetActive(true);
            Invoke(nameof(CloseErrorMessage), 1f);
        }
    }

    private void CloseErrorMessage()
    {
        m_textErrorMessage.SetActive(false);
    }

    private void OnReticle()
    {
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

            //trap
            
            Vector3 offset = mousePos - transform.position;
            offset.y = 0f;

            if(offset.magnitude > m_trapPlaceRange) offset = offset.normalized * m_trapPlaceRange;

            m_trapSetPosition = transform.position + offset;
        }

    }

    //this method is to rotate playerDirObject(arrowDir) 
    private void OnRotatePlayerDirObject(Vector3 moveDir)
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

    public void SetCanInput(bool ignoreInput)
    {
        m_ignoreInput = ignoreInput;

        if (!ignoreInput)
        {
            m_input.OnInputClear();
        }
    }
    public virtual bool ReceiveItem(Item item)
    {
        Debug.Log("ReceiveItemäJén");

        if (item == null)
        {
            Debug.Log("itemÇ™null");
            return false;
        }

        if (m_inventorySystem == null)
        {
            Debug.Log("InventorySystemÇ™null");
            return false;
        }

        Debug.Log("AddItemÇåƒÇ—Ç‹Ç∑");

        bool success = m_inventorySystem.AddItem(item, 1);

        Debug.Log("AddItemèIóπ : " + success);

        return success;
    }
    private DropItem m_selecrItem;
    private Vector3 m_popupPosition;
    private void SearchItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, m_itemSerachLayer))
        {
            DropItem drop = hit.collider.GetComponentInParent<DropItem>();

            if (drop != null)
            {

                float distancee =Vector3.Distance(transform.position,drop.transform.position);
                
                if(distancee >m_itemSearchRange)
                {
                    m_selecrItem = null;
                    m_itemDescriptionText.text = "";
                    return;
                }
                if (m_selecrItem != drop)
                {
                    m_selecrItem = drop;
                    m_popupPosition = Input.mousePosition + new Vector3(20f, -20f, 0f);
                }

                m_textPanel.SetActive(true);

                m_nameText.text = drop.ItemData.ItemName;
                m_itemDescriptionText.rectTransform.position = m_popupPosition;
                m_itemDescriptionText.text = drop.ItemData.info;
                return;
            }
        }
        m_selecrItem = null;
        m_itemDescriptionText.text = "";
        m_textPanel.SetActive(false);
    }
}
