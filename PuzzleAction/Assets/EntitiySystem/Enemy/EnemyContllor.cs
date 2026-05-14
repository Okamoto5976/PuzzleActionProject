using UnityEngine;

public class EnemyContllor : Entity
{
    //[SerializeField] private EnemyData m_data;
    [SerializeField] Transform m_target;

    //private EnemyState m_state;

    protected override void Awake()
    {
        base.Awake();

        //m_state = GetComponent<EnemyState>();

        //ämîFóp
        //Debug.Log(m_state);
    }

    private void Start()
    {
        //Speed = m_data.MoveSpeed;

    }
    private void Update()
    {
        //à⁄ìÆêßå‰
        //if (m_currentState != null && !m_currentState.CanMove)
        //{
        //    return;
        //}

        Vector3 dir = (m_target.position - transform.position);

        dir.y = 0f;
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = rot;

        m_moveDir = dir.normalized;

    }
}
