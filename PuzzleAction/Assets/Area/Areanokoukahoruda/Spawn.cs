using UnityEngine;

public class Spawn : MonoBehaviour
{
    public string m_objectKey = "SummonedObject";//pool名
    public Transform m_spawnPoint;

    private bool m_Spawned = false;//出現確認

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !m_Spawned)
        {
            ActivateSpawn();
            m_Spawned = true;
        }
    }




    public void ActivateSpawn()
    {
        Debug.Log($"召喚{m_objectKey}発動");
        //pool入れる
    }





    [ContextMenu("初期化")]
    public void ResetSpawn()
    {
        m_Spawned = false;
        Debug.Log("リセットしたお");
    }
}
