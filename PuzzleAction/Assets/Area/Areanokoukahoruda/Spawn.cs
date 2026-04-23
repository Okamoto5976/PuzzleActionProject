using UnityEngine;

public class Spawn : MonoBehaviour
{
    public string m_objectKey = "SummonedObject";//pool名
    public Transform m_spawnPoint;

    private bool m_Spawned = false;//出現確認

    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player") && !m_Spawned)
    //    {
    //        ActivateSpawn();
    //        m_Spawned = true;
    //    }
    //}

    //public void SpawnEnemy()
    //{
    //    Debug.Log("EnemySpawn");
    //    Debug.Log(m_spawnPoint.position);
    //    ActivateSpawn();
    //    m_Spawned = true;
    //}


    public void ActivateSpawn()
    {
        if (m_spawnPoint == null)
        {
            Debug.LogWarning("スポーン地点が設定されてない");
            return;
        }
        //範囲
        Vector2 randomCircle = Random.insideUnitCircle * 3f;

        Vector3 spawnPosition = new Vector3
            (m_spawnPoint.position.x + randomCircle.x,
            m_spawnPoint.position.y,
            m_spawnPoint.position.z + randomCircle.y);

        Debug.Log($"{m_objectKey}を{spawnPosition}に召喚");

        //var obj = m_pool.Get{m_objectKey}
        //obj.transform.position = spawnPosition

        m_Spawned = true;
    }





    [ContextMenu("初期化")]
    public void ResetSpawn()
    {
        m_Spawned = false;

    }
}
