using System.Collections.Generic;
using UnityEngine;

public class HealingArea : TrapBase
{
    [Header("Regenerate Settings")]
    [SerializeField] private float m_healAmount = 5f;
    [SerializeField] private float m_buffDuration = 1.0f;

    [SerializeField] private float m_lifeTime = 10f;


    private readonly List<Entity> m_targets =
        new List<Entity>();

    private float m_timer;


    private void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer < m_buffDuration)
            return;

        m_timer = 0f;

        ApplyRegenerate();
    }


    protected override void EntitySetUp()
    {
        Invoke(nameof(OnHit), m_lifeTime);
    }

    protected override void OnHit()
    {
        OnReturnPool();
    }

    private void ApplyRegenerate()
    {
        foreach (Entity target in m_targets)
        {
            if (target == null)
                continue;

            AddRegenerate(target);
        }
    }


    private void AddRegenerate(Entity target)
    {
        StatusModifier modifier = new StatusModifier
        {
            m_statType = StatusType.Regenerate,
            m_value = m_healAmount,
            m_modType = ModifierType.Add
        };

        target.AddBuff(
            modifier,
            BuffID.Regenerate,
            m_buffDuration
        );
    }


    protected override void OnTriggerEnter(Collider other)
    {
        Entity target = other.GetComponent<Entity>();

        if (target == null)
            return;

        if (!m_targets.Contains(target))
        {
            m_targets.Add(target);

             
            AddRegenerate(target);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        Entity target = other.GetComponent<Entity>();

        if (target == null)
            return;

        m_targets.Remove(target);
    }
}
