using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ReturnObjectToPool))]
public class GasCylinder : TrapBase
{
    [Header("Hit/Layer Settings")]
    [SerializeField] private LayerMask m_hitLayers;

    [Header("Gas Area Settings")]
    [SerializeField] private Collider m_gasCollider;
    [SerializeField] private GameObject m_gasEffect;
    [SerializeField] private float m_duration = 10f;
    [SerializeField] private float m_tickInterval = 0.5f;
    [SerializeField] private float m_poisonDamage = 2f;
    [SerializeField] private float m_poisonDuration = 3f;

    private bool m_isGassing=false;
    private bool m_isAddForceCalled = false;
    private float m_gasTimer = 0f;
    private float m_tickTimer = 0f;

    private readonly List<Entity>m_targetsInRange=new List<Entity>();

    protected override void EntitySetUp()
    {
        m_isGassing = false;
        m_isAddForceCalled = false;
        m_gasTimer = 0f;
        m_tickTimer=0f;
        m_targetsInRange.Clear();

        if(m_rb!=null)
        {
            m_rb.isKinematic = false;
            m_rb.linearVelocity=Vector3.zero;
            m_rb.angularVelocity=Vector3.zero;
        }
        if (m_gasCollider != null) m_gasCollider.enabled = false;
        if(m_gasEffect!=null)m_gasEffect.SetActive(false);
    }

    public override void TrapInit()
    {
        base.TrapInit();
        EntitySetUp();
    }
    protected override void OnHit()
    {
        StartGas();
    }
    private void FixedUpdate()
    {
        if (!m_isGassing)
        {
            if (!m_isAddForceCalled)
            {
                OnAddForce(m_dir, m_power);
                m_isAddForceCalled = true;
            }
        }
    }
    private void Update()
    {
        CheckDeadLine();

        if (!m_isGassing) return;

        m_gasTimer += Time.deltaTime;
        if (m_gasTimer >= m_duration)
        {
            if (m_gasCollider != null) m_gasCollider.enabled = false;
            if (m_gasEffect != null) m_gasEffect.SetActive(false);
            OnReturnPool();
            return;
        }
       
        m_tickTimer += Time.deltaTime;
        if(m_tickTimer>=m_tickInterval)
        {
            m_tickTimer = 0;
            ApplyPoisonEffect();
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if(!m_isGassing)
        {
            if((m_hitLayers.value&(1<<other.gameObject.layer))!=0)
            {
                OnHit();
                return;
            }

            Entity hitTarget = other.GetComponent<Entity>();
            if (hitTarget != null && hitTarget.Team != m_team)
            {
                OnHit();
                return;
            }
            return;
        }

        Entity inGasTarget=other.GetComponent<Entity>();
        if (inGasTarget == null) return;
        if (inGasTarget.Team == m_team) return;

        if(!m_targetsInRange.Contains(inGasTarget))
        {
            m_targetsInRange.Add(inGasTarget);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!m_isGassing) return;

        Entity target=other.GetComponent<Entity>();
        if (target != null && m_targetsInRange.Contains(target))
        {
            m_targetsInRange.Remove(target);
        }
    }
    private void StartGas()
    {
        m_isGassing = true;

        if(m_rb!=null)
        {
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
            m_rb.isKinematic = true;
        }
        if (m_gasCollider != null) m_gasCollider.enabled = true;
        if (m_gasEffect != null) m_gasEffect.SetActive(true);
    }

    private void ApplyPoisonEffect()
    {
        for(int i=m_targetsInRange.Count-1; i>=0; i--)
        {
            Entity target=m_targetsInRange[i];

            if(target==null)
            {
                m_targetsInRange.RemoveAt(i);
                continue;
            }
            StatusModifier poisonModifier = new StatusModifier
            {
                m_statType = StatusType.Poison,
                m_value = m_poisonDamage,
                m_modType = ModifierType.Add
            };
            target.AddBuff(poisonModifier,BuffID.Poison,m_poisonDuration);
        }
    }
}
