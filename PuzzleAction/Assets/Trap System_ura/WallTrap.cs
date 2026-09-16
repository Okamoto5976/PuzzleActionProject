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

    private Coroutine m_returnCoroutine;


    protected override void EntitySetUp()
    {
        
    }

    public override void TrapInit()
    {
        base.TrapInit();

        m_dir = transform.forward;

        StartCoroutine(SpawnDelay());
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
