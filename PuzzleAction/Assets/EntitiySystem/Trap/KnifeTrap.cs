using UnityEngine;

public class KnifeTrap : TrapBase
{
    [SerializeField] private LayerMask m_hitLayers;

    private void FixedUpdate()
    {
        OnMove(m_dir);
    }

    protected override void EntitySetUp()
    {
        
    }

    protected override void OnHit()
    {
        OnReturnPool();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if ((m_hitLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            OnHit();
            return;
        }

        Entity target =
            other.GetComponentInParent<Entity>();

        if (target == null)
            return;

        if (target.Team ==
            TeamType.Nature)
            return;

        if (target.Team == m_team) return;

        target.TakeDamage(m_damageData);

        
        OnHit();
    }
}
