using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EntityTemporaryBuffSystem))]
public class EnemyController : Entity
{
    [Header("Target")]
    [SerializeField] private Vector3Asset m_target;
    [Header("Range")]
    [SerializeField] private float m_findRange = 8f;
    [SerializeField] private float m_attackRange = 1.5f;
    [Header("Attack")]
    [SerializeField] private float m_attackCooldown = 1f;
    private float m_attackCooldownDuration;
    private bool m_isCooldownEnd = true;
    [Header("AttachCollider Setthing")]
    [SerializeField] private AttackHitBox m_attackHitBox;
    private HitCollider m_hitCollider;
    [System.Serializable]
    public class AttackItem
    {
        public string ItemAnimation;
        public Item attackItem;
    }
    [Header("Item")]
    [SerializeField] private bool m_isRandom = false;
    [SerializeField] private List<AttackItem> m_attackItems = new();
    [SerializeField] private float m_power = 3f;
    [SerializeField] private Vector3 m_shootOffset = new Vector3(0f, 0.5f, 0f);
    private int m_itemIndex;
    #region UnityEditor
    #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (m_attackItems == null) return;

            Vector3 shootPos = transform.position + m_shootOffset;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(shootPos, 0.15f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(shootPos, shootPos + transform.forward * 2f);

            Handles.color = Color.white;
            Handles.Label(shootPos + Vector3.up * 0.3f, "Shoot Offset");
        }
    #endif
    #endregion
    private ItemManager m_itemManager;

    [Header("Drop")]
    [SerializeField] private GachaEngine m_itemDropGachaEngine;
    public GachaEngine ItemDropGachaEngine => m_itemDropGachaEngine;
    [NonSerialized] public Item m_dropItem;

    public Item DropItem
    {
        get => m_dropItem;
        set => m_dropItem = value;
    }

    private NavMeshAgent m_agent;
    private IEnemyBehaviour m_enemyBehaviour;
    private Vector3 m_spawnPosition;

    private bool m_isRotating = true;

    //===== API =====
    public bool CanAction => CurrentState != EntityState.Dead && !IsStun;
    public float AttackRange => m_attackRange;
    public float FindRange => m_findRange;
    public bool IsCooldownReady => m_isCooldownEnd;
    public float ShootPower => m_power;
    public Vector3 Forward => transform.forward;
    public Vector3 SpawnPosition => m_spawnPosition;
    public Vector3Asset Target => m_target;
    public NavMeshAgent Agent => m_agent;
    public AttackHitBox AttackHitBox => m_attackHitBox;
    public HitCollider HitCollider => m_hitCollider;
    //public float CurrentMoveSpeed
    //{
    //    get
    //    {
    //        float slowMultiplier = 1f - Swamp * (1f - SlowRes);
    //        slowMultiplier = Mathf.Clamp(slowMultiplier, 0.25f, 1f);

    //        float finalSpeed = (Speed * slowMultiplier) - Slow;

    //        return Mathf.Max(0f, finalSpeed);
    //    }
    //}


    public void InitializeSpawn()
    {
        ChangeState(EntityState.Idle);

        m_isCooldownEnd = true;
        m_attackCooldownDuration = 0f;

        if (m_entityHP is EnemyHP hp)
        {
            hp.ResetHP();
        }

        SetCanMove(true);
        SetIsStun(false);
        SetIsInvincible(false);
        AssignDropItem();
    }

    #region UNITY EVENT
    protected override void Awake()
    {
        base.Awake();

        m_spawnPosition = transform.position;

        m_agent = GetComponent<NavMeshAgent>();
        m_itemManager = FindAnyObjectByType<ItemManager>();
        if(m_itemManager == null)
        {
            Debug.LogWarning($"{this.name} : ItemManager Not Found");
        }



        m_enemyBehaviour = GetComponent<IEnemyBehaviour>();
        m_hitCollider = new HitCollider(true);

        if (m_enemyBehaviour != null)
        {
            m_enemyBehaviour.Initialized(this);
        }

        m_agent.updateRotation = false;
        m_agent.updatePosition = true;

        int playerLyer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        Physics.IgnoreLayerCollision(playerLyer, enemyLayer, true);
    }

