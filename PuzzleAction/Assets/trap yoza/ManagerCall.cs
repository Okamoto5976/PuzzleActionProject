using UnityEngine;

public class ManagerCall : MonoBehaviour
{
    public ItemManagercari m_ItemManager;
    [Header("Dynamaite=0 Gas=1 Swamp=2 Test")]
    public string m_ItemName = "";
    [Header("四角い部屋の敷き詰め設定")]
    [Tooltip("部屋の横幅(x)と置く幅(z)のサイズ")]
    [SerializeField] private Vector2 m_RoomSize = new Vector2(5f, 5f);

    [ContextMenu("テスト")]

    public void DeployTrap()
    {
        if (m_ItemManager == null) return;
        Vector3 myPos = transform.position;

        if (m_ItemName == "0")
        {
            m_ItemManager.SpawnItemAtPosition(m_ItemName, myPos);
        }
        else if (m_ItemName == "1" || m_ItemName == "2")
        {
            m_ItemManager.SpawnItemAtPosition(m_ItemName, myPos);
            //tuikasitene↓
            if (m_ItemName == "1")
            {
                Gas_Trap[] allGas = Object.FindObjectsByType<Gas_Trap>(FindObjectsSortMode.None);
                foreach (var gas in allGas)
                {
                    BoxCollider box = gas.GetComponent<BoxCollider>();
                    //dasitasyunnkann
                    if (box != null && box.size.x <= 1.1f)
                    {
                        box.size = new Vector3(m_RoomSize.x, 1f, m_RoomSize.y);
                        gas.Init(myPos, 1f, 1);
                        break;
                    }
                }
            }
            if (m_ItemName == "2")
            {
                Gas_Trap[] allSwamp = Object.FindObjectsByType<Gas_Trap>(FindObjectsSortMode.None);
                foreach (var swamp in allSwamp)
                {
                    BoxCollider box = swamp.GetComponent<BoxCollider>();
                    //dasitasyunnkann
                    if (box != null && box.size.x <= 1.1f)
                    {
                        box.size = new Vector3(m_RoomSize.x, 1f, m_RoomSize.y);
                        swamp.Init(myPos, 1f, 1);
                        break;
                    }
                }
            }
            Debug.Log($"[SYSTEM] {m_ItemName} の要請とサイズ自動フィットが完了しました。");
        }
    }
    [ContextMenu("初期化")]
    public void ClearALLTraps()
    {
        //tuikasitene↓
        Gas_Trap[] allGas = Object.FindObjectsByType<Gas_Trap>(FindObjectsSortMode.None);
        foreach (var gas in allGas)
        {
            //gas.ReleaseToPool();//poolhakore
            Destroy(gas.gameObject);//poolnisurutokikesitene
        }
        Swamp_Trap[] allSwamp = Object.FindObjectsByType<Swamp_Trap>(FindObjectsSortMode.None);
        foreach (var swamp in allSwamp)
        {
            //swamp.ReleaseToPool();//poolhakore
            Destroy(swamp.gameObject);//poolnisurutokikesitene
        }
        Dynamite_Trap[] alldynamaite = Object.FindObjectsByType<Dynamite_Trap>(FindObjectsSortMode.None);
        foreach (var dynamaite in alldynamaite)
        {
            //dynamite.ReleaseToPool();//poolhakore
            Destroy(dynamaite.gameObject);//poolnisurutokikesitene
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(m_RoomSize.x, 0.1f, m_RoomSize.y));
    }
}
