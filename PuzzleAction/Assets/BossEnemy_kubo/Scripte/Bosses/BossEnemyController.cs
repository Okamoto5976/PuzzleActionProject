using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossEnemyController : Entity
{
    [Header("Target")]
    [SerializeField] private Vector3Asset m_target;

    [Header("Range")]
    [SerializeField] private float m_findRange = 15f;
    [SerializeField] private float m_attackRange = 3f;

    [Header("Attack")]
    [SerializeField] private float m_attackCooldown = 3f;

    [Header("Drop")]
    [SerializeField] private GachaEngine m_itemDropGachaEngine;
    public GachaEngine ItemDropGachaEngine => m_itemDropGachaEngine;
    [SerializeField] private int m_dropCount = 3;

    [Header("Ref")]
    [SerializeField] private AttackHitBox m_attackHitBox;
    private HitCollider m_hitCollider;

    [Header("Item")]
    [SerializeField] private Item m_attackItem;
    [SerializeField] private float m_power = 24f;
    [SerializeField] private Vector3 m_shootOffset = new Vector3(0f, 0.5f, 0f);
    #region UnityEditor
    #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (m_attackItem == null) return;

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
    private List<Item> m_dropItems = new();

    private float m_cooldownTimer;
    private bool m_isCooldownReady = true;
    private bool m_isRotating = true;

    private NavMeshAgent m_agent;
    private IBossBehaviour m_bossBehaviour;
    private ReturnObjectToPool m_returnPool;

    public bool CanAction => CurrentState != EntityState.Dead && !IsStun;
    public float FindRange => m_findRange;
    public float AttackRange => m_attackRange;
    public bool IsCooldownReady => m_isCooldownReady;
    public float ShootPower => m_power;
    public Vector3 SpawnPosition { get; private set; }
    public NavMeshAgent Agent => m_agent;
    public Vector3Asset Target => m_target;
    public Vector3 Forward => transform.forward;

    public void InitializeSpawn()
    {
        ChangeState(EntityState.Idle);

        m_isCooldownReady = true;

        if (m_entityHP is EnemyHP hp)
        {
            hp.ResetHP();
        }

        SpawnPosition = transform.position;

        m_agent.ResetPath();
        m_agent.isStopped = false;

        m_knockBackVelocity = Vector3.zero;

        m_rb.linearVelocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;

        m_rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        SetCanMove(true);
        SetIsStun(false);
        SetIsInvincible(false);

        AssignDropItem();
    }
    #region UNITY EVENT
    protected override void Awake()
    {
        base.Awake();

        SpawnPosition = transform.position;

        m_agent = GetComponent<NavMeshAgent>();
        m_hitCollider = new HitCollider(true);
        m_bossBehaviour = GetComponent<IBossBehaviour>();
        m_returnPool = GetComponent<ReturnObjectToPool>();
        m_itemManager = FindAnyObjectByType<ItemManager>();

        m_attackHitBox.m_pos = gameObject.transform.position;

        //if (m_attackItem == null)
        //{
        //    Debug.LogWarning($"{name} AttackItem Missing");
        //}

        if (m_itemManager == null)
        {
            Debug.LogWarning($"{name} ItemManager Missing");
        }

        if (m_bossBehaviour != null)
        {
            m_bossBehaviour.Initialize(this);
        }

        m_agent.updateRotation = false;
        m_agent.updatePosition = true;
    }

    private void Update()
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

        m_bossBehaviour?.Execute();
    }
    private void OnEnable()
    {
        ChangeState(EntityState.Idle);

        m_isCooldownReady = true;
        m_cooldownTimer = 0f;

        if (m_entityHP is EntityHP hp)
        {
            hp.ResetHP();
        }
    }
    #endregion

    #region ATTACK
    public bool TryUseCooldown()
    {
        if (!m_isCooldownReady) return false;

        ConsumeCooldown();
        return true;
    }
    public void ConsumeCooldown()
    {
        m_isCooldownReady = false;
        m_cooldownTimer = 0f;
    }
    private void HandleCooldown()
    {
        if (m_isCooldownReady) return;

        m_cooldownTimer += Time.deltaTime;

        if (m_cooldownTimer >= m_attackCooldown)
        {
            m_isCooldownReady = true;
            m_cooldownTimer = 0f;
        }
    }
    public bool TryAttack()
    {
        if (CurrentState == Entity.EntityState.Dead) return false;
        if (!m_isCooldownReady) return false;
        if (m_anim != null)
        {
            m_anim.SetTrigger("Attack");

        }
        Attack();
        ConsumeCooldown();
        return true;
    }
    public void Attack()
    {
        Debug.DrawLine(transform.position, m_attackHitBox.m_pos, Color.red, 2f);
        Debug.Log(Vector3.Distance(m_attackHitBox.m_pos, m_target.Value));
        Debug.Log(m_attackHitBox.m_pos);
        Debug.Log(m_attackHitBox.m_radius);


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
        m_attackHitBox.m_pos = transform.position;

        var hits = m_hitCollider.AttackCollider(damage, Team, m_attackHitBox);
        DebugViewCollider.Instance.ViewHitCollider(m_attackHitBox);

        foreach (Collider hit in hits)
        {
            Entity entity = hit.GetComponentInParent<Entity>();
            if (entity == null)
            {
                continue;
            }

            if (entity.Team == Team) continue;

            entity.TakeDamage(damage);
        }

        Debug.Log("BossEnemyController : Player ‚ÉHIT");
    }
    public void UseItem(Vector3 dir)
    {
        ItemRecieveData data = new ItemRecieveData
        {
            entity = this,
            pos = transform.position,
            dir = dir, 
            power = m_power, 
            offset = m_shootOffset,
            
        };

        m_itemManager.OnUseItem(m_attackItem, data);
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

            m_agent.Move(m_knockBackVelocity.normalized * (m_knockbackPower * 5f) * Time.deltaTime);

            return true;
        }

        if (!m_canMove || IsStun)
        {
            m_agent.ResetPath();
            Stop();
            if (m_anim != null)
            {
                m_anim.SetBool("Move", false);
            }
            return true;
        }

        return false;
    }
    public void InputMove(Vector3 dir, float speed)
    {
        if (dir == Vector3.zero)
        {
            Stop();

            m_agent.Move(Vector3.zero);
            return;
        }

        m_agent.isStopped = false;
        m_agent.speed = speed;

        m_agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        m_agent.avoidancePriority = 50;
        m_agent.Move(dir * m_agent.speed * Time.deltaTime);
    }
    public void SetDestination(Vector3 pos, float speed)
    {
        if (CurrentState == EntityState.Dead) return;

        if (m_anim != null)
        {
            m_anim.SetBool("Move", !m_agent.isStopped);
        }

        m_agent.isStopped = false;

        m_agent.speed = speed;
        m_agent.acceleration = speed * 2.5f;
        m_agent.stoppingDistance = m_attackRange;

        m_agent.SetDestination(pos);
    }
    #endregion

    #region ROTATE
    private void HandleRotation(float distance)
    {
        if (!m_isRotating) return;
        if (distance > m_findRange) return;

        Vector3 dir = m_target.Value - transform.position;
        Rotate(dir);
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

        if (m_anim != null)
        {
            m_anim.SetBool("Move", false);
        }
    }
    private void StopAll()
    {
        Stop();
        m_bossBehaviour?.Stop();
    }
    #endregion

    #region POSITION
    public Vector2 GetRandomPosition(float range)
    {
        Vector2 result = new Vector2(transform.position.x, transform.position.z);
        for (float i = range; i >= 0; i -= 1f)
        {
            Vector3 randomPoint = transform.position + new Vector3((UnityEngine.Random.value * 2 - 1) * range, 0, (UnityEngine.Random.value * 2 - 1) * range);
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1f, NavMesh.AllAreas))
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
    public virtual void OnDead(bool isItemDrop)
    {
        GameManager.Instance.SetKey();

        if(isItemDrop)
        {
            DropItems();
        }
    }
    private void DropItems()
    {
        if (m_itemManager == null) return;

        foreach (Item item in m_dropItems)
        {
            if (item == null) continue;

            Vector3 pos = transform.position + UnityEngine.Random.insideUnitSphere;
            pos.y = transform.position.y;
            m_itemManager.DropItemSetData(pos, item);
        }
    }
    #endregion

    #region BOSS
    public static EnemyController SpawnEnemy(Enum_EnemyType type, Vector3 position)
    {
        Middleman_Enemy pool = FindAnyObjectByType<Middleman_Enemy>();
        if (pool == null)
        {
            Debug.LogWarning("Middleman_Enemy Not Found");
            return null;
        }
        //get enemy flom pool
        EnemyController enemy = pool.GetComponent(type);
        if (enemy == null)
        {
            Debug.LogWarning($"Pool Missing : {type}");
            return null;
        }

        //set enemy info
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

        m_dropItems.Clear();

        for (int i = 0; i < m_dropCount; i++)
        {
            RarityEnumAsset rarity = m_itemDropGachaEngine.Collapse();
            Item item = m_itemManager.DropItem(rarity);
            if (item != null)
            {
                m_dropItems.Add(item);
            }
        }
    }
    #endregion
}

public interface IBossBehaviour
{
    void Initialize(BossEnemyController controller);
    void Execute();
    void Stop();
}