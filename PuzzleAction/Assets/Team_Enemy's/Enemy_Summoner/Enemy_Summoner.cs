using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class Enemy_Summoner : MonoBehaviour, IEnemyBehaviour
{
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
            //Vector3 Summon_pos =  angle * radius;
            if (m_enemyController.AttackRange > radius)
            {
                //壁がある場合
                //エネミーを召喚する処理



            }

            return;

        }
        m_enemyController.SetDestination(m_enemyController.Target.Value, m_enemyController.Speed);
    }

    public void Stop() => m_enemyController.Stop();
}
