using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 5f;

    private Rigidbody m_rb;
    private PlayerInputActions m_inputActions;
    private Vector2 m_moveInput;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_inputActions = new PlayerInputActions();

        m_inputActions.Player.Move.performed += ctx => m_moveInput = ctx.ReadValue<Vector2>();
        m_inputActions.Player.Move.canceled += ctx => m_moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        m_inputActions.Enable();
    }

    private void OnDisable()
    {
        m_inputActions.Disable();
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(m_moveInput.x, 0f, m_moveInput.y) * m_moveSpeed * Time.fixedDeltaTime;

        Vector3 nextPos = m_rb.position + move;

        if (IsOnPlane(nextPos))
        {
            m_rb.MovePosition(nextPos);
            return;
        }

        Vector3 nextPosX = m_rb.position + new Vector3(move.x, 0f, 0f);
        if (IsOnPlane(nextPosX))
        {
            m_rb.MovePosition(nextPosX);
            return;
        }

        Vector3 nextPosZ = m_rb.position + new Vector3(0f, 0f, move.z);
        if (IsOnPlane(nextPosZ))
        {
            m_rb.MovePosition(nextPosZ);
            return;
        }

    }

    private bool IsOnPlane(Vector3 position)
    {
        Ray ray = new Ray(position + Vector3.up * 0.1f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 1.1f))
        {
            return hit.collider.tag == "plane";
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * 1.1f);
    }
}