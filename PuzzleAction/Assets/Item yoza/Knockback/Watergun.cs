using UnityEngine;

public class Watergun : TrapBase
{
    [Header("WaterGun Settings")]
    [SerializeField] private float m_Slowmoving = 3f;
    [SerializeField] private float m_slowAmount = 0.3f;
   protected override void SetUp()
    {
        
    }
    protected override void OnHit()
    {
        
    }
    private void FixedUpdate()
    {
        OnMove(m_dir);
        CheckRange();
        CheckDeadLine();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        Entity target=other.GetComponent<Entity>();
        if (target == null) return;

        //é©îöÇµÇ»Ç¢
        if (target.Team == m_team) return;

        //0É_ÉÅ
        DamageData damageData = SetDamageData();
        damageData.Attack = 0f;
        damageData.Attacker = m_owner;
        target.TakeDamage(damageData);

        //ì›ë´ïtó^
        StatusModifier slowModifier = new StatusModifier
        {
            m_statType = StatusType.Slow,
            m_value = m_slowAmount,
            m_modType = ModifierType.Add
        };
           
        target.AddBuff(slowModifier, BuffID.Slow, m_Slowmoving);

        OnReturnPool();
    }

}
