using UnityEngine;

public class T_Camera : MonoBehaviour
{
    [Header("Follow Target")]
    [SerializeField] private Transform m_target;

    [Header("Offset")]
    [SerializeField] private float m_distance;

    [Header("Follow")]
    [SerializeField] private float m_followSpeed = 0f; // 0 = ‘¦Žž’Ç]


    public float Distance
    {
        get => m_distance;
        set
        {
            m_distance = value;
            CalculateOffset();
        }
    }

    private Vector3 m_offset;
    private float m_angle;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        CalculateOffset();
    }

    private void CalculateOffset()
    {
        m_angle = transform.rotation.eulerAngles.x;
        m_offset = GetOffset(Mathf.Abs(m_distance), m_angle);
    }
    public void SetTarget(Transform target)
    {
        m_target = target;
    }

    private Vector3 GetOffset(float distanceToObject, float rotationFromHorizon)
    {
        float height = Mathf.Abs(distanceToObject) / Mathf.Tan((90 - rotationFromHorizon) * Mathf.Deg2Rad);
        return new(0, height, -distanceToObject);
    }

    private void LateUpdate()
    {
        DoCameraCorrection();
    }

    private void DoCameraCorrection()
    {
        if (m_target == null) return;

        if (transform.rotation.eulerAngles.x != m_angle)
        {
            CalculateOffset();
        }

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
}