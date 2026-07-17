using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Enemy_Rush : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController m_controller;

    private enum Enum_RushState
    {
        Prepare,
        Rush
    }
    private Enum_RushState m_state;

    private float m_prepareTime = 1f;

    private Vector3 m_targetPos;
    private Vector3 m_dir;

    private bool m_hasHit;

    private LineRenderer m_lineRenderer;
    private Coroutine m_prepareCoroutine;

    public bool IsRunning => m_state == Enum_RushState.Rush;
    public Vector3 CurrentDirection => m_dir;

    private void Awake()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        m_lineRenderer.enabled = false;
    }

    public void Initialized(EnemyController controller)
    {
        m_controller = controller;
        StartPrepare();
    }
    public void Execute()
    {

        if (m_controller.Target == null)
        {
            m_lineRenderer.enabled = false;
            return;
        }

        float distance = Vector3.Distance(transform.position, m_controller.Target.Value);
        if (distance > m_controller.FindRange)
        {
            m_lineRenderer.enabled = false;
            return;
        }
        switch (m_state)
        {
            case Enum_RushState.Prepare:
                UpdatePrepare();
                break;

            case Enum_RushState.Rush:
                UpdateRush();
                break;
        }
    }

    // ====================Prepare
    private void UpdatePrepare()
    {
        Vector3 dir = m_controller.Target.Value - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
        DrawLine();
    }

    private IEnumerator PrepareRush()
    {
        if (m_controller.Target == null)
        {
            m_prepareCoroutine = null;
            yield break;
        }

        m_state = Enum_RushState.Prepare;

        m_hasHit = false;

        m_lineRenderer.enabled = true;

        float timer = 0f;

        while (timer < m_prepareTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        m_targetPos = m_controller.Target.Value;
        m_dir = m_targetPos - transform.position;
        m_dir.y = 0f;
        m_dir.Normalize();

        transform.rotation = Quaternion.LookRotation(m_dir);
        m_lineRenderer.enabled = false;
        m_state = Enum_RushState.Rush;
        m_prepareCoroutine = null;
    }

    // ==================Rush
    private void UpdateRush()
    {
        transform.rotation = Quaternion.LookRotation(m_dir);

        m_controller.Move(m_dir, m_controller.EvasionSpeed);

        float distanceToPlayer = Vector3.Distance(transform.position, m_controller.Target.Value);
        if (distanceToPlayer <= m_controller.AttackRange)
        {
            if (!m_hasHit)
            {
                m_controller.TryAttack();
                m_hasHit = true;
                Stop();
                return;
            }
        }

        Vector3 toTarget = m_targetPos - transform.position;
        if (Vector3.Dot(toTarget, m_dir) <= 0f)
        {
            Stop();
        }
    }
    // ====================Visual
    private void DrawLine()
    {
        m_lineRenderer.positionCount = 2;
        m_lineRenderer.useWorldSpace = true;

        m_lineRenderer.SetPosition(0, transform.position + Vector3.up * 0.5f);

        m_lineRenderer.SetPosition(1, m_controller.Target.Value + Vector3.up * 0.5f);
    }
    private void StartPrepare()
    {
        if (m_prepareCoroutine != null) return;

        m_prepareCoroutine = StartCoroutine(PrepareRush());
    }
    public void Stop()
    {
        if (m_prepareCoroutine != null)
        {
            StopCoroutine(m_prepareCoroutine);
            m_prepareCoroutine = null;
        }

        m_lineRenderer.enabled = false;
        m_controller.Stop();
        m_state = Enum_RushState.Prepare;
        StartPrepare();
    }
}