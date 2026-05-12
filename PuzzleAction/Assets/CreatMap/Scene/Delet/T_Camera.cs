using UnityEngine;

public class T_Camera : MonoBehaviour
{
    [Header("Follow Target")]
    [SerializeField] private Transform m_target;

    [Header("Offset")]
    [SerializeField] private Vector3 m_offset = new Vector3(0f, 8f, -8f);

    [Header("Follow")]
    [SerializeField] private float m_followSpeed = 0f; // 0 = ë¶éûí«è]

    private void LateUpdate()
    {
        if (m_target == null) return;

        Vector3 targetPos = m_target.position + m_offset;

        if (m_followSpeed <= 0f)
        {
            transform.position = targetPos;
        }
        else
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetPos,
                Time.deltaTime * m_followSpeed
            );
        }
    }

    public void SetTarget(Transform target)
    {
        m_target = target;
    }
}