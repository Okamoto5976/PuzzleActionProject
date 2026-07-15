using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    [SerializeField]
    private float m_moveSpeed = 5f;

    [Header("Direction Marker")]
    [SerializeField]
    private Transform m_arrow;

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(h, 0f, v);

        if (moveDir.sqrMagnitude > 0f)
        {
            moveDir.Normalize();

            transform.position +=
                moveDir * m_moveSpeed * Time.deltaTime;

            m_arrow.rotation =
                Quaternion.LookRotation(moveDir) *
                Quaternion.Euler(90f, 0f, 0f);
        }
    }
}