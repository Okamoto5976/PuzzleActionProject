using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ReturnObjectToPool))]
public class PoisonMagic : TrapBase
{
    [Header("Direct Hit Setting")]
    [SerializeField] private LayerMask m_hitLayers;
    [SerializeField] private float m_lifeTime = 5f;

    [SerializeField] private float m_duration = 10f;
    [SerializeField] private float m_tickInterval = 0.5f;
    [SerializeField] private float m_poisonDuration = 4f;

    private bool m_isAreaActive = false;
    private bool m_isAddForceCalled = false;
    private float m_flyTimer = 0f;
    private float m_areaTimer = 0f;
    private float m_tickTimer = 0f;

    private readonly List<Entity> m_targetsInRange = new List<Entity>();
    protected override void EntitySetUp()
    {
        m_isAreaActive = false;
        m_isAddForceCalled = false;
        m_flyTimer = 0f;
        m_areaTimer = 0f;
        m_tickTimer = 0f;
        m_targetsInRange.Clear();

        if (m_rb != null)
        {
            m_rb.isKinematic = false;
            m_rb.useGravity = false;
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }
        //if(m_poisonCollider!=null)m_poisonCollider.enabled= false;
        //if(m_poisonEffect!=null)m_poisonEffect.SetActive(false);
    }
   
    
    //public override void TrapInit(ItemRecieveData data)
    //{
    //    base.TrapInit(data);
    //    //EntitySetUp();
    //}

    private void FixedUpdate()
    {
        if (!m_isAreaActive && !m_isAddForceCalled)
        {
            OnAddForce(m_dir, m_power);
            m_isAddForceCalled = true;
        }
    }

    private void Update()
    {
        CheckDeadLine();

        if (!m_isAreaActive)
        {
            m_flyTimer += Time.deltaTime;
            if (m_flyTimer >= m_lifeTime)
            {
                OnReturnPool();
            }
            return;
        }

        m_areaTimer += Time.deltaTime;
        if (m_areaTimer >= m_duration)
        {
            //if (m_poisonCollider != null) m_poisonCollider.enabled = false;
            //if (m_poisonEffect != null) m_poisonEffect.SetActive(false);
            OnReturnPool();
            return;
        }

        m_tickTimer += Time.deltaTime;
        if (m_tickTimer >= m_tickInterval)
        {
            m_tickTimer = 0f;
            ApplyPoisonEffect();
        }
    }

    protected override void OnHit()
    {
        StartPoisonArea();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (!m_isAreaActive)
        {
            if ((m_hitLayers.value & (1 << other.gameObject.layer)) != 0)
            {
                OnHit();
                return;
            }

            Entity hitTarget = other.GetComponentInParent<Entity>();
            if (hitTarget != null && hitTarget.Team != m_team)
            {
                CreateDamageData();
                hitTarget.TakeDamage(m_damageData);

                OnHit();
            }
            return;
        }

        Entity inAreaTarget = other.GetComponentInParent<Entity>();
        if (inAreaTarget == null || inAreaTarget.Team == m_team) return;

        if (!m_targetsInRange.Contains(inAreaTarget))
        {
            m_targetsInRange.Add(inAreaTarget);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!m_isAreaActive) return;

        Entity target = other.GetComponentInParent<Entity>();
        if (target != null && m_targetsInRange.Contains(target))
        {
            m_targetsInRange.Remove(target);
        }
    }

    private void StartPoisonArea()
    {
        m_isAreaActive = true;

        if (m_rb != null)
        {
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
            m_rb.isKinematic = true;
        }
    }

    private void ApplyPoisonEffect()
    {
        float poisonDamage = m_trapData != null ? m_trapData.m_attack : 0f;

        for (int i = m_targetsInRange.Count - 1; i >= 0; i--)
        {
            Entity target = m_targetsInRange[i];

            if (target == null)
            {
                m_targetsInRange.RemoveAt(i);
                continue;
            }

            StatusModifier poisonModifier = new StatusModifier
            {
                m_statType = StatusType.Poison,
                m_value = poisonDamage,
                m_modType = ModifierType.Add
            };
            target.AddBuff(poisonModifier, BuffID.Poison, m_poisonDuration);
        }
    }
}