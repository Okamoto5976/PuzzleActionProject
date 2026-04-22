using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
public class AreaSet : MonoBehaviour
{
    [Header("設置するプレハブ")]
    public GameObject m_ShopPrefab;
    public GameObject m_GoalPrefab;

    [Header("設定")]
    public bool m_IsGoal = false;
    public Transform m_SpawnPoint;
    void Start()
    {
        if (m_IsGoal) SetupGoal();

    }
    private void SetupGoal()
    {
        Transform targetPoint = m_SpawnPoint != null ? m_SpawnPoint : transform;
        Instantiate(m_GoalPrefab, targetPoint.position, targetPoint.rotation, targetPoint);
        Debug.Log("G");
    }
    public void SetupRoom(AreaType type, Transform spawnPoint)
    {
        switch (type)
        {
            case AreaType.Shop:
                if (m_ShopPrefab != null)
                {
                    Instantiate(m_ShopPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
                    Debug.Log("S");
                }
                break;

                // case AreaType.Summon:
                //     if (m_GoalPrefab != null)
                //     {
                //         Instantiate(m_GoalPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
                //         Debug.Log("G");
                //     }
                //     break;
        }  
    }
}