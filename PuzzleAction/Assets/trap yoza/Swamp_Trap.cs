using System.Collections.Generic;
using UnityEngine;

public class SwampTrap : Entity
{
    [SerializeField]
    private SwampTrapData m_data;

    private readonly Dictionary<Entity, StatusModifier>
        m_slowTargets = new();

    public void Initialize(TrapUseData data)
    {
        transform.position = data.Position;
    }

    private void Update()
    {
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                m_data.m_radius
            );

        HashSet<Entity> currentTargets =
            new();

        foreach (Collider hit in hits)
        {
            Entity target =
                hit.GetComponent<Entity>();

            if (target == null)
            {
                continue;
            }

            currentTargets.Add(target);

            if (!m_slowTargets.ContainsKey(target))
            {
                StatusModifier slow =
                    new StatusModifier();

                slow.m_statType =
                    StatusType.Speed;

                slow.m_modType =
                    ModifierType.Multiply;

                slow.m_value =
                    m_data.m_slowMultiplier;

                slow.m_source =
                    this;

                target
                    .GetStatus(StatusType.Speed)
                    .AddModifier(slow);

                m_slowTargets.Add(
                    target,
                    slow
                );

                Debug.Log(
                    target.name +
                    " Ç™è¿Ç…ì¸Ç¡ÇΩ"
                );
            }
        }

        List<Entity> removeList =
            new();

        foreach (var pair in m_slowTargets)
        {
            if (!currentTargets.Contains(pair.Key))
            {
                pair.Key
                    .GetStatus(StatusType.Speed)
                    .RemoveModifier(pair.Value);

                removeList.Add(pair.Key);

                Debug.Log(
                    pair.Key.name +
                    " Ç™è¿Ç©ÇÁèoÇΩ"
                );
            }
        }

        foreach (Entity target in removeList)
        {
            m_slowTargets.Remove(target);
        }
    }

    private void OnDestroy()
    {
        foreach (var pair in m_slowTargets)
        {
            if (pair.Key != null)
            {
                pair.Key
                    .GetStatus(StatusType.Speed)
                    .RemoveModifier(pair.Value);
            }
        }

        m_slowTargets.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        if (m_data == null)
        {
            return;
        }

        Gizmos.color =
            new Color(
                0.5f,
                0.25f,
                0f
            );

        Gizmos.DrawWireSphere(
            transform.position,
            m_data.m_radius
        );
    }
}