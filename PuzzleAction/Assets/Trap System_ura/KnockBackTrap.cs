using UnityEngine;

[RequireComponent(typeof(ReturnObjectToPool))]
public class KnockBackTrap : MonoBehaviour
{
    [Header("Trap Data")]
    [SerializeField]
    private TrapData m_trapData;

    [Header("Move")]
    [SerializeField]
    private float m_speed = 2f;

    [Header("Life Time")]
    [SerializeField]
    private float m_lifeTime = 3f;

    private ReturnObjectToPool m_returnObjPool;

    private Vector3 m_moveDirection;
    private float m_timer;


    private void Awake()
    {
        m_returnObjPool =
            GetComponent<ReturnObjectToPool>();
    }


    public void Init(Vector3 direction)
    {
        m_moveDirection =
            direction.normalized;

        m_timer = 0f;

        transform.rotation =
            Quaternion.LookRotation(
                m_moveDirection);
    }


    private void Update()
    {
        // 移動方向へまっすぐ移動
        transform.position +=
            m_moveDirection *
            m_speed *
            Time.deltaTime;


        // 時間経過
        m_timer += Time.deltaTime;

        if (m_timer >= m_lifeTime)
        {
            ReturnToPool();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Entity target =
            other.GetComponentInParent<Entity>();

        if (target == null)
            return;


        EntityHP entityHP =
            target.GetComponent<EntityHP>();

        if (entityHP == null)
            return;


        DamageData damageData =
            new DamageData
            {
                Attack = 0f,

                Knockback =
                    m_trapData.m_knockback,

                // 風の移動方向
                AttackDir =
                    m_moveDirection
            };


        // ノックバックはTakeDamage側で処理
        entityHP.TakeDamage(
            damageData);


        // 命中後Poolへ
        ReturnToPool();
    }


    private void ReturnToPool()
    {
        if (m_returnObjPool == null)
            return;

        m_returnObjPool.ReturnToPool();
    }
}