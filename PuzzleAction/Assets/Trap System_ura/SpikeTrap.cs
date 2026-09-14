using System.Collections;
using UnityEngine;

public class SpikeTrap : TrapBase
{
    [Header("Trap")]
    [SerializeField]
    private float m_cooldown = 2.0f;

    // 発動可能か
    private bool m_isActive = true;


    protected override void EntitySetUp()
    {
        // ダメージデータを作成
        m_damageData = new DamageData
        {
            Attack = m_str + m_owner.STR,
            AttackType = m_attackType,

            CriticalRate = m_owner.CriticalRate,
            CriticalDamage = m_owner.CriticalDamage,
            BreakRate = m_owner.BreakRate,

            Knockback = m_owner.KnockBack,
            StunDuration = m_owner.Stun,

            AttackDir = m_dir
        };

        // 発動可能にする
        m_isActive = true;
    }


    protected override void OnTriggerEnter(Collider other)
    {
        // クールダウン中なら無視
        if (!m_isActive)
        {
            return;
        }

        // Entityを取得
        Entity entity = other.GetComponent<Entity>();

        if (entity == null)
        {
            return;
        }

        // 同じチームなら無視
        if (entity.Team == Team)
        {
            return;
        }

        // ダメージを与える
        entity.TakeDamage(m_damageData);

        // 発動処理
        OnHit();

        // 一時的に機能停止
        m_isActive = false;

        // クールダウン開始
        StartCoroutine(Cooldown());
    }


    private IEnumerator Cooldown()
    {
        // 指定時間待つ
        yield return new WaitForSeconds(m_cooldown);

        // 再び発動可能
        m_isActive = true;
    }


    protected override void OnHit()
    {
        // 針が飛び出すアニメーションやSEなど
        // 必要になったらここに追加
    }
}

