using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Summoner : MonoBehaviour, IEnemyBehaviour
{
    [Header("Summon Setting")]
    [SerializeField] private List<Enum_EnemyType> m_summonTypes = new();

    private EnemyController m_enemyController;

    public void Initialized(EnemyController enemyController)
    {
        m_enemyController = enemyController;
    }

    public void Execute()
    {
        if (m_enemyController.Target == null) return;

        float distance = Vector3.Distance(transform.position, m_enemyController.Target.Value);

        // distination
        if (distance > m_enemyController.AttackRange)
        {
            m_enemyController.SetDestination(m_enemyController.Target.Value, m_enemyController.Speed);
            return;
        }

        m_enemyController.Stop();

        if (!m_enemyController.TryUseCooldown()) return;

        SummonEnemies();
    }

    private void SummonEnemies()
    {
        foreach (Enum_EnemyType enemyType in m_summonTypes)
        {
            if (!TryGetSummonPosition(out Vector3 summonPos))
            {
                Debug.LogWarning("Summon Position Not Found");
                continue;
            }

            //summon enemy
            EnemyController enemy = EnemyController.SpawnEnemy(enemyType, summonPos);

            if (enemy == null) continue;
        }
    }

    private bool TryGetSummonPosition(out Vector3 result)
    {
        result = transform.position;

        const int maxTry = 20;

        for (int i = 0; i < maxTry; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);

            float radius = Random.Range(1f, m_enemyController.AttackRange);

            Vector3 candidate = transform.position + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);

            // navmesh de position hosei 
            if (!NavMesh.SamplePosition(candidate, out NavMeshHit hit, 1f, NavMesh.AllAreas)) 
            {
                continue;
            }

            //can summon position 
            candidate = hit.position; 
            Vector3 dir = candidate - transform.position;
            float dist = dir.magnitude;

            dir.Normalize();

            // wall check
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, dist))
            {
                continue;
            }

            result = candidate;
            return true;
        }

        return false;
    }

    public void Stop()
    {
        m_enemyController.Stop();
    }
}