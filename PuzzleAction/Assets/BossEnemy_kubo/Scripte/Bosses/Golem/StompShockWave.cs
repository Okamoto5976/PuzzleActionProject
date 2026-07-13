using UnityEngine;

public class StompShockWave : MonoBehaviour
{
    private DamageData m_damage;
    private TeamType m_team;

    private float m_radius;
    private float m_speed = 15f;

    public void Initialize(DamageData damage,TeamType team,float radius,float lifeTime)
    {
        m_damage = damage;
        m_team = team;
        m_radius = radius;

        transform.localScale = Vector3.zero;

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale,Vector3.one * m_radius,m_speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity entity = other.GetComponentInParent<Entity>();

        if (entity == null) return;
        if (entity.Team == m_team) return;

        float distance = Vector3.Distance(transform.position,entity.transform.position);

        if (distance > m_radius)return;
        entity.TakeDamage(m_damage);
    }
}