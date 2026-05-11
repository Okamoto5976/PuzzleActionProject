using UnityEngine;

/// <summary>
/// Poolから取得したObjectの召喚
/// </summary>
public class Spawn : MonoBehaviour
{
    [SerializeField] private ObjectPoolManager m_pool;
    [SerializeField] private string m_objectKey = "SummonedObject";//Object名
    private Transform m_spawnPosition;

    private bool m_Spawned = false;      //Spawnしたかどうか

    private void Awake()
    {
        m_spawnPosition = GetComponent<Transform>();
    }

    /// <summary>
    /// Spawnの処理
    /// </summary>
    [ContextMenu("ActivateSpawn")]
    public void ActivateSpawn()
    {
        if (m_spawnPosition == null || m_pool == null)
        {
            Debug.LogWarning("スポーン地点が設定されてない");
            return;
        }

        //poolからオブジェクトを持ってくる所
        //var obj = m_pool.Get{m_objectKey}
        GameObject obj = m_pool.GetObjectFromPool();

        //半径3mいないの円の中の値をランダムに取得
        Vector2 randomCircle = Random.insideUnitCircle * 3f;

        Vector3 spawnPosition = new Vector3
            (m_spawnPosition.position.x + randomCircle.x,
            m_spawnPosition.position.y,
            m_spawnPosition.position.z + randomCircle.y);

        obj.transform.position = spawnPosition;

        m_Spawned = true;
        //enm enemyScript = obj.GetComponent<enm>();
        //if (enemyScript != null)
        //{
        //    enemyScript.SetOrigin(m_pool);
        //}
        Debug.Log($"PoolManager : {m_objectKey}を{spawnPosition}に召喚");
    }

    // オブジェクトをpoolに戻す処理の追加
    //ReturnObjectToPool(obj)

    [ContextMenu("初期化")]
    public void ResetSpawn()
    {
        m_Spawned = false;

        //消す
        //enm[] enemies = Object.FindObjectsByType<enm>(FindObjectsSortMode.None);
        //foreach (var e in enemies)
        //{
        //    e.ReturnToPool(); // 前に作った「戻す」処理を呼ぶ
        //}

        Debug.Log("すべての敵を回収してリセットしました。");
    }
}
