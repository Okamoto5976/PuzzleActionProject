using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class T_Camera : MonoBehaviour
{
    [Header("Follow Target")]
    [SerializeField] private Transform m_target;

    [Header("Offset")]
    [SerializeField] private float m_distance;

    [Header("Follow")]
    [SerializeField] private float m_followSpeed = 0f; // 0 = ‘¦Žž’Ç]

    [SerializeField] private bool m_isShaking = false;
    [SerializeField] private float m_shakeStrength = 0.1f;
    [SerializeField] private float m_shakeSpeed = 0.05f;
    private Vector3 m_shakeOffset = Vector3.zero;

    private Coroutine m_shakeCoroutine;

    public bool IsShaking
    {
        get => m_isShaking;
        set => m_isShaking = value;
    }

    public float ShakeStrength
    {
        get => m_shakeStrength;
        set => m_shakeStrength = value;
    }

    public float ShakeSpeed
    {
        get => m_shakeSpeed;
        set => m_shakeSpeed = value;
    }

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

    private void OnEnable()
    {
        StartShake();
    }

    private void StartShake()
    {
        if (m_shakeCoroutine != null)
        {
            StopCoroutine(m_shakeCoroutine);
        }
        StartCoroutine(DoShake());
    }

    private IEnumerator DoShake()
    {
        while (true)
        {
            m_shakeOffset = Random.onUnitCircle * m_shakeStrength;
            yield return new WaitForSeconds(m_shakeSpeed);
        }
    }

    private void OnDisable()
    {
        if (m_shakeCoroutine != null)
        {
            StopCoroutine(m_shakeCoroutine);
        }
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

#if UNITY_EDITOR
    private void OnValidate()
    {
        CalculateOffset();
    }
#endif

    private void DoCameraCorrection()
    {
        if (m_target == null) return;

        if (transform.rotation.eulerAngles.x != m_angle)
        {
            CalculateOffset();
        }

        Vector3 targetPos = m_target.position + m_offset;
        if (m_isShaking)
        {
            targetPos += m_shakeOffset;
        }

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