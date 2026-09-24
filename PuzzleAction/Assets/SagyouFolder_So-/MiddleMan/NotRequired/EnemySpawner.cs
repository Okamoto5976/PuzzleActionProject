using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Middleman_Enemy middleman;

    public void SpawnEnemy(int enemyType)
    {
        var enemy = middleman.GetComponent((Enum_EnemyType)enemyType);
        enemy.transform.position = transform.position;
        enemy.gameObject.SetActive(true);
    }
}
