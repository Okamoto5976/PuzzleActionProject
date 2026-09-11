using UnityEngine;

//[RequireComponent(typeof(BossSlimeKingeAttack))]
public class BossEnemy_SlimeKing : MonoBehaviour//, IBossBehaviour
{
    //[Header("Attack Range")]
    //[SerializeField] private float m_shortRange = 4f;
    //[SerializeField] private float m_longRange = 12f;

    private BossEnemyController m_controller;
    //public void Initialized(EnemyController enemyController) => m_enemyController = enemyController;
    public void Execute()
    {
        if (m_controller.Target == null) return;
        float distance = Vector3.Distance(transform.position, m_controller.Target.Value);
        //Attack

        //if(distance> BossEnemyController.m_AttackRange) 
        //{
        //int timerCount=0;
        //if(timerCount < 20f)
        //{
        //if(Distance <= m_shortRange)
        //{
        // BossEnemyController.tryAttack();
        //}
        //}
        //else
        //{
        //   BossEnemyController.Rush();
        //}
        //m_BossenemyController.SetDestination(m_BossenemyController.Target.Value, m_BossenemyController.Speed);
        //else if(timerCount > 20f && Distance <= BossEnemyController.AttackRange)
        //{
        //    for (int i = 0; i < 6; i++)
        //    {
        //          Vector3 Summon_pos = transform.position + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

        //壁がある場合
        //          if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, radius))
        //          {        
        //              if(hit.collider == m_mapGeneration.SouthWall && Active(true))
        //              {
        //                  //壁がある場合は召喚位置を変更する
        //                  //Summon_pos = 召喚者の位置から壁までの距離(Mathf.Max(召喚者の位置, hitした壁の位置) - Mathf.Min(召喚者の位置, hitした壁の位置))　＆＆　AttackRange内にSpawn
        //                  //Summon_pos = m_enemyController.AttackRange > ( Mathf.Max(Radise,hit.collider)-Mathf.Min(Radise,hit.collider));
        //               }
        //           }
        //    //Enum_EnemtType type;
        //    int test = Random.Range(0, 2);// 0or1
        //    if (test == 0)
        //    {
        //        //type = Enum_EnemtType.Enemy_Slime_Red;
        //    }
        //    else if (test == 1)
        //    {
        //        //type = Enum_EnemtType.Enemy_Slime_Blue;
        //    }
        //    //エネミーを召喚する処理
        //    //m_enemyController.(type,Summon_pos);
        //    }
        //}

    }
    public void Stop() => m_controller.Stop();
}