    private void FixedUpdate()
    {
        UpdateFlag();
        if (HandleStateMovement()) return;

        if (m_target == null) return;

        HandleCooldown();

        float distance = Vector3.Distance(transform.position, m_target.Value);
        HandleRotation(distance);

        if (distance > m_findRange)
        {
            StopAll();
            return;
        }
        m_rb.constraints = RigidbodyConstraints.FreezeRotationY  | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        m_enemyBehaviour.Execute();
    }
    private void OnEnable()
    {
        ChangeState(EntityState.Idle);

        m_isCooldownEnd = true;
        m_attackCooldownDuration = 0f;

        if(m_entityHP is EnemyHP hp)
        {
            hp.ResetHP();
        }
    }
    #endregion

    #region ATTACK
    public bool TryUseCooldown()
    {
        if (!m_isCooldownEnd) return false;

        ConsumeCooldown();

        return true;
    }
    public void ConsumeCooldown()
    {
        m_isCooldownEnd = false;
        m_attackCooldownDuration = 0f;
    }
    private void HandleCooldown()
    {
        if (m_isCooldownEnd) return;

        m_attackCooldownDuration += Time.deltaTime;

        if (m_attackCooldownDuration >= m_attackCooldown)
        {
            m_attackCooldownDuration = 0f;
            m_isCooldownEnd = true;
        }
    }
    public bool TryAttack()
    {
        if (!m_isCooldownEnd) return false;

        if(m_anim != null)
        {
            m_anim.SetTrigger("Attack");

        }

        Attack();
        ConsumeCooldown();
        return true;
    }
    public void Attack()
    {
        if (m_hitCollider == null) return;

        DamageData damage = new DamageData
            {
                Attack = (int)STR,
                CriticalRate = CriticalRate,
                CriticalDamage = CriticalDamage,
                BreakRate = BreakRate,
                Knockback = KnockBack,
                StunDuration = m_data.StunDuration,
                AttackDir = transform.forward,
            };

        m_hitCollider.AttackCollider(damage, Team, m_attackHitBox);
        Debug.Log("EnemyController : Player HIT");
    }
    private AttackItem GetUseItem()
    {
        if (m_attackItems == null || m_attackItems.Count == 0) return null;

        if (m_isRandom)
        {
            return m_attackItems[UnityEngine.Random.Range(0, m_attackItems.Count)];
        }

        AttackItem item = m_attackItems[m_itemIndex];

        m_itemIndex++;

        if (m_itemIndex >= m_attackItems.Count)
        {
            m_itemIndex = 0;
        }

        return item;
    }
    public void UseItem(Vector3 dir)
    {
        AttackItem useData = GetUseItem();

        if (useData == null) return;
        if (useData.attackItem == null)
        {
            Debug.LogError($"{name} attackItem is NULL"); return;
        }

        ItemRecieveData data =
            new ItemRecieveData
            {
                entity = this,
                pos = transform.position,
                dir = dir,
                power = m_power * 8,
                offset = m_shootOffset,
            };

        if (m_anim != null && !string.IsNullOrEmpty(useData.ItemAnimation))
        {
            if (m_anim.parameters.Any(x => x.name == useData.ItemAnimation))
            {
                m_anim.SetTrigger(useData.ItemAnimation);
            }
            else
            {
                Debug.LogWarning($"{name} Animator Parameter Missing : {useData.ItemAnimation}");
            }
        }

        m_itemManager.OnUseItem(useData.attackItem, data);
    }
    #endregion

    #region MOVE

