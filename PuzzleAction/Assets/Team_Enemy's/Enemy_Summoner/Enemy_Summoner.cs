using UnityEngine;

public class Enemy_Summoner : MonoBehaviour, IEnemyBehaviour
{
    [SerializeField] private MapGeneration m_mapGeneration;
    private EnemyController m_enemyController;
    public void Initialized(EnemyController enemyController) => m_enemyController = enemyController;
    public void Execute()
    {

        if (m_enemyController.Target == null) return;
        float distance = Vector3.Distance(transform.position, m_enemyController.Target.Value);
        if (distance <= m_enemyController.AttackRange)
        {



            m_enemyController.Stop();
            Vector3 Radise = transform.position;
            float radius = Random.Range(0, m_enemyController.AttackRange);
            float angle =  Random.Range(0, 360);



            if (m_enemyController.AttackRange > radius)
            {
                for (int i = 0; i < 2; i++)
                {
                    
                    Vector3 Summon_pos = transform.position + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

                    //壁がある場合
                    if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, radius))
                    {
                        //hitしたObjectが m_mapGeneration の (SouthWall || WestWall) && Active(true)
                        //if(hit.collider == m_mapGeneration.SouthWall && Active(true))
                        {
                            //壁がある場合は召喚位置を変更する
                            //Summon_pos = 召喚者の位置から壁までの距離(Mathf.Max(召喚者の位置, hitした壁の位置) - Mathf.Min(召喚者の位置, hitした壁の位置))　＆＆　AttackRange内にSpawn
                            
                            //Summon_pos = m_enemyController.AttackRange > ( Mathf.Max(Radise,hit.collider)-Mathf.Min(Radise,hit.collider));

                        }
                    }
                    //Enum_EnemtType type;
                    int test = Random.Range(0, 2);// 0or1
                    if (test == 0)
                    {
                        //type = Enum_EnemtType.Enemy_Rush;
                    }
                    else if (test == 1)
                    {
                        //type = Enum_EnemtType.Enemy_Chase;
                    }
                    //エネミーを召喚する処理
                    //m_enemyController.(type,Summon_pos);
                    

                }
            }

            return;

        }
        m_enemyController.SetDestination(m_enemyController.Target.Value, m_enemyController.Speed);
    }

    public void Stop() => m_enemyController.Stop();
}
