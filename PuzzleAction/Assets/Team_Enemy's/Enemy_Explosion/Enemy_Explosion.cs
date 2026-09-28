using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Explosion : MonoBehaviour,IEnemyBehaviour
{
    private EnemyController m_enemyController;

    [SerializeField] private EffectEventDataSO m_effectEventData;

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
                //m_explosionParticle.Play();
                Effect data = new Effect()
                {
                    effectType = Enum_EffectType.Explosion,
                    effectPos = transform.position + new Vector3(0f, 0.5f, 0f),
                    effectRot = transform.rotation,
                };

                m_effectEventData.Raise(data);

                m_enemyController.ReturnPool();
            }

            return;
        }
        m_enemyController.SetDestination(m_enemyController.Target.Value, m_enemyController.Speed);
    }
    public void Stop() => m_enemyController.Stop();
}
