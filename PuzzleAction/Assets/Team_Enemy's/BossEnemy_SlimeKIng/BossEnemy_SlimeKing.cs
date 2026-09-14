using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy_SlimeKing : MonoBehaviour, IBossBehaviour
{
    [SerializeField]
    private SlimeKingController m_slimeKing;

    private BossEnemyController m_controller;

    private readonly List<EnemyController> m_children = new();

    public void Initialize(BossEnemyController controller)
    {
        m_controller = controller;
    }

    public void Execute()
    {
        if (m_controller == null) return;
        if (m_controller.Target == null) return;

        float distance = Vector3.Distance(transform.position, m_controller.Target.Value);
        if (distance <= m_controller.AttackRange)
        {
            CloseAttack();
            return;
        }
        if (distance > m_controller.FindRange)
        {
            return;
        }

        m_controller.SetDestination(m_controller.Target.Value, m_controller.Speed);

        if (m_controller.TryUseCooldown())
        {
            Debug.Log("Summon Start");

            SummonEnemies();
        }
    }

    private void CloseAttack()
    {
        m_controller.Stop();
        m_controller.TryAttack();
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
        Vector2 pos = m_controller.GetRandomPosition(m_slimeKing.SummonRadius);
        result = new Vector3(pos.x, transform.position.y, pos.y);
        return true;
    }

    public void Stop()
    {
        m_controller.Stop();
    }
}