using UnityEngine;

<<<<<<< HEAD
public class ArrowTrap : MonoBehaviour
{
    [Header("Data")]

    [SerializeField]
    private ArrowData m_data;

    //使用方法
    private Vector3 m_direction;

    //初期位置
    private Vector3 m_startPosition;

    //Hit判定
    private GameObject m_owner;

    public void Initialize(ArrowUseData data)
    {
        //出現位置
        transform.position = data.Position;

        //Normalize方向(正規化)
        m_direction = data.Direction.normalized;

        //初期位置保存
        m_startPosition = transform.position;

        //使用者
        m_owner = data.Owner;
    }

    private void Update()
    {
        Move();

        CheckRange();
    }

    private void Move()
    {
        transform.position += 
            m_direction * 
            m_data.speed * 
            Time.deltaTime;
    }

    private void CheckRange()
    {
        float distance=
            Vector3.Distance(
                m_startPosition,
                transform.position 
                );

        if(distance >= m_data.range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject == m_owner)
        {
            return;
        }

        // Entity取得
        Entity target =
            other.GetComponent<Entity>();

        Entity owner =
          m_owner.GetComponent<Entity>();


        //Entitiyなし
        if (target == null || owner == null)
        {
            return;
        }

        // 同チーム無効
        if (target.Team == owner.Team)
        {
            // Nature例外
            if (
                target.Team != Entity.Teamtype.Nature
                )
            {
                return;
            }
        }

        //DamageData
        DamageData damageData =
          new DamageData();

        //Attacker
        damageData.Attacker =
            m_owner;

        //Damage
        damageData.Damage =
          m_data.damage;

        //HitPosition
        damageData.HitPoint =
            transform.position;

        //targetDamage
        target.TakeDamage(damageData.Damage);

        Debug.Log(other.name + "Hit");

        //矢を消す
        Destroy(gameObject);
        //のちにpull
=======
public class ArrowTrap : TrapBase
{
    [Header("ArrowData")]
    [SerializeField]
    private TrapData m_trapdata;

    public void Init(Entity entity, Vector3 dir, int baseValue)
    {
        m_direction = dir.normalized;
        transform.rotation = Quaternion.LookRotation(dir);
        m_owner = entity;

        m_basevalue = baseValue;
    }

    public void Initialize(TrapUseData data)
    {
        //position
        transform.position = data.Position;

        //Normalizedirection
        m_direction = data.Direction.normalized;
        transform.rotation = Quaternion.LookRotation(m_direction);

        //startposition
        m_startPosition = transform.position;

        //user
        //m_owner = data.Owner;

        //BaseValue
        m_basevalue = data.BaseValue;

        //Data
        m_range = m_trapdata.range;

        //damage
        //m_damage = m_trapdata.damage + m_basevalue;
>>>>>>> EntitySystem
    }
}
