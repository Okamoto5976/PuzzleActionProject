using System.Collections.Generic;
using UnityEngine;

public class HealingArea : MonoBehaviour
{
    [Header("Healing Settings")]
    [SerializeField] private float m_healAmount = 5f;

    [SerializeField] private float m_healInterval = 1f;


    private List<Entity> m_targets =
        new List<Entity>();


    private float m_timer;


    private void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer < m_healInterval)
            return;

        m_timer = 0f;

        HealTargets();
    }


    private void HealTargets()
    {
        foreach (Entity target in m_targets)
        {
            if (target == null)
                continue;


            EntityHP hp =
                target.GetComponent<EntityHP>();


            if (hp == null)
                continue;


            hp.Heal(m_healAmount);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Entity target =
            other.GetComponent<Entity>();


        if (target == null)
            return;


        if (!m_targets.Contains(target))
        {
            m_targets.Add(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity target =
            other.GetComponent<Entity>();


        if (target == null)
            return;


        m_targets.Remove(target);
    }
}