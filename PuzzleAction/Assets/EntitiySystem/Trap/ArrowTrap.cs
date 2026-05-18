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

        //Hit判定
        m_owner = data.Owner;
    }

    private void Update()
    {
        Move();

        CheckRange();
    }

    private void Move()
    {
        transform.position += m_direction * m_data.speed * Time.deltaTime;
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
        //自分無視
        if(other.gameObject == m_owner)
        {
            return;
        }

        //Debug.Log(other.name + "にHit");

        //HP取得
        //EnemyHP hP=
        //    other.GetComponent<EnemyHP>();

        //if(hP != null) 
        //{
        //    hP.TakeDamage(10);
        //}
        
        //Team取得
        TestTeam targetTeam =
            other.GetComponent<TestTeam>();

        TestTeam ownerTeam=
            m_owner.GetComponent<TestTeam>();

        //Teamある
        if(
            targetTeam != null &&
            ownerTeam != null
            ) 
        {
            //同じTeam   
            if(
                targetTeam.Team ==
                ownerTeam.Team
                )

            {
                Debug.Log("同じチーム");

                return;
            }

            //Nature同士
            if(
                targetTeam.Team ==
                TestTeam.TeamType.Nature &&
                ownerTeam.Team ==
                TestTeam.TeamType.Nature
                )
            {
                Debug.Log("Nature同士HIt");
            }
        }

        Debug.Log(other.name + "Hit");

        //矢を消す
        Destroy(gameObject);
        //のちにpull
    }
}
