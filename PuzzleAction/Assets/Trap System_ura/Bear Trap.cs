using UnityEngine;
using static EnemyData;

public class BearTrap : TrapBase
{
    [Header("Bear Trap")]
    [SerializeField] private Collider m_damageCollider;

    [Header("Damage")]
    [SerializeField] private float m_damage = 10.0f;

    [Header("Recovery")]
    [SerializeField] private float m_recoveryTime = 3.0f;

    private bool m_isActive;
    private bool m_isRecovering;

    // TrapArea用の初期化
    protected override void EntitySetUp()
    {
        m_isActive = true;
        m_isRecovering = false;

        if (m_damageCollider != null)
        {
            m_damageCollider.enabled = true;
        }
    }

    // Entityがトラばさみに触れた時
    protected override void OnTriggerEnter(Collider other)
    {
        if (!m_isActive || m_isRecovering)
        {
            return;
        }

        Entity target = other.GetComponent<Entity>();

        if (target == null)
        {
            return;
        }

        // DamageDataを作成
        m_damageData = new DamageData
        {
            Attack = m_damage,
            //AttackType = m_attackType,
            AttackDir =
                (target.transform.position - transform.position).normalized
        };

        // ダメージを与える
        target.TakeDamage(m_damageData);

        // 命中処理
        OnHit();

        // トラばさみを無効化
        m_isActive = false;
        m_isRecovering = true;

        if (m_damageCollider != null)
        {
            m_damageCollider.enabled = false;
        }

        // 一定時間後に復活
        Invoke(nameof(RecoverTrap), m_recoveryTime);
    }

    // 命中処理
    protected override void OnHit()
    {
        Debug.Log("トラばさみに引っかかった！");
    }

    // トラばさみ復活
    private void RecoverTrap()
    {
        m_isActive = true;
        m_isRecovering = false;

        if (m_damageCollider != null)
        {
            m_damageCollider.enabled = true;
        }

        Debug.Log("トラばさみが復活しました！");
    }

    // Poolから再利用された時などに初期状態へ戻す
    public override void TrapInit()
    {
        base.TrapInit();

        CancelInvoke(nameof(RecoverTrap));

        m_isActive = true;
        m_isRecovering = false;

        if (m_damageCollider != null)
        {
            m_damageCollider.enabled = true;
        }
    }
}