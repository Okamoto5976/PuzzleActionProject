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
    [SerializeField] private PlayerData m_target;

    [Header("Range")]
    [SerializeField] private float m_findRange = 8f;
    [SerializeField] private float m_attackRange = 1.5f;

    private Enemy_Chase chase;
    private Enemy_Rush rush;

    private bool m_isPreparing = false;

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
            Vector3.Distance(transform.position, m_target.PlayerPostition);

        //直線移動中はtargetがFindRangeから出てもすぐにはやめない
        if(m_type == EnemyType.Rush && rush.IsRunningStart || m_isPreparing)
        {
            return;
        }
        // 索敵範囲外
        if (distance > m_findRange)
        {
            StopAll();
            return;
        }

        // 攻撃範囲内
        if (distance <= m_attackRange)
        {
            StopAll();
            Attack();
            return;
        }

        // 種類ごとの処理
        if (m_type == EnemyType.Chase)
        {
            chase.Move();
        }
        else
        {
            RushLogic();
        }
    }

    /// <summary>
    /// 突進処理
    /// </summary>
    private void RushLogic()
    {
        //準備開始
        if (!rush.IsRunningStart && !m_isPreparing)
        {
            m_isPreparing = true;

            //この時点の位置を固定
            rush.ReadyRush();

            Invoke(nameof(StartRush), Random.Range(0.5f, 1.0f));
        }

        //準備中は毎フレームtargetを補足する
        if(m_isPreparing)
        {
            rush.UpdatePreparation(m_target.PlayerPostition);
        }
    }

    private void StartRush()
    {
        rush.StartRush();

        //準備終了
        m_isPreparing = false;
    }

    private void StopAll()
    {
        if (chase != null) chase.Stop();
        if (rush != null) rush.StopRush();

        CancelInvoke(); // Invokeが何回も呼ばれないように停止

        m_isPreparing = false; //準備終了
    }

    private void Attack()
    {
        // 攻撃処理
        Debug.Log("H I T");
    }

    // Debug表示
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, m_findRange);

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, m_attackRange);
    //}
}