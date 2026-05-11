using UnityEngine;
[System.Serializable]
public enum AreaType
{
    None,
    Damage,
    Summon,
    Normal,
    Shop,
    Goal,
    NotImplemented,
}

public class AreaSet : MonoBehaviour
{
    [Header("エリアの種類")]
    //public AreaType m_CurrentArea;
    [Header("設置するプレハブ")]
    public GameObject m_ShopPrefab;
    public GameObject m_GoalPrefab;
    public GameObject m_EnemySpawnPrefab;

     void Start()
    {
        // エリアが作られた時にその種類に応じた初期設定をやる
        //SetupShop(transform.position);   
        //SetupGoal(transform.position);   
    }
    ///<summary>
    ///エリアの種類に応じて必要な物を出す
    /// </summary>
    public void SetupShop(Vector3 tragetPosition)
    {
        if (m_ShopPrefab != null)
        {
            Instantiate(m_ShopPrefab, tragetPosition, transform.rotation);
            Debug.Log($"Shopを{tragetPosition}に設置しました");
        }
    }
    public void SetupGoal(Vector3 tragetPosition)
    {
        if (m_GoalPrefab != null)
        {
            Instantiate(m_GoalPrefab, tragetPosition, transform.rotation);
            Debug.Log($"Shopを{tragetPosition}に設置しました");
        }
    }

    public void SetUpEnemySpawn(Vector3 pos)
    {
        if(m_EnemySpawnPrefab != null)
        {
            Instantiate(m_EnemySpawnPrefab, pos, Quaternion.identity);
        }
    }

    ///<summary>
    ///エリアのメイン機能を動かす　(debug)
    /// </summary>
    //public void 


}


