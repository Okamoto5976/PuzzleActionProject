using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Explosion : MonoBehaviour,IEnemyBehaviour
{
    private EnemyController m_enemyController;

    [SerializeField] private AudioData m_explodeSE;

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
                Particle();
                AudioManager.Instance.PlayAudio(m_explodeSE);
                //m_enemyController.ReturnPool();
            }

            return;
        }
        m_enemyController.SetDestination(m_enemyController.Target.Value, m_enemyController.Speed);
    }
    public void Stop() => m_enemyController.Stop();

    private void Particle()
    {
        var pos = transform.position + new Vector3(0f, 0.5f, 0f);

        ParticleManager.Instance.PlayParticle(Enum_EffectType.Explosion, pos);
    }
}
