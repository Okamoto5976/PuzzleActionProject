using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Explosion : MonoBehaviour,IEnemyBehaviour
{
    private EnemyController m_enemyController;

    public void Initialized(EnemyController enemyController)=>m_enemyController=enemyController;

    public void Execute()
    {
        if (m_enemyController.Target == null) return;
        float distance = Vector3.Distance(transform.position, m_enemyController.Target.Value);
        if (distance <= m_enemyController.AttackRange)
        {
            m_enemyController.Stop();

            if (m_enemyController.TryAttack())
            {
                Die();
            }

            return;
        }
        m_enemyController.SetDestination(m_enemyController.Target.Value, m_enemyController.Speed);
    }

    private void Die()
    {
        //pool‚É•ÏX‚·‚é
        Destroy(gameObject);
    }

    public void Stop() => m_enemyController.Stop();
}
