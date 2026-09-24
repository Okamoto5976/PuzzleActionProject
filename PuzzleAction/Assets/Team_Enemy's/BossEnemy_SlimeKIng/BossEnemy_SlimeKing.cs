using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy_SlimeKing : MonoBehaviour, IBossBehaviour
{
    [SerializeField]
    private SlimeKingController m_slimeKing;

    private BossEnemyController m_bossEnemyController;

    private readonly List<EnemyController> m_children = new();

    public void Initialize(BossEnemyController controller)
    {
        m_bossEnemyController = controller;
    }

    public void Execute()
    {
        if (m_bossEnemyController == null) return;
        if (m_bossEnemyController.Target == null) return;

        float distance = Vector3.Distance(transform.position, m_bossEnemyController.Target.Value);
        if (distance <= m_bossEnemyController.AttackRange)
        {
            CloseAttack();
            return;
        }
        if (distance > m_bossEnemyController.FindRange)
        {
            return;
        }

        m_bossEnemyController.SetDestination(m_bossEnemyController.Target.Value, m_bossEnemyController.Speed);

        if (m_bossEnemyController.TryUseCooldown())
        {
            Debug.Log("Summon Start");

            SummonEnemies();
        }
    }

    private void CloseAttack()
    {
        m_bossEnemyController.Stop();
        m_bossEnemyController.TryAttack();
    }

    private void SummonEnemies()
    {
        int aliveCount = GetAliveCount();

        int remain = m_slimeKing.MaxAliveSummons - aliveCount;
        if (remain <= 0)
        {
            return;
        }

        int summonCount = Mathf.Min(m_slimeKing.SummonCount, remain);

        Debug.Log($"Summon Count = {summonCount}");

        for (int i = 0; i < summonCount; i++)
        {
            if (!TryGetSummonPosition(out Vector3 summonPos))
            {
                Debug.LogWarning("Summon Position Not Found");

                continue;
            }

            int index = Random.Range(0, m_slimeKing.SummonTypes.Count);
            Enum_EnemyType type = m_slimeKing.SummonTypes[index];
            EnemyController enemy = BossEnemyController.SpawnEnemy(type, summonPos);

            if (enemy == null)
            {
                Debug.LogWarning($"Spawn Failed : {type}");
                continue;
            }

            //enemy.transform.SetParent(transform);
            m_children.Add(enemy);

            Debug.Log($"Summon Success : {enemy.name}");
        }
    }

    private int GetAliveCount()
    {
        m_children.RemoveAll(x => x == null || !x.gameObject.activeSelf);
        return m_children.Count;
    }
    private bool TryGetSummonPosition(out Vector3 result)
    {
        Vector2 pos = m_bossEnemyController.GetRandomPosition(m_slimeKing.SummonRadius);
        result = new Vector3(pos.x, transform.position.y, pos.y);
        return true;
    }

    public void Stop()
    {
        m_bossEnemyController.Stop();
    }
}