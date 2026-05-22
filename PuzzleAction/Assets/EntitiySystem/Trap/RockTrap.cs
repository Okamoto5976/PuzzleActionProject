using UnityEngine;

public class RockTrap : MonoBehaviour
{
    [Header("Data")]
    [SerializeField]
    private RockData m_data;

    //方向
    private Vector3 m_direction;

    //開始位置
    private Vector3 m_startPostion;

    //使用者
    private GameObject m_owner;

    //トラップ生成の初期化管理
    public void Initialize(RockUseData data)
    {
        transform.position = data.Position;

        m_direction = data.Direction.normalized;

        m_startPostion=transform.position;

        m_owner=data.Owner;
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
            m_data.Speed *
            Time.deltaTime;
    }

    private void CheckRange()
    {
        float distance =
            Vector3.Distance(
                m_startPostion,
                transform.position
                );
        if(distance >= m_data.range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject==m_owner) 
        {
            return;
        }

        //Entitiy取得
        Entity target=
            other.GetComponent<Entity>();

        Entity owner=
            m_owner.GetComponent<Entity>();

        //Entityなし
        if(target==null||owner==null)
        {
            return;
        }

        //同Team無効
        if (target.Team == owner.Team)
        {   
            //Nature例外
            if(target.Team != Entity.Teamtype.Nature)
            {
                return;
            }
        }
        //ダメージ
        target.TakeDamage(m_data.damage);

        Debug.Log(other.name +" に岩Hit");

        //岩を消す
        Destroy(gameObject);
        //のちにPull
    }    
}
