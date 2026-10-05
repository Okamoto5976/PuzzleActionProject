using UnityEngine;
using UnityEngine.InputSystem;

public class InputProvider
{
    private InputSystem_Actions m_action;

    private Vector2 m_move;
    private bool m_active;
    private bool m_activeRelease;

    private bool m_isEvasion;
    private bool m_isHotber1;
    private bool m_isHotber2;
    private bool m_isHotber3;
    private bool m_isCancel;
    private bool m_isInteract;
    private bool m_isGetDropItem;

    public InputProvider()//newÇ≥ÇÍÇΩÇ∆Ç´èâä˙âª
    {
        m_action = new InputSystem_Actions();

        m_action.Player.Move.performed += OnMove;
        m_action.Player.Move.canceled += OnMove;
        m_action.Player.Attack.canceled += OnActiveRelease;

        m_action.Player.Attack.performed += OnActive;
        m_action.Player.Sprint.performed += OnEvasion;
        m_action.Player.Hotber1.performed += OnHotber1;
        m_action.Player.Hotber2.performed += OnHotber2;
        m_action.Player.Hotber3.performed += OnHotber3;
        m_action.Player.Cancel.performed += OnCancel;
        m_action.Player.Interact.performed += OnInteract;
        m_action.Player.GetDropItem.performed += OnGetDropItem;
        m_action.Enable();
    }

    public void Enable()
    {

    }

    public void Disable()
    {
        m_action.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        m_move = context.ReadValue<Vector2>();
    }

    private void OnActive(InputAction.CallbackContext context)
    {
        m_active = true;
    }

    private void OnActiveRelease(InputAction.CallbackContext context)
    {
        m_activeRelease = true;
    }

    private void OnEvasion(InputAction.CallbackContext context)
    {
        m_isEvasion = true;
    }

    private void OnHotber1(InputAction.CallbackContext context)
    {
        m_isHotber1 = true;
    }

    private void OnHotber2(InputAction.CallbackContext context)
    {
        m_isHotber2= true;
    }

    private void OnHotber3(InputAction.CallbackContext context)
    {
        m_isHotber3 = true;
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        m_isCancel = true;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        m_isInteract = true;
    }


    public void OnInputClear()
    {
        //Debug.LogWarning("InputClear");
        m_active = false;
        m_activeRelease = false;
        m_isEvasion = false;
        m_isHotber1 = false;
        m_isHotber2 = false;
        m_isHotber3 = false;
        m_isCancel = false;
        m_isInteract = false;
    }

    private void OnGetDropItem(InputAction.CallbackContext context)
    {
        Debug.Log("OnGetDropItem");
        m_isGetDropItem = true;
    }

    public Vector2 Move
    {
        get
        {
            return m_move;
        }
    }

    public bool IsActive
    {
        get
        {
            bool result = m_active;
            m_active = false;

            return result;
        }
    }

    public bool IsEvasion
    {
        get
        {
            bool result = m_isEvasion;
            m_isEvasion = false;

            return result;
        }
    }

    public bool IsHotber1
    {
        get
        {
            bool result = m_isHotber1;
            m_isHotber1 = false;

            return result;
        }
    }

    public bool IsHotber2
    {
        get
        {
            bool result = m_isHotber2;
            m_isHotber2 = false;

            return result;
        }
    }

    public bool IsHotber3
    {
        get
        {
            bool result = m_isHotber3;
            m_isHotber3 = false;

            return result;
        }
    }

    public bool IsCancel
    {
        get
        {
            bool result = m_isCancel;
            m_isCancel = false;

            return result;
        }
    }

    public bool IsActiveHold
    {
        get
        {
            return m_action.Player.Attack.IsPressed();
        }
    }

    public bool IsActiveRelease
    {
        get
        {
            bool result = m_activeRelease;
            m_activeRelease = false;
            return result;
        }
    }

    public bool IsInteract
    {
        get
        {
            bool result = m_isInteract;
            m_isInteract = false;

            return result;
        }
    }

    public bool IsGetDropItem
    {
        get
        {
            bool result = m_isGetDropItem;
            m_isGetDropItem = false;

            return result;
        }
    }

}
