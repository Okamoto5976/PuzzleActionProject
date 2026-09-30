using System.Collections.Generic;
using UnityEngine;

public class StunArea : TrapBase
{
    private List<Entity> m_HitTargets = new List<Entity>();

    [SerializeField] private ParticleSystem m_stunParticle;

    private float m_timer;

    protected override void EntitySetUp()
    {

    }

    public override void TrapInit()
    {
        base.TrapInit();

        CreateTrapDamageData();
    }

    protected override void CreateTrapDamageData()
    {
        base.CreateTrapDamageData();
    }

    protected override void OnHit()
    {

    }

    private void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer > 3f)
        {
            m_timer = 0f;
            m_stunParticle.Play();

            for (int i = 0; i < m_HitTargets.Count; i++)
            {
                 m_HitTargets[i].TakeDamage(m_damageData);
            }
        }


    }

    protected override void OnTriggerEnter(Collider other)
    {
        Entity target = other.GetComponentInParent<Entity>();

        if (target == null) return;

        if (target.Team == TeamType.Nature) return;

        if (!m_HitTargets.Contains(target))
        {
            m_HitTargets.Add(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity target = other.GetComponentInParent<Entity>();

        if (target != null && m_HitTargets.Contains(target))
        {
            m_HitTargets.Remove(target);
        }
    }
    private void OnDrawGizmosSelected()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        if (box != null)
        {
            Gizmos.color = Color.green;
            Vector3 size = new Vector3(
                box.size.x * transform.lossyScale.x,
                box.size.y * transform.lossyScale.y,
                box.size.z * transform.lossyScale.z
            );
            Vector3 center = transform.position + transform.TransformDirection(box.center);
            Gizmos.DrawWireCube(center, size);
        }
    }
}
