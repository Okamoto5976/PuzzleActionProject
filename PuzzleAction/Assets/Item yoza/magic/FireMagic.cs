using UnityEngine;

[RequireComponent(typeof(ReturnObjectToPool))]
public class FireMagic : TrapBase
{
    [Header("Fire Magic Settings")]
    [SerializeField] private float m_damage = 10f;
    [SerializeField] private float m_burnDamage = 2f;
    [SerializeField] private float m_burnDuration = 4f;
    [SerializeField] private float m_lifeTime = 5f;

    private bool m_isAddForceCalled = false;
    private float m_timer = 0f;

    protected override void EntitySetUp()
    {
        m_isAddForceCalled = false;
        m_timer = 0f;

        if(m_rb!=null)
        {
            m_rb.isKinematic = false;
            m_rb.useGravity = false;
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }
    }
    public override void TrapInit()
    {
        base.TrapInit();
        EntitySetUp();
    }

    private void FixedUpdate()
    {
        if(!m_isAddForceCalled)
        {
            OnAddForce(m_dir, m_power);
            m_isAddForceCalled=true;
        }
    }

    private void Update()
    {
        CheckDeadLine();

        m_timer += Time.deltaTime;
        if(m_timer>=m_lifeTime)
        {
            OnReturnPool();
        }
    }

    protected override void OnHit()
    {
        OnReturnPool();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (m_team == TeamType.Nature) return;

        Entity target =other.GetComponent<Entity>();
        if(target==null) return;
        if (target.Team == m_team) return;

        DamageData damageData = new DamageData
        {
            Attack = m_damage,
            Attacker = m_owner,
            AttackDir = m_dir,
        };
        target.TakeDamage(damageData);

        StatusModifier burnModifier = new StatusModifier
        {
            m_statType = StatusType.Burn,
            m_value = m_burnDamage,
            m_modType = ModifierType.Add
        };
        target.AddBuff(burnModifier, BuffID.Burn, m_burnDuration);

        OnHit();
    }
}
