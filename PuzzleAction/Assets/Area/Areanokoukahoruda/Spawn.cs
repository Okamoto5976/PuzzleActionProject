using UnityEngine;

public class Spawn : MonoBehaviour
{
    public string m_objectKey = "SummonedObject";//poolñº
    public Transform m_spawnPoint;

    private bool m_Spawned = false;//èoåªämîF

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
        Debug.Log(m_spawnPoint.position);
        m_Spawned = true;
        Debug.Log($"è¢ä´{m_objectKey}î≠ìÆ");
        //poolì¸ÇÍÇÈ
    }





    [ContextMenu("èâä˙âª")]
    public void ResetSpawn()
    {
        m_Spawned = false;
     
    }
}
