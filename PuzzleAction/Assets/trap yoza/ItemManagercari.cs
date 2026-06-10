using UnityEngine;
//これ仮ねテストね捨てるねごみ箱ね
public class ItemManagercari : MonoBehaviour
{
    [Header("シリアライズでの罠登録枠")]
    [SerializeField] private GameObject m_D;
    [SerializeField] private GameObject m_G;
    [SerializeField] private GameObject m_S;
    
    [Header("罠の効果別　デバフ値")]
    [SerializeField] private float b;//罠の効果別デバフ
     

    //名前座標受け取り対応
    public void SpawnItemAtPosition(string itemName, Vector3 spawnPosition)
    {
        //Dynamite
        if (itemName == "0")
        {                
            if (m_D != null)
            {
                GameObject obj = Instantiate(m_D, spawnPosition, Quaternion.identity);
                //ダイナマ
                var d = obj.GetComponent<Dynamite_Trap>();
                if (d != null) d.Init(spawnPosition, 1, b);
                Debug.Log($"[仮MANAGER] 座標 {spawnPosition} に ダイナマイト を出しました！");
            }
        }
        //Gas
        else if (itemName == "1")
        {
            if (m_G != null)
            {
                GameObject obj = Instantiate(m_G, spawnPosition, Quaternion.identity);
                //ガス
                var g = obj.GetComponent<Gas_Trap>();
                if (g != null) g.Init(spawnPosition, 1, (int)b);
                Debug.Log($"[仮MANAGER] 座標 {spawnPosition} に ガス を出しました！");
            }
        }
        //Swamp
        else if (itemName == "2")
        {
            if (m_S != null)
            {
                GameObject obj = Instantiate(m_S, spawnPosition, Quaternion.identity);
                //沼
                var s = obj.GetComponent<Swamp_Trap>();
                if (s != null) s.Init(spawnPosition, 1, b);
                Debug.Log($"[仮MANAGER] 座標 {spawnPosition} に 沼 を出しました！");
            }
        }
    }
}
