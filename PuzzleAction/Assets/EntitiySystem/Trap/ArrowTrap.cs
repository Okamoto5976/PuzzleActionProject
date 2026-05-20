using UnityEngine;

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

        //Normalize方向
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
        // 自分無視
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

        //DamageData作成
        DamageData damageData =
          new DamageData();

        //攻撃者
        damageData.Attacker =
            m_owner;

        //ダメージ
        damageData.Damage =
          m_data.damage;

        //Hit位置
        damageData.HitPoint =
            transform.position;

        //ダメージ
        target.TakeDamage(damageData.Damage);

        Debug.Log(other.name + "Hit");

        //矢を消す
        Destroy(gameObject);
        //のちにpull
    }
}
