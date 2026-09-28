using UnityEngine;
[RequireComponent(typeof(ReturnObjectToPool))]
public class Water : TrapBase
{
    [Header("WaterGun Settings")]
    [SerializeField] private float m_SlowTimer = 3f;
    [SerializeField] private float m_slowAmount = 0.3f;
    [SerializeField] private float m_lifeTimer = 5f;

    private bool m_isAddForceCalled=false;
    private float m_timer = 0f;

    protected override void EntitySetUp()
    {
        m_isAddForceCalled = false;
        m_timer = 0f;

        if(m_rb!=null)
        {
            m_rb.isKinematic =false;
            m_rb.useGravity = false;
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }
    }
    public override void TrapInit()
    {
        base.TrapInit();
        
        m_isAddForceCalled = false;
        m_timer = 0f;

        if (m_rb!=null)
        {
            m_rb.isKinematic = false;
            m_rb.useGravity = false;
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity= Vector3.zero;
        }
    }
    private void FixedUpdate()
    {
        if (!m_isAddForceCalled)
        {
            OnAddForce(m_dir, m_power);
            m_isAddForceCalled = true;
        }
    }
    private void Update()
    {
        CheckDeadLine();

        m_timer += Time.deltaTime;
        if(m_timer>=m_lifeTimer)
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
        if (m_team == TeamType.Nature) return;

        Entity target=other.GetComponent<Entity>();
        if (target == null) return;

        if (target.Team == m_team) return;

        StatusModifier slowModifier = new StatusModifier
        {
            m_statType = StatusType.Slow,
            m_value = m_slowAmount,
            m_modType = ModifierType.Add
        };
        target.AddBuff(slowModifier,BuffID.Water,m_SlowTimer);

        OnHit();
    }
}
