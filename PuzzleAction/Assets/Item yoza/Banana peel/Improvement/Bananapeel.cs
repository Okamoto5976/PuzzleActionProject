using UnityEngine;

[RequireComponent(typeof(ReturnObjectToPool))]
public class Bananapeel : TrapBase
{
    [Header("Bannan Peel Settings")]
    [SerializeField] private float m_stunDuration = 2.0f;

    private float m_appliedStunDuration = 2.0f;

    protected override void EntitySetUp()
    {
        float owanerStun =(m_owner!=null)?m_owner.StunPower : 0;
        m_appliedStunDuration = m_stunDuration + owanerStun;
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
        if (m_team == TeamType.Nature) return;

        Entity victim =other.GetComponentInParent<Entity>();

        if (victim!=null)
        {
            if (victim.Team == m_team) return;
            victim.AddControlEffectStun(m_appliedStunDuration);
            OnHit();
        }
    }
}
