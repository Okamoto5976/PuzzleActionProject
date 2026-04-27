using UnityEngine;

abstract public class Entity : MonoBehaviour
{
    //component
    private Rigidbody m_rb;
    //anim
    private EntityHP m_hp;



    protected float m_speed;

    //private bool m_isDashing;

    protected Vector3 m_movement;

    //public EntityHP m_HP { get; private set; }
    //public EntityMove m_Move { get; private set; }

    public enum Teamtype
    {
        Player,
        Enemy,
        Neutral
    }

    [SerializeField] private Teamtype team;
    public Teamtype Team => team;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_hp = GetComponent<EntityHP>();
        //m_Move = GetComponent<EntityMove>();
    }

    protected virtual void FixedUpdate()
    {
        OnMove(m_movement, m_speed);

    }

    public void OnMove(Vector3 movement, float speed)
    {
        //m_isDashing? m_dashSpeed:m_speed;
        float m_currentSpeed = speed;

        Vector3 velocity = new Vector3(movement.x * m_currentSpeed, m_rb.linearVelocity.y, movement.z * m_currentSpeed);

        m_rb.linearVelocity = velocity;
    }

    public bool IsSameTeam(Entity other)
    {
        if (other == null) return false;
        return this.team== other.team;
    }

    public bool IsEnemy(Entity other)
    {
        if (other == null) return false;
        return this.team != other.team;
    }
}
