using System.Collections.Generic;
using UnityEngine;

public class Poolinstallationbypulling : MonoBehaviour
{
    [SerializeField] private Middleman_Enemy m_pool;

    private List<Enum_EnemyType> m_enemyTypes = new()
    {
    Enum_EnemyType.Archer,
    Enum_EnemyType.Rush,
    Enum_EnemyType.Chase,
    //Enum_EnemyType.OOOOO,
    };
    ///<summary>
    ///This will place randomly selected enemies at multiple specified world coordinates.
    ///渡された複数の世界座標に、プールからランダムな敵を配置する
    /// </summary>
    public void SpawnEnemiesAtPositions(List<Vector3>spawnPositions)
    {
        if(m_pool==null)
        {
            Debug.LogWarning("Poolinstallationbypulling:Pool")
        }
    }
}
