using UnityEngine;

[RequireComponent(typeof(ReturnObjectToPool))]
public class Bananapeel : TrapBase
{
    protected override void EntitySetUp()
    {

    }

    private void Update()
    {
        CheckDeadLine();
    }

    protected override void OnHit()
    {
        OnReturnPool();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Entity target =other.GetComponentInParent<Entity>();

        if (target != null)
        {
            if (target.Team == TeamType.Nature) return;
            if (target.Team == m_team) return;

            target.TakeDamage(m_damageData);

            OnHit();
        }
    }
}
