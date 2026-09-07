using System.Collections.Generic;
using UnityEngine;

public class Poolinstallationpulling : MonoBehaviour
{
    [SerializeField] private Middleman_Enemy m_pool;

    [SerializeField] private List<Enum_EnemyType> m_enemyTypes = new()
    {
       Enum_EnemyType.Archer,
       Enum_EnemyType.Chase,
       Enum_EnemyType.Rush,
    };

    /// <summary>
    /// 渡された複数の世界座標に、プールからランダムな敵を配置する
    /// Place random enemies from a pool at multiple given world coordinates.
    /// </summary>
    public void SpawnEnemiesAtPositions(List<Vector3> spawnPositions)
    {
        if (m_pool == null) return;

        foreach (Vector3 pos in spawnPositions)
        {
            //random selection
            int roulette = Random.Range(0, m_enemyTypes.Count);
            Enum_EnemyType selectedType = m_enemyTypes[roulette];

            //***
            if(m_pool.gameObject != null)
            {
                m_pool.gameObject.SendMessage("Awake", SendMessageOptions.DontRequireReceiver);

                foreach(Transform child in m_pool.transform)
                {
                    child.gameObject.SendMessage("Awake", SendMessageOptions.DontRequireReceiver);
                }
            }

            //Pool acquisition
            DummyEnemyScript enemy = m_pool.GetEnemy(selectedType);

            if (enemy == null)
            {
                Debug.LogWarning("プールからselectedTypeを取得できませんでした");
                continue;
            }
            enemy.gameObject.transform.position = pos;
            enemy.gameObject.SetActive(true);
            
        }
    }
}
