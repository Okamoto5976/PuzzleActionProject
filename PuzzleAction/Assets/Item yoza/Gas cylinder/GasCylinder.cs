using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ReturnObjectToPool))]
public class GasCylinder : TrapBase
{
    [Header("Hit/Layer Settings")]
    //[SerializeField] private LayerMask m_hitLayers;

    [Header("Gas Area Settings")]
    [SerializeField] private float m_duration = 10f;
    [SerializeField] private float m_tickInterval = 0.5f;
    //[SerializeField] private float m_poisonDuration = 3f;

    private bool m_isGassing = false;
    private bool m_isAddForceCalled = false;
    private float m_gasTimer = 0f;
    private float m_tickTimer = 0f;

    private readonly List<Entity> m_targetsInRange = new List<Entity>();

    protected override void EntitySetUp()
    {
        m_isGassing = false;
        m_isAddForceCalled = false;
        m_gasTimer = 0f;
        m_tickTimer = 0f;
        m_targetsInRange.Clear();

        if (m_rb != null)
        {
            m_rb.isKinematic = false;
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }
    }

    protected override void OnHit()
    {
        StartGas();
    }

    private void FixedUpdate()
    {
        if (!m_isGassing && !m_isAddForceCalled)
        {
            //OnAddForce(m_dir, m_power);
            m_isAddForceCalled = true;
        }
    }
    private void Update()
    {
        CheckDeadLine();

        if (!m_isGassing) return;

        m_gasTimer += Time.deltaTime;
        if (m_gasTimer >= m_duration)
        {
            OnReturnPool();
            return;
        }

        m_tickTimer += Time.deltaTime;
        if (m_tickTimer >= m_tickInterval)
        {
            m_tickTimer = 0;
            ApplyPoisonEffect();
        }
    }

    private void StartGas()
    {
        m_isGassing = true;

        if (m_rb != null)
        {
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
            m_rb.isKinematic = true;
        }
    }

    private void ApplyPoisonEffect()
    {
        float poisonDamage = m_trapData != null ? m_trapData.m_attack : 0;

        for (int i = m_targetsInRange.Count - 1; i >= 0; i--)
        {
            Entity target = m_targetsInRange[i];

            if (target == null)
            {
                m_targetsInRange.RemoveAt(i);
                continue;
            }

            if (m_trapData.m_buffSetting.Count != 0)
            {
                foreach (var buff in m_trapData.m_buffSetting)
                {
                    if (buff.m_duration <= 0) continue;

                    var modifier = SetModifier(buff);

                    target.AddBuff(modifier, buff.m_buffID, buff.m_duration);
                }
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!m_isGassing)
        {
            //if ((m_hitLayers.value & (1 << other.gameObject.layer)) != 0)
            //{
            //    OnHit();
            //    return;
            //}

            Entity hitTarget = other.GetComponentInParent<Entity>();
            if (hitTarget != null && hitTarget.Team != m_team)
            {
                OnHit();
                return;
            }
            return;
        }

        Entity inGasTarget = other.GetComponentInParent<Entity>();
        if (inGasTarget == null || inGasTarget.Team == m_team) return;

        if (!m_targetsInRange.Contains(inGasTarget))
        {
            m_targetsInRange.Add(inGasTarget);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!m_isGassing) return;

        Entity target = other.GetComponentInParent<Entity>();
        if (target != null && m_targetsInRange.Contains(target))
        {
            m_targetsInRange.Remove(target);
        }
    }
    
}