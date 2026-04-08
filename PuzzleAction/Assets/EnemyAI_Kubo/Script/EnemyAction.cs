using System;
using UnityEngine;


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

    private Rigidbody m_rb;
    [NonSerialized]public bool m_isFound = false;
    private bool m_isAttacking = false;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        if (m_rb == null)
        {
            Debug.LogError("RigidbodyがEnemyにアタッチされていません");
            return;
        }
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
            MoveToTarget();
        }
    }

    // targetに向けて移動
    private void MoveToTarget()
    {
        Vector3 direction =　(m_target.transform.position - transform.position).normalized;

        Vector3 move =　direction * m_moveSpeed * Time.fixedDeltaTime;

        m_rb.MovePosition(m_rb.position + move);

        // targetに対して方向転換する
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            m_rb.MoveRotation(lookRotation);
        }
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_findRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attackRange);
    }
}
