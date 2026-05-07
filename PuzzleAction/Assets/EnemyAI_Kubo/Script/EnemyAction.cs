using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class EnemyAction : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private GameObject m_target; //攻撃対象

    [Header("Status")]
    [SerializeField] public int m_attackValue = 1; //enemyの攻撃力
    [SerializeField] private float m_moveSpeed = 1.0f; //enemyの移動速度

    [Header("Range")]
    [SerializeField] private float m_findRange = 8f;     //target索敵範囲
    [SerializeField] private float m_attackRange = 1.5f; //攻撃範囲

    [Header("Ground Check")]
    [SerializeField] private float m_groundCheckDistance = 2.0f;
    [SerializeField] private float m_rayOffset = 0.5f;


    private Rigidbody m_rb;
    [NonSerialized]public bool m_isFound = false;
    [NonSerialized]public bool m_isAttacking = false;

    public bool IsFaund => m_isFound;
    public bool IsAttacking => m_isAttacking;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (m_target == null)
        {
            Debug.LogError("targetが設定されていません");
            return;
        }

        float distance = Vector3.Distance(transform.position, m_target.transform.position); //Playerとenemyとの距離計算

        // 索敵範囲内に入ったら発見
        if (distance <= m_findRange)
        {
            m_isFound = true;
        }
        else
        {
            m_isFound = false;
            m_isAttacking = false;
            return;
        }

        // 攻撃範囲内なら攻撃
        if (distance <= m_attackRange)
        {
            m_isAttacking = true;
            Attack();
        }
        else
        {
            m_isAttacking = false;
            EnemyContoroller();
        }
    }

    // targetに向けて移動
    private void EnemyContoroller()
    {
        Vector3 toTarget = (m_target.transform.position - transform.position).normalized;

        //targetの方向に進めるか
        if(IsMove(toTarget))
        {
            MoveToTarget(toTarget);
            return;
        }

        Vector3[] directions =
        {
            Quaternion.Euler(0, -90, 0) * toTarget,
            Quaternion.Euler(0, 90, 0) * toTarget,
            -toTarget
        };
        foreach(var dir in directions) //directionsの中身を一つずつ見ていき、それをvar型のdirに格納する
        {
            if(IsMove(dir))
            {
                MoveToTarget(dir);
                return;
            }
        }

        //全方向進めないのなら移動停止
    }

    //進行方向にobjectが存在しているかどうか
    private bool IsMove(Vector3 direction)
    {
        Vector3 rayStart = transform.position + Vector3.up * 0.01f + direction * m_rayOffset;

        return Physics.Raycast(rayStart, Vector3.down, m_groundCheckDistance); 
    }

    //実際の移動
    private void MoveToTarget(Vector3 direction)
    {
        Vector3 move = direction * m_moveSpeed * Time.fixedDeltaTime;
        m_rb.MovePosition(m_rb.position + move);

        //targetに方向転換
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        m_rb.MoveRotation(lookRotation);
    }
    // 攻撃処理（今はフラグのみ）
    private void Attack()
    {
        // 攻撃処理記入欄
        // m_target.GetComponent<PlayerHP>().Damage(m_attackValue);

        Debug.Log($"攻撃中！ ダメージ：{m_attackValue}");
    }

    // デバッグ用：範囲可視化 
    // Scene viewのみ可視化可能
    private void OnDrawGizmosSelected()
    {
        //索敵範囲　可視化
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_findRange);

        //攻撃可能範囲　可視化
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attackRange);

        //Ray　可視化
        Gizmos.color = Color.green;
        Gizmos.DrawRay((transform.position + transform.forward) * m_rayOffset, //場所
                        Vector3.down * m_groundCheckDistance);                 //方向
    }
}