    private bool HandleStateMovement()
    {
        if (CurrentState == EntityState.Dead) return true;

        

        if (IsKnockBack)
        {
            m_agent.ResetPath();
            m_agent.isStopped = true;
            m_agent.Move(m_knockBackVelocity.normalized * (m_knockbackPower * 5f) * Time.fixedDeltaTime);

            return true;
        }

        if (!m_canMove || IsStun)
        {
            m_agent.ResetPath();
            Stop();

            if (m_anim != null) m_anim.SetBool("Move", false);

            return true;
        }

        return false;
    }
    public void InputMove(Vector3 dir, float speed)
    {
        //if (m_currentState == EntityState.Dead) return;
        //if (m_currentState == EntityState.Attack) return;
        //if (!m_canMove || IsStun)
        //{
        //    Stop();
        //    Move(Vector3.zero, 0);
        //    return;
        //}
        //if (IsKnockBack)
        //{
        //    Move(m_knockBackVelocity, m_knockbackPower * 5f);
        //    return;
        //}
        if (dir == Vector3.zero)
        {
            Stop();
            m_agent.Move(Vector3.zero);
            return;
        }

        m_agent.isStopped = false;
        //m_agent.speed = Mathf.Min(speed, CurrentMoveSpeed);
        m_agent.speed = speed;

        m_agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;

        m_agent.avoidancePriority = 50;

        m_agent.Move(dir * m_agent.speed * Time.deltaTime);
    }
    public void SetDestination(Vector3 targetPos, float speed)
    {
        if (m_currentState == EntityState.Dead) return;
        
        if (m_anim != null)
        {
            m_anim.SetBool("Move", !m_agent.isStopped);
        }

        m_agent.isStopped = false;
        //m_agent.speed = Mathf.Min(speed, CurrentMoveSpeed);
        m_agent.speed = speed;
        m_agent.acceleration = speed * 2.5f;
        m_agent.stoppingDistance = m_attackRange;

        m_agent.SetDestination(targetPos);
    }
    #endregion
    #region ROTATE
    private void HandleRotation(float distance)
    {
        if (!m_isRotating) return;
        if (distance > m_findRange) return;

        Enemy_Rush rush = m_enemyBehaviour as Enemy_Rush;
        m_rb.freezeRotation = false;
        if (rush != null && rush.IsRunning)
        {
            Rotate(rush.CurrentDirection);
        }
        else
        {
            Vector3 dir = m_target.Value - transform.position;
            Rotate(dir);
        }
    }
    private void Rotate(Vector3 dir)
    {
        dir.y = 0f;

        if (dir == Vector3.zero) return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
    }
    public void SetEnableRotation(bool state)
    {
        m_isRotating = state;
    }
    #endregion
    #region STOP
    public void Stop()
    {
        m_agent.isStopped = true;
        //m_rb.freezeRotation = true;
        m_rb.constraints = RigidbodyConstraints.FreezeAll;

        if(m_anim != null)
        {
            m_anim.SetBool("Move", !m_agent.isStopped);

        }

    }
    private void StopAll()
    {
        m_enemyBehaviour?.Stop();
        Stop();
    }
    #endregion
    #region POSITION
    public Vector2 GetRandomPosition(float range)
    {
        Vector2 result = new(transform.position.x, transform.position.z);

        for (var i = range; i >= 0; i -= 1)
        {
            Vector3 randomPoint = transform.position + new Vector3((UnityEngine.Random.value * 2 - 1) * range, 0, (UnityEngine.Random.value * 2 - 1) * range);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result.x = hit.position.x;
                result.y = hit.position.z;
                break;
            }
        }

        return result;
    }
    public void TeleportToPosition(Vector2 position)
    {
        Vector3 origin = transform.position;
        origin.x = position.x;
        origin.z = position.y;
        m_agent.Warp(origin);
    }
    #endregion
    #region DEAD 

    public void OnDead(bool isDropItem = true)
    {
        if (isDropItem)
        {
            ItemDrop();
        }
    }

    public void ItemDrop()
    {
        if (m_dropItem == null) return;
        m_itemManager.DropItemSetData(transform.position, m_dropItem);
    }
    public void ReturnPool()
    {
        ReturnObjectToPool pool = GetComponent<ReturnObjectToPool>();
        if(pool == null)
        {
            Debug.LogWarning($"{this.name} : ReturnObjectToPool Not Found");
        }
        pool.ReturnToPool();
    }
    #endregion

    #region ENEMY

    public static EnemyController SpawnEnemy( Enum_EnemyType type, Vector3 position)
    {
        Middleman_Enemy pool = FindAnyObjectByType<Middleman_Enemy>();
        if (pool == null)
        {
            Debug.LogWarning("Middleman_Enemy Not Found"); return null;
        }

        EnemyController enemy = pool.GetComponent(type);

        if (enemy == null)
        {
            Debug.LogWarning($"Pool Missing : {type}"); return null;
        }

        enemy.ChangeState(Entity.EntityState.Idle);
        enemy.transform.position = position;
        enemy.gameObject.SetActive(true);
        enemy.InitializeSpawn();
        enemy.AssignDropItem();

        return enemy;
    }
    public void AssignDropItem()
    {
        if (m_itemManager == null) return;
        if (m_itemDropGachaEngine == null) return;

        RarityEnumAsset rarity = m_itemDropGachaEngine.Collapse();
        Item item = m_itemManager.DropItem(rarity);
        m_dropItem = item;
    }
    #endregion
}
public interface IEnemyBehaviour
{
    void Initialized(EnemyController Controller);
    void Execute();
    void Stop();
}