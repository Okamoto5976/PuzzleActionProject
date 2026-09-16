using System.Collections;
using UnityEngine;

public class WallTrap : TrapBase
{
    [Header("Rock")]
    [SerializeField] private float m_spawnDistance = 3.0f;
    [SerializeField] private float m_lifeTime = 5.0f;


    protected override void EntitySetUp()
    {
        
    }
    
    public override void TrapInit()
    {
        base.TrapInit();

        m_dir = transform.forward;

        Spawn();
    }

    private void Spawn()
    {
        transform.position +=
            m_dir * m_spawnDistance;

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