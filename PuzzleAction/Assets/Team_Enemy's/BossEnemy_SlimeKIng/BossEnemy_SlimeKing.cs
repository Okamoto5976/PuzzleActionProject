using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum SlimeKingAttackType{
    Attack,
    Summon

}
public class BossEnemy_SlimeKing : MonoBehaviour//, IBossBehaviour
{
    [Header("Summon Setting")]
    [SerializeField] private List<Enum_EnemyType> m_summonTypes = new();
    [SerializeField] private float SlimeKingRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float findRange; 
    private EnemyController m_enemyController;

    private BossEnemyController m_controller;
    public void Initialized(EnemyController enemyController)
    {
        m_enemyController = enemyController;
    }
    public void Execute()
    {
        if (m_enemyController.Target == null) return;
        float distance = Vector3.Distance(transform.position, m_enemyController.Target.Value);
        SlimeKingAttackType attackType;
        if (distance <= SlimeKingRange)
        {
            attackType = SlimeKingAttackType.Attack ;
        }
        else
        {
            attackType = SlimeKingAttackType.Summon;
        }
        m_controller.StartAttack();

        m_enemyController.Stop();
        switch (attackType)
        {
            case SlimeKingAttackType.Attack:

                AttackRange(distance);
                break;

            case SlimeKingAttackType.Summon:

                FindRange(distance);
                break;
         
        }
    }


    private void SummonEnemies()
    {
        foreach (Enum_EnemyType enmeyType in m_summonTypes)
        {
            if (!TryGetSummonPosition(out Vector3 summonPos))
            {
                Debug.LogWarning("Summon Position Not Found");
                continue;
            }
            //EnemyController enemy = EnemyController.SpawnEnemy(enemyType, summonPos);
            //if (enemy == null) continue;
        }
    }
    private bool TryGetSummonPosition(out Vector3 result)
    {
        result = transform.position;
        const int maxTry = 10;
        for (int i = 0; i < maxTry; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = Random.Range(1f, m_enemyController.AttackRange);
            Vector3 candidate = transform.position + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            if (!NavMesh.SamplePosition(candidate, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                continue;
            }
            candidate = hit.position;
            Vector3 dir = candidate - transform.position;
            float dist = dir.magnitude;

            dir.Normalize();
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, dist)) ;
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
    public void AttackRange(float distance)
    {
        if (distance > m_enemyController.AttackRange)
        {
            m_enemyController.TryAttack();
            Stop();
            return;
        }
    }
    public void FindRange(float distance)
    {
        if (distance > m_enemyController.FindRange)
        {
            m_enemyController.SetDestination(m_enemyController.Target.Value, m_enemyController.Speed);
            if (!m_enemyController.TryUseCooldown()) return;
            SummonEnemies();
        }
    }
  
    
}



