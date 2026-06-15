using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Enemy_Rush : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController m_controller;

    private Vector3 m_dir;
    private Vector3 m_targetPos;

    private bool m_isRunning;
    private bool m_preparing;
    
    private LineRenderer m_line;

    public Vector3 CurrentDirection => m_dir;
    public bool IsRunning => m_isRunning;

    private void Awake()
    {
        m_line = GetComponent<LineRenderer>();  
        m_line.enabled = false;
    }

    public void Initialized(EnemyController controller)
    {
        m_controller = controller;
    }

    public void Execute()
    {
        if (m_isRunning)
        {
            m_controller.Move(m_dir, m_controller.DashSpeed);

            if (Vector3.Distance(transform.position, m_targetPos) < 0.5f)
            {
                Stop();
            }
            return;
        }

        if (m_preparing)
        {
            DrawLine();
            return;
        }

        StartCoroutine(Rush());
    }


    IEnumerator Rush()
    {
        m_preparing = true;

        m_targetPos = m_controller.Target.position;

        m_dir = (m_targetPos - transform.position).normalized;
        m_dir.y = 0;

        m_line.enabled = true;

        yield return new WaitForSeconds(5f);

        m_preparing = false;
        m_isRunning = true;

        m_line.enabled = false;

        yield return new WaitForSeconds(10f);
    }
    private void DrawLine()
    {
        if (m_controller.Target == null) return;

        m_line.positionCount = 2;

        m_line.SetPosition(0, transform.position);

        m_line.SetPosition(1, m_targetPos);
    }

    public void Stop()
    {
        m_isRunning = false;
        m_preparing = false;

        m_line.enabled = false;
    }

}
