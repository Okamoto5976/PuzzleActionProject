using UnityEngine;

[RequireComponent(typeof(ReturnObjectToPool))]
public class Bananapeel : TrapBase
{
    protected override void EntitySetUp()
    {
    }
    public void FixedUpdate()
    {
        CheckDeadLine();
    }

    protected override void OnHit()
    {
        OnReturnPool();
    }
    protected override void OnTriggerEnter(Collider other)
    {

        Entity victim =other.GetComponentInParent<Entity>();

        if (victim!=null)
        {
            if (victim.Team == TeamType.Nature) return;
            if (victim.Team == m_team) return;

            CreateDamageData();
            victim.ApplyStun(m_damageData.StunDuration);

            OnHit();
        }
    }
}
