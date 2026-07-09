using UnityEngine;
using UnityEngine.InputSystem;

public class InputProvider
{
    private InputSystem_Actions m_action;

    private Vector2 m_move;
    private bool m_active;

    private bool m_isEvasion;
    private bool m_isPrevious;
    private bool m_isNext;

    public InputProvider()//newÇ≥ÇÍÇΩÇ∆Ç´èâä˙âª
    {
        m_action = new InputSystem_Actions();

        m_action.Player.Move.performed += OnMove;
        m_action.Player.Move.canceled += OnMove;

        m_action.Player.Attack.performed += OnActive;
        m_action.Player.Sprint.performed += OnEvasion;
        m_action.Player.Previous.performed += OnPrevious;
        m_action.Player.Next.performed += OnNext;
        //m_action.Player.ActionLow.performed += OnSpecialLow;
        //m_action.Player.ActionMiddle.performed += OnSpecialMiddle;
        //m_action.Player.ActionHigh.performed += OnSpecialHigh;

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

    private void OnEvasion(InputAction.CallbackContext context)
    {
        m_isEvasion = true;
    }

    private void OnPrevious(InputAction.CallbackContext context)
    {
        m_isPrevious = true;
    }

    private void OnNext(InputAction.CallbackContext context)
    {
        m_isNext= true;
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

    public bool IsPrevious
    {
        get
        {
            bool result = m_isPrevious;
            m_isPrevious = false;

            return result;
        }
    }

    public bool IsNext
    {
        get
        {
            bool result = m_isNext;
            m_isNext = false;

            return result;
        }
    }
}
