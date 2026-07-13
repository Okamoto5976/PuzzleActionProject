using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(LineRenderer))]
public class Enemy_Rush : MonoBehaviour, IEnemyBehaviour
{
    private EnemyController m_controller;

    private Vector3 m_dir;
    private Vector3 m_targetPos;

    private bool m_isRunning;
    private bool m_preparing;
    private bool m_hasHit;

    private LineRenderer m_line;

    private Coroutine m_rushCoroutine;

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
        if (m_controller.Target == null) return;

        if (m_isRunning)
        {
            m_controller.Move(m_dir, m_controller.EvasionSpeed);

            Vector3 toTarget =m_targetPos -transform.position;

            if (Vector3.Dot(toTarget, m_dir) <= 0f)
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

        if (m_rushCoroutine == null)
        {
            m_rushCoroutine = StartCoroutine(Rush());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_isRunning) return;

        if (m_hasHit) return;

        Entity entity = other.GetComponentInParent<Entity>();

        if (entity.Team == m_controller.Team) 
            return;

        DamageData damage =
            new DamageData
            {
                Attack = (int)m_controller.STR,
                HitRate = 100f,
                CriticalRate = m_controller.CriticalRate,
                CriticalDamage = m_controller.CriticalDamage,
                BreakRate = m_controller.BreakRate,
                Knockback = m_controller.KnockBack,
                Stun = m_controller.Stun,
                AttackDir = m_dir,
                Attacker = m_controller,
                AttackerSE = m_controller.AttackSE,
                AudioSource = m_controller.AudioSource
            };

        entity.TakeDamage(damage);

        m_hasHit = true;

        Stop();
    }

    IEnumerator Rush()
    {
        m_preparing = true;

        m_targetPos = m_controller.Target.Value;

        m_dir = (m_targetPos - transform.position).normalized;

        m_dir.y = 0f;

        m_line.enabled = true;

        yield return new WaitForSeconds(5f);

        m_preparing = false;
        m_isRunning = true;
        m_hasHit = false;

        m_line.enabled = false;

        yield return new WaitForSeconds(10f);

        m_rushCoroutine = null;
    }

    private void DrawLine()
    {
        m_line.positionCount = 2;

        m_line.SetPosition(0, transform.position);

        m_line.SetPosition(1, m_targetPos);
    }

    public void Stop()
    {
        if (m_rushCoroutine != null)
        {
            StopCoroutine(m_rushCoroutine);
            m_rushCoroutine = null;
        }

        m_isRunning = false;
        m_preparing = false;

        m_line.enabled = false;
    }
}