using System.Collections;
using UnityEngine;

public class WallTrap : TrapBase
{
    [Header("Rock")]
    [SerializeField] private float m_spawnDistance = 3.0f;
    [SerializeField] private float m_lifeTime = 5.0f;

    [Header("Spawn")]
    [SerializeField] private float m_spawnDelay = 1.0f;

    [Header("KnockBack")]
    [SerializeField] private float m_knockBackPower = 5.0f;


    protected override void EntitySetUp()
    {
        
    }


    public override void TrapInit()
    {
        base.TrapInit();

        m_dir = transform.forward;

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
        m_rb.linearVelocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;

        transform.position +=
            m_dir * m_spawnDistance;
        OnAddForce(
            m_dir,
            m_knockBackPower
        );

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