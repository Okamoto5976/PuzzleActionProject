using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ReturnObjectToPool))]
public class SwampBottle : TrapBase
{
    [Header("Hit/Layer Settings")]
    [SerializeField] private LayerMask m_hitLayers;

    [Header("Gas Area Settings")]
    [SerializeField] private Collider m_swampCollider;
    [SerializeField] private GameObject m_swampEffect;
    [SerializeField] private float m_duration = 10f;
    [SerializeField] private float m_tickInterval = 0.5f;
    [SerializeField] private float m_slowAmount = 0.3f;
    [SerializeField] private float m_slowTimer = 3f;

    private bool m_isMudActive = false;
    private bool m_isAddForceCalled = false;
    private float m_swampTimer = 0f;
    private float m_tickTimer = 0f;

    private readonly List<Entity> m_targetsInRange = new List<Entity>();

    protected override void EntitySetUp()
    {
        // 設置した時点で沼を開始
        m_isMudActive = true;
        m_isAddForceCalled = false;
        m_swampTimer = 0f;
        m_tickTimer = 0f;
        m_targetsInRange.Clear();

        // 設置型なので動かさない
        if (m_rb != null)
        {
            m_rb.isKinematic = true;
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }

        // 沼を最初から有効にする
        if (m_swampCollider != null)
            m_swampCollider.enabled = true;

        if (m_swampEffect != null)
            m_swampEffect.SetActive(true);
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
            if (m_swampCollider != null)
                m_swampCollider.enabled = false;

            if (m_swampEffect != null)
                m_swampEffect.SetActive(false);

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
        base.OnTriggerEnter(other);

        Entity inGasTarget =
            other.GetComponentInParent<Entity>();

        if (inGasTarget == null) return;

        if (inGasTarget.Team == m_team) return;

        if (!m_targetsInRange.Contains(inGasTarget))
        {
            m_targetsInRange.Add(inGasTarget);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!m_isMudActive) return;

        Entity target =
            other.GetComponentInParent<Entity>();

        if (target != null && m_targetsInRange.Contains(target))
        {
            m_targetsInRange.Remove(target);
        }
    }

    private void ApplyPoisonEffect()
    {
        for (int i = m_targetsInRange.Count - 1; i >= 0; i--)
        {
            Entity target = m_targetsInRange[i];

            if (target == null)
            {
                m_targetsInRange.RemoveAt(i);
                continue;
            }

            StatusModifier slowModifier = new StatusModifier
            {
                m_statType = StatusType.Slow,
                m_value = m_slowAmount,
                m_modType = ModifierType.Add
            };

            target.AddBuff(
                slowModifier,
                BuffID.Water,
                m_slowTimer
            );
        }
    }
}