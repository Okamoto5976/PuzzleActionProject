using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class Enemy_Rush : MonoBehaviour
{
    private EnemyContllor m_controller;

    private LineRenderer m_line;

    private Vector3 m_targetPos;
    private Vector3 m_dir;

    private bool m_isRunning;
    private bool m_isPreparing;

    private Vector3 m_fixedDir;

    public bool IsRunning => m_isRunning;

    public void Initialized(EnemyContllor controller)
    {
        m_controller = controller;

        m_line = GetComponent<LineRenderer>();
        m_line.enabled = false;
    }

    // 準備
    public void Ready()
    {
        m_isPreparing = true;
        m_line.enabled = true;
    }

    public void UpdatePrepare(Vector3 playerPos)
    {
        if (!m_isPreparing) return;

        if (NavMesh.SamplePosition(playerPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            m_targetPos = hit.position;
        else
            m_targetPos = playerPos;

        m_dir = (m_targetPos - transform.position).normalized;

        //予測線表示
        m_line.positionCount = 2;
        m_line.SetPosition(0, transform.position);
        m_line.SetPosition(1, m_targetPos);
    }

    // 突進開始
    public void StartRush()
    {
        m_isPreparing = false;
        m_isRunning = true;

        m_fixedDir = m_dir;
        m_line.enabled = false;
    }

    public Vector3 GetDirection()
    {
        if (!m_isRunning) return Vector3.zero;

        if (Vector3.Distance(transform.position, m_targetPos) < 0.5f)
        {
            Stop();
            return Vector3.zero;
        }

        Vector3 next = transform.position + m_dir * 0.5f;

        if (!NavMesh.SamplePosition(next, out _, 0.5f, NavMesh.AllAreas))
        {
            Stop();
            return Vector3.zero;
        }

        return m_fixedDir;
    }

    public void Stop()
    {
        m_isRunning = false;
        m_isPreparing = false;
        m_line.enabled = false;
    }
}