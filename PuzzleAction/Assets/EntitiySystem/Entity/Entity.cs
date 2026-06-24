using System.Collections.Generic;
using UnityEngine;
public enum TeamType
{
    Player,
    Enemy,
    Nature
}
[RequireComponent (typeof(Rigidbody))]
abstract public class Entity : MonoBehaviour
{
    //Entity Status
    public float HP => m_status[StatusType.HP].Value;
    public float STR  => m_status[StatusType.Strength].Value; 
    public float KnockBack => m_status[StatusType.KnockBack].Value;
    public float DEF => m_status[StatusType.Defense].Value;
    public float Speed => m_status[StatusType.Speed].Value;
    public float DashSpeed => m_status[StatusType.DashSpeed].Value;
    public float CriticalRate => m_status[StatusType.CriticalRate].Value;
    public float CriticalDamage => m_status[StatusType.CriticalDamage].Value;
    public float DEX => m_status[StatusType.Dexterity].Value;
    public float AGI => m_status[StatusType.Agility].Value;
    public float Vision => m_status[StatusType.Vision].Value;
    public float BreakRate => m_status[StatusType.BreakRate].Value;
    public float Stun => m_status[StatusType.Stun].Value;
    public float PoisonRes => m_status[StatusType.PoisonRes].Value;
    public float StunRes => m_status[StatusType.StunRes].Value;
    public float SlowRes => m_status[StatusType.SlowRes].Value;
    public float BlindRes => m_status[StatusType.BlindRes].Value;

    //���
    public enum EntityState
    {
        Idle,
        Attack,
        Damage,
        Dead
    }
    protected EntityState m_currentState = EntityState.Idle;
    public EntityState CurrentState { get => m_currentState; }


    //component
    protected Rigidbody m_rb;
    //protected Animator m_anim;
    protected EntityHP m_entityHP;

    protected EntityBuffSystem m_buffSystem;

    protected Inventory m_inventory;
    //SE
    [SerializeField] 
    protected AudioClip m_attackSE;

    public AudioClip AttackSE => m_attackSE;

    [SerializeField]
    protected AudioClip m_damageSE;

    public AudioClip DamageSE=> m_damageSE;

    protected AudioSource m_audioSource;
    public AudioSource AudioSource => m_audioSource;

    

    [SerializeField] protected TeamType m_team;
    public TeamType Team => m_team;

    [SerializeField] private EntityData m_data;

    //�Q�[���I�[�o�[�ȂǃC�x���g�� �ړ��L�[����
    protected bool m_canMove;
    //�m�b�N�o�b�N����X�^�����@�����Ȃ��t���O�i���Ԍo�߂ŉ񕜁j
    protected bool m_isStun;
    //���G���@�_���[�W���@�����
    protected bool m_isInvincible;

    public bool CanMove { get => m_canMove; }
    public bool IsStun { get => m_isStun; }
    public bool IsInvincible {  get => m_isInvincible; }

    protected float m_stunTime;

    protected float m_currentMoveSpeed;

    protected Dictionary<StatusType, EntityStatus> m_status = new();

    protected Vector3 m_moveDir;
    protected Vector3 m_velocity;

    public Vector3 MoveDir { get => m_moveDir; }

    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_entityHP = GetComponent<EntityHP>();
        m_audioSource=GetComponent<AudioSource>();

        m_buffSystem=GetComponent<EntityBuffSystem>();
        m_inventory = GetComponent<Inventory>();

        if (m_data == null) return;
        m_status.Add(StatusType.HP, new EntityStatus(m_data.HP));
        m_status.Add(StatusType.Strength, new EntityStatus(m_data.STR));
        m_status.Add(StatusType.KnockBack, new EntityStatus(m_data.KnockBack));
        m_status.Add(StatusType.Defense, new EntityStatus(m_data.DEF));
        m_status.Add(StatusType.Speed, new EntityStatus(m_data.Speed));
        m_status.Add(StatusType.DashSpeed, new EntityStatus(m_data.DashSpeed));
        m_status.Add(StatusType.CriticalRate, new EntityStatus(m_data.CriticalRate));
        m_status.Add(StatusType.CriticalDamage, new EntityStatus(m_data.CriticalDamage));
        m_status.Add(StatusType.Dexterity, new EntityStatus(m_data.DEX));
        m_status.Add(StatusType.Agility, new EntityStatus(m_data.AGI));
        m_status.Add(StatusType.Vision, new EntityStatus(m_data.Vision));
        m_status.Add(StatusType.BreakRate, new EntityStatus(m_data.BreakRate));
        m_status.Add(StatusType.Stun, new EntityStatus(m_data.Stun));
        m_status.Add(StatusType.PoisonRes, new EntityStatus(m_data.PoisonRes));
        m_status.Add(StatusType.StunRes, new EntityStatus(m_data.StunRes));
        m_status.Add(StatusType.SlowRes, new EntityStatus(m_data.SlowRes));
        m_status.Add(StatusType.BlindRes, new EntityStatus(m_data.BlindRes));

        m_currentMoveSpeed = Speed;
    }

    private void Start()
    {
        


    }

    public EntityStatus GetStatus(StatusType type)
    {
        return m_status[type];
    }

    public void AddBuff(StatusModifier modifier,float duration)
    {
        if(m_buffSystem==null)
        {
            return;
        }
        m_buffSystem.AddBuff(modifier,duration);
    }
    protected virtual void FixedUpdate()
    {
        if (m_isStun) return;
        if (m_canMove) return;
        
        OnMove(m_moveDir);

    }

    protected virtual void Update()
    {

        if(m_isStun)
        {
            m_stunTime -= Time.deltaTime;

            if(m_stunTime <= 0)
            {
                m_isStun = false;

                ChangeState(EntityState.Idle);
            }
        }
    }

    protected void OnMove(Vector3 dir)
    {
        //Vector3 velocity = new Vector3(dir.x * speed, m_rb.linearVelocity.y, dir.z * speed);

        //m_rb.MovePosition
        //    (
        //        m_rb.position + 
        //        velocity * Time.fixedDeltaTime

        //    );
        dir = dir.normalized; //�����I�Ƀx�N�g����1��

        m_velocity = m_rb.linearVelocity;

        m_velocity.x = dir.x * m_currentMoveSpeed;
        m_velocity.z = dir.z * m_currentMoveSpeed;

        m_rb.linearVelocity = m_velocity;

    }

    //EntityをTakeDamageに
    public virtual void TakeDamage(DamageData data)//��XDamageData��DamageResult
    {
        Debug.Log("TakeDamageよばれた");
        if (m_isInvincible) return;

        if (m_entityHP == null) return;

        m_entityHP.TakeDamage(data);
    }

    //��ԕύX�p
    public void ChangeState(EntityState newState)
    {
        m_currentState = newState;
    }

    public void SetCanMove(bool value) => m_canMove = value;
    public void SetIsStun(bool value) => m_isStun = value;
    public void SetIsInvincible(bool value) => m_isInvincible = value;

    //�m�b�N�o�b�N�̏���
    //EntityState��ύX
    public virtual void ApplyKnockBack(Vector3 direction,float power,float stunTime)
    {
        //���G�Ȃ疳���ɂ������ꍇ
        if (m_isInvincible) return;

        ChangeState(EntityState.Damage);

        //�X�^�����
        m_isStun = true;
        m_stunTime = stunTime;
        direction.y = 0;

        //���̑��x�����Z�b�g
        m_rb.linearVelocity = Vector3.zero;

        //�͂�������
        m_rb.AddForce(direction.normalized*power,ForceMode.Impulse);
    }
    public virtual bool ReceiveItem(Item item)
    {
        if (item == null) return false;
        if(m_inventory==null) return false;

        return m_inventory.AddItem(item.itemId);
    }
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


//�ǉ���������
//����ɍU���������邩�ǂ���
//public virtual bool CanHit(Entity other)
//{
//    if (other == null)
//    {
//        return false;
//    }

//    // ���`�[������
//    if (Team == other.Team)
//    {
//        return false;
//    }

//    return true;
//}

//public bool IsSameTeam(Entity other)
//{
//    if (other == null) return false;
//    return this.m_team == other.m_team;
//}