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
        // Œp³æ‚ÅDamageData‚ðÝ’è
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


     
    public override void TrapInit()
    {
        base.TrapInit();
 
        m_damageData = new DamageData
        {
            Attack = m_str,
            AttackType = m_attackType,

            Knockback = 0,
            StunDuration = 0,

            AttackDir = transform.forward
        };

         
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
         
        transform.position +=
            m_dir * m_spawnDistance;
        
        OnAddForce(
            m_dir,
            m_knockBackPower
        );
        
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
         
    }
}
