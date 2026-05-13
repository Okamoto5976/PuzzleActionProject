using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    public enum EnemyType
    {
        Chase,
        Rush
    }

    [Header("Type")]
    [SerializeField] private EnemyType m_type;

    [Header("Target")]
    [SerializeField] private GameObject m_target;

    [Header("Range")]
    [SerializeField] private float m_findRange = 8f;
    [SerializeField] private float m_attackRange = 1.5f;

    private Enemy_Chase chase;
    private Enemy_Rush rush;

    private void Start()
    {
        if (m_type == EnemyType.Chase)
        {
            chase = GetComponent<Enemy_Chase>();
            chase.Initialized(m_target);
        }
        else
        {
            rush = GetComponent<Enemy_Rush>();
        }
    }

    private void Update()
    {
        if (m_target == null) return;

        float distance =
            Vector3.Distance(transform.position, m_target.transform.position);

        // õ“G”ÍˆÍŠO
        if (distance > m_findRange)
        {
            StopAll();
            return;
        }

        // UŒ‚”ÍˆÍ“à
        if (distance <= m_attackRange)
        {
            StopAll();
            Attack();
            return;
        }

        // í—Ş‚²‚Æ‚Ìˆ—
        if (m_type == EnemyType.Chase)
        {
            chase.Move();
        }
        else
        {
            RushLogic();
        }
    }

    // =========================
    // “Ëiˆ—i®—”Åj
    // =========================
    private void RushLogic()
    {
        // ‚Ü‚¾“®‚¢‚Ä‚È‚¢‚È‚ç€”õ ¨ “Ëi
        if (!rush.IsRunning)
        {
            rush.ReadyRush(m_target.transform.position);

            // ­‚µ—­‚ß‚Ä‚©‚ç“Ëii1•bj
            Invoke(nameof(StartRush), 1.0f);
        }
    }

    private void StartRush()
    {
        rush.StartRush();
    }

    private void StopAll()
    {
        if (chase != null) chase.Stop();
        if (rush != null) rush.StopRush();

        CancelInvoke(); // Invoke‚Ì–\‘––h~
    }

    private void Attack()
    {
        // UŒ‚ˆ—
        Debug.Log("H I T");
    }

    // Debug•\¦
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_findRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attackRange);
    }
}