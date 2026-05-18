using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class Enemy_Rush : MonoBehaviour
{
    [SerializeField] private float rushSpeed = 10f;

    private Rigidbody m_rb;
    private LineRenderer m_line;

    private Vector3 m_targetPosition;
    private Vector3 m_direction;

    private bool m_isRushingStart = false;
    private bool m_isPreparing = false;


    public bool IsRunningStart => m_isRushingStart;
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_line = GetComponent<LineRenderer>();

        m_line.enabled = false;

    }

    // 突進開始準備
    public void ReadyRush()
    {
        m_isPreparing = true;
        m_line.enabled = true;
    }

    public void UpdatePreparation(Vector3 playerPos)
    {
        if (!m_isPreparing) return;

        m_targetPosition = playerPos;
        m_direction = (m_targetPosition - transform.position).normalized;

        DrawPredictionLine();
    }


    // 予測線
    private void DrawPredictionLine()
    {
        //m_line.enabled = true;

        m_line.positionCount = 2;
        m_line.SetPosition(0, transform.position);
        m_line.SetPosition(1, m_targetPosition);
    }

    // 突進開始
    public void StartRush()
    {
        m_isPreparing = false;
        m_isRushingStart = true;
        m_line.enabled = false;
    }

    private void FixedUpdate()
    {
        if (!m_isRushingStart) return;

        //次の移動予定位置
        Vector3 nextPos = m_rb.position + m_direction * rushSpeed * Time.fixedDeltaTime;

        ////NavMesh上にあるかどうか
        //if (!NavMesh.SamplePosition(nextPos, out NavMeshHit hit, 0.5f, NavMesh.AllAreas) || !NavMesh.CalculatePath(transform.position, nextPos, NavMesh.AllAreas, new NavMeshPath()))
        //{
        //    StopRush();
        //    return;
        //}

        //bool samplePos = NavMesh.SamplePosition(nextPos, out NavMeshHit hit, 0.5f, NavMesh.AllAreas);
        bool path = NavMesh.CalculatePath(transform.position, nextPos, NavMesh.AllAreas, new NavMeshPath());

        ////NavMesh上にあるかどうか
        //if (!path)
        //{
        //    Debug.Log("Path");
        //    StopRush();
        //    return;
        //}

        //何も問題なければ移動開始
        m_rb.MovePosition(nextPos);

        // 到達で終了
        if (Vector3.Distance(transform.position, m_targetPosition) < 0.5f)
        {
            m_isRushingStart = false;
        }
    }

    public void StopRush()
    {
        m_isRushingStart = false;
        m_line.enabled = false;
    }
    
}