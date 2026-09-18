using System.Collections.Generic;
using UnityEngine;

public class Enemy_Summoner : MonoBehaviour, IEnemyBehaviour
{
    [Header("Summon")]
    [SerializeField] private List<Enum_EnemyType> m_summonTypes = new();

    [SerializeField] private int m_summonCount = 2;
    [SerializeField] private int m_maxAliveSummons = 6;

    private EnemyController m_enemyController;
    private readonly List<EnemyController> m_children = new();

    public void Initialized(EnemyController enemyController)
    {
        m_enemyController = enemyController;
    }

    public void Execute()
    {
        if (m_enemyController == null) return;
        if (m_enemyController.Target == null) return;

        float distance = Vector3.Distance(transform.position, m_enemyController.Target.Value);

        //distination
        if (distance > m_enemyController.AttackRange)
        {
            m_enemyController.SetDestination(m_enemyController.Target.Value, m_enemyController.Speed);
            return;
        }

        m_enemyController.Stop();

        if (!m_enemyController.TryUseCooldown()) return;

        Debug.Log($"{name} Summon Start");

        SummonEnemies();
    }

    private void SummonEnemies()
    {
        int aliveCount = GetAliveCount();
        int remain = m_maxAliveSummons - aliveCount;
        if (remain <= 0) return;


        int summonCount = Mathf.Min(m_summonCount, remain);
        for (int i = 0; i < summonCount; i++)
        {
            if (!TryGetSummonPosition(out Vector3 summonPos))
            {
                Debug.LogWarning("Summon Position Not Found");
                continue;
            }

            if (m_summonTypes.Count == 0)
            {
                Debug.LogWarning("Summon Types Empty");
                return;
            }

            int index = Random.Range(0, m_summonTypes.Count);
            Enum_EnemyType enemyType = m_summonTypes[index];

            //summon enemy
            EnemyController enemy = EnemyController.SpawnEnemy(enemyType, summonPos);
            if (enemy == null)  continue;
            m_children.Add(enemy);
        }
    }

    private bool TryGetSummonPosition(out Vector3 result)
    {
        Vector2 pos = m_enemyController.GetRandomPosition(m_enemyController.AttackRange);
        result = new Vector3(pos.x, transform.position.y, pos.y);
        return true;
    }

    private int GetAliveCount()
    {
        m_children.RemoveAll(x => x == null ||  !x.gameObject.activeSelf);
        return m_children.Count;
    }

    public void Stop()
    {
        m_enemyController.Stop();
    }
}
