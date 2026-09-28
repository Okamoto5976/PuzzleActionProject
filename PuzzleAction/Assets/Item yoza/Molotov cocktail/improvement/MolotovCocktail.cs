using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ReturnObjectToPool))]
public class MolotovCocktail : TrapBase
{
    [Header("Hit/Layer Settings")]
    [SerializeField] private LayerMask m_hitLayers;

    [Header("Fire Area Settings")]
    [SerializeField] private Collider m_fireCollider;
    [SerializeField] private GameObject m_fireEffect;
    [SerializeField] private float m_duration = 1f;
    [SerializeField] private float m_tickInterval = 0.5f;
    [SerializeField] private float m_damagePerTick = 5f;
    [SerializeField] private float m_burnBuffDuration = 1f;

    private bool m_isBurning = false;
    private bool m_isAddForceCalled = false;
    private float m_burnTimer = 0f;
    private float m_tickTimer = 0f;

    private readonly List<Entity> m_targetsInRange = new List<Entity>();

    protected override void EntitySetUp()
    {
        m_isBurning = false;
        m_isAddForceCalled = false;
        m_burnTimer = 0f;
        m_tickTimer = 0f;
        m_targetsInRange.Clear();

        if(m_rb!=null)
        {
            m_rb.isKinematic = false;
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }

        if (m_fireCollider != null) m_fireCollider.enabled = false;
        if (m_fireEffect != null) m_fireEffect.gameObject.SetActive(false);
    }

    protected override void OnHit()
    {
        StartFire();
    }

    private void FixedUpdate()
    {
        if (!m_isBurning)
        {
            if(!m_isAddForceCalled)
            {
                OnAddForce(m_dir, m_power);
                m_isAddForceCalled=true;
            }
        }
    }

    private void Update()
    {
        CheckDeadLine();
        if (!m_isBurning) return;

        m_burnTimer += Time.deltaTime;
        if (m_burnTimer >= m_duration)
        {
            if (m_fireCollider != null) m_fireCollider.enabled = false;
            if (m_fireEffect != null) m_fireEffect.gameObject.SetActive(false);
            OnReturnPool();
            return;
        }

        m_tickTimer += Time.deltaTime;
        if (m_tickTimer >= m_tickInterval)
        {
            m_tickTimer = 0f;
            ApplyFireDamage();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (!m_isBurning)
        {
           if((m_hitLayers.value &(1<<other.gameObject.layer)) != 0)
            {
                OnHit();
            return;
            }
           Entity hitTarget=other.GetComponentInParent<Entity>();
            if (hitTarget != null && hitTarget.Team != m_team)
            {
                OnHit();
                return;
            }
            return;
        }

        Entity inFireTarget = other.GetComponentInParent<Entity>();
        if (inFireTarget == null) return;
        if (inFireTarget.Team == m_team) return;

        if (!m_targetsInRange.Contains(inFireTarget))
        {
            m_targetsInRange.Add(inFireTarget);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!m_isBurning) return;

        Entity target = other.GetComponentInParent<Entity>();
        if (target != null && m_targetsInRange.Contains(target))
        {
            m_targetsInRange.Remove(target);
        }
    }

    private void StartFire()
    {
        m_isBurning = true;

        if (m_rb != null)
        {
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
            m_rb.isKinematic = true;
        }

        if (m_fireCollider != null) m_fireCollider.enabled = true;

        if (m_fireEffect != null) m_fireEffect.gameObject.SetActive(true);
    }

    private void ApplyFireDamage()
    {
        for (int i = m_targetsInRange.Count - 1; i >= 0; i--)
        {
            Entity target = m_targetsInRange[i];

            if (target == null)
            {
                m_targetsInRange.RemoveAt(i);
                continue;
            }

            StatusModifier burnModifier = new StatusModifier
            {
                m_statType = StatusType.Burn,
                m_value = m_damagePerTick + m_str,
                m_modType = ModifierType.Add
            };

            target.AddDamageBuff(burnModifier, BuffID.Burn, m_burnBuffDuration);
        }
    }
}