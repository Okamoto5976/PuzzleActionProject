using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class Enemy_Rush : MonoBehaviour
{
    [SerializeField] private float rushSpeed = 10f;

    private Rigidbody m_rb;
    private LineRenderer m_line;

    private Vector3 m_targetPosition;
    private Vector3 m_direction;

    private bool m_isRushing = false;


    public bool IsRunning => m_isRushing;
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_line = GetComponent<LineRenderer>();

        m_line.enabled = false;

    }

    // 突進開始準備
    public void ReadyRush(Vector3 playerPos)
    {
        m_targetPosition = playerPos;
        m_direction = (m_targetPosition - transform.position).normalized;

        DrawPredictionLine();
    }

    // 予測線
    private void DrawPredictionLine()
    {
        m_line.enabled = true;

        m_line.positionCount = 2;
        m_line.SetPosition(0, transform.position);
        m_line.SetPosition(1, m_targetPosition);
    }

    // 突進開始
    public void StartRush()
    {
        m_isRushing = true;
        m_line.enabled = false;
    }

    private void FixedUpdate()
    {
        if (!m_isRushing) return;

        m_rb.MovePosition(m_rb.position + m_direction * rushSpeed * Time.fixedDeltaTime);

        // 到達で終了
        if (Vector3.Distance(transform.position, m_targetPosition) < 0.5f)
        {
            m_isRushing = false;
        }
    }

    public void StopRush()
    {
        m_isRushing = false;
        m_line.enabled = false;
    }
}