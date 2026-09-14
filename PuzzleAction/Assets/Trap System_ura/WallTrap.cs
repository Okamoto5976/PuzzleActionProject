using System.Collections;
using UnityEngine;

public class WallTrap : TrapBase
{
    [Header("Rock")]
    [SerializeField] private float m_spawnDistance = 3.0f;
    [SerializeField] private float m_lifeTime = 5.0f;

    [Header("Spawn")]
    [SerializeField] private float m_spawnDelay = 0.0f;

    [Header("KnockBack")]
    [SerializeField] private float m_knockBackPower = 5.0f;

    private Coroutine m_returnCoroutine;


    protected override void EntitySetUp()
    {
        // 継承先でDamageDataを設定
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

        SpawnRock();
    }


    // TrapArea用
    public override void TrapInit()
    {
        base.TrapInit();

        // TrapAreaではownerがいないので
        // Entityの情報は使用しない
        m_damageData = new DamageData
        {
            Attack = m_str,
            AttackType = m_attackType,

            Knockback = 0,
            StunDuration = 0,

            AttackDir = transform.forward
        };

        // TrapAreaから呼ばれた場合
        SpawnRock();
    }


    private void SpawnRock()
    {
        if (m_spawnDelay > 0.0f)
        {
            StartCoroutine(SpawnDelay());
        }
        else
        {
            Spawn();
        }
    }


    private IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(m_spawnDelay);

        Spawn();
    }


    private void Spawn()
    {
        // Trapの前方へ岩を移動
        transform.position +=
            m_dir * m_spawnDistance;


        // 発生した岩を前方へノックバック
        OnAddForce(
            m_dir,
            m_knockBackPower
        );


        // 一定時間後にPoolへ返す
        m_returnCoroutine =
            StartCoroutine(ReturnAfterTime());
    }


    private IEnumerator ReturnAfterTime()
    {
        yield return new WaitForSeconds(m_lifeTime);

        OnReturnPool();
    }


    protected override void OnHit()
    {
        // 岩が何かに当たったときの処理
    }
}
