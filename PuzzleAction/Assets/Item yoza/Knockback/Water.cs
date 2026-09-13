using UnityEngine;

public class Water : TrapBase
{
    [Header("WaterGun Settings")]
    [SerializeField] private float m_SlowTimer = 3f;
    [SerializeField] private float m_slowAmount = 0.3f;

    protected override void EntitySetUp()
    {
        
    }
    protected override void OnHit()
    {
        OnReturnPool();

    }

    private void FixedUpdate()
    {
        OnMove(m_dir);
        //CheckRange();
    }

    private void Update()
    {
        CheckDeadLine();

    }

    public override void TrapInit()
    {
        base.TrapInit();
    }

    protected override void OnTriggerEnter(Collider other)
    {

        Entity target=other.GetComponent<Entity>();
        if (target == null) return;

        //é©îöÇµÇ»Ç¢
        if (target.Team == m_team) return;

        //0É_ÉÅ
        //DamageData damageData = SetDamageData();
        //damageData.Attack = 0f;
        //damageData.Attacker = m_owner;
        //target.TakeDamage(damageData);

        //ì›ë´ïtó^
        StatusModifier slowModifier = new StatusModifier
        {
            m_statType = StatusType.Slow,
            m_value = m_slowAmount,
            m_modType = ModifierType.Add
        };
           
        target.AddBuff(slowModifier, BuffID.Water, m_SlowTimer);

        OnHit();
    }

}
