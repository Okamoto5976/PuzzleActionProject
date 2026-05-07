using UnityEngine;

public class EnemyContllor : Entity
{
    [SerializeField] private EnemyData m_data;
    [SerializeField] Transform m_target;

    private void Start()
    {
        m_speed = m_data.MoveSpeed;

    }
    private void Update()
    {
        Vector3 dir = (m_target.position - transform.position);

        m_movement = dir.normalized;
    }

    
    
}
