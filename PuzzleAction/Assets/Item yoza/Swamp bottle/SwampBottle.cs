using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ReturnObjectToPool))]
public class SwampBottle : TrapBase
{
    [Header("Gas Area Settings")]
    //[SerializeField] private Collider m_swampCollider;
    //[SerializeField] private GameObject m_swampEffect;
    [SerializeField] private float m_duration = 10f;
    [SerializeField] private float m_tickInterval = 0.5f;

    private bool m_isMudActive = false;
    private float m_swampTimer = 0f;
    private float m_tickTimer = 0f;

    private readonly List<Entity> m_targetsInRange = new List<Entity>();

    protected override void EntitySetUp()
    {
        m_isMudActive = true;
        m_swampTimer = 0f;
        m_tickTimer = 0f;
        m_targetsInRange.Clear();

        if (m_rb != null)
        {
            m_rb.isKinematic = true;
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }

        //if (m_swampCollider != null) m_swampCollider.enabled = true;

        //if (m_swampEffect != null)m_swampEffect.SetActive(true);
    }

    protected override void OnHit()
    {
    }

    private void Update()
    {
        CheckDeadLine();

        if (!m_isMudActive) return;

        m_swampTimer += Time.deltaTime;

        if (m_swampTimer >= m_duration)
        {
            //if (m_swampCollider != null) m_swampCollider.enabled = false;

            //if (m_swampEffect != null)m_swampEffect.SetActive(false);

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

    protected override void OnTriggerEnter(Collider other)
    {
        Entity inGasTarget = other.GetComponentInParent<Entity>();

        if (inGasTarget == null || inGasTarget.Team == m_team) return;

        if (!m_targetsInRange.Contains(inGasTarget))
        {
            m_targetsInRange.Add(inGasTarget);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!m_isMudActive) return;

        Entity target = other.GetComponentInParent<Entity>();
        if (target != null && m_targetsInRange.Contains(target))
        {
            m_targetsInRange.Remove(target);
        }
    }

    private void ApplyPoisonEffect()
    {
        float slowValue = m_trapData != null ? m_trapData.m_attack : 0f;
        
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
}