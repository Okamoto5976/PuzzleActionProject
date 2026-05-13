using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
abstract public class Entity : MonoBehaviour
{
    //component
    private Rigidbody m_rb;
    //anim
    private EntityHP m_hp;
    //state
    protected State m_state;


    protected float m_speed;

    //private bool m_isDashing;

    protected Vector3 m_movement;

    //public EntityHP m_HP { get; private set; }
    //public EntityMove m_Move { get; private set; }

    private Dictionary<StatusType, EntityStatus> m_status = new();

    public enum Teamtype
    {
        Player,
        Enemy,
        Neutral,
        Nature
    }

    [SerializeField] private Teamtype team;
    public Teamtype Team => team;

    //追加した部分
    //トラップのダメージ対象かどうか
    [Header("Damage")]
    [SerializeField]
    protected bool m_damageable = true;
    public bool Damageable => m_damageable;

    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_hp = GetComponent<EntityHP>();
        m_state = GetComponent<State>();
        //m_Move = GetComponent<EntityMove>();
    }

    protected virtual void Start()
    {
        m_status.Add(StatusType.HP, new EntityStatus(100));
        m_status.Add(StatusType.Attack, new EntityStatus(10));
        m_status.Add(StatusType.Speed, new EntityStatus(10));

    }

    public EntityStatus GetStatus(StatusType type)
    {
        return m_status[type];
    }

    protected virtual void FixedUpdate()
    {
        if(m_state != null && !m_state.CanMove)
        {
            return;
        }

        OnMove(m_movement, m_speed);

    }

    protected void OnMove(Vector3 movement, float speed)
    {
        //m_isDashing? m_dashSpeed:m_speed;
        float m_currentSpeed = speed;

        Vector3 velocity = new Vector3(movement.x * m_currentSpeed, m_rb.linearVelocity.y, movement.z * m_currentSpeed);

        m_rb.MovePosition
            (
                m_rb.position + 
                velocity * Time.fixedDeltaTime

            );
    }

    public virtual void TakeDamage(int damage)//後々DamageDataとDamageResult
    {
        //追加した部分
        //ダメージ無効
        if (!m_damageable)
        {
            return;
        }

        if (m_state != null && m_state.IsInvincible)
        {
            return ;
        }

        //追加した部分
        //HPを持たない(トラップ等)対応
        if(m_hp==null)
        {
            return;
        }

        m_hp.TakeDamage(damage);
    }

    public bool IsSameTeam(Entity other)
    {
        if (other == null) return false;
        return this.team == other.team;
    }

    //public bool IsEnemy(Entity other)
    //{
    //    if (other == null) return false;
    //    return this.team != other.team;
    //}

    //public bool IsPlayer(Entity other)
    //{
    //    if (other == null) return false;
    //    return true;


    //追加した部分
    //相手に攻撃が当たるかどうか
    public virtual bool CanHit(Entity other)
    {
        if (other == null)
        {
            return false;
        }

        // 同チーム無効
        if (Team == other.Team)
        {
            return false;
        }

        return true;
    }
}
