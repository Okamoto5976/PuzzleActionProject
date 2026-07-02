using UnityEngine;

public class tesususu : MonoBehaviour
{
    [Header("テスト用の罠プレハブ（あらかじめBoxColliderやGas_Trap等がついている想定）")]
    [SerializeField] private GameObject m_TestTrapPrefab;

    [Header("テスト用の配置サイズ")]
    [SerializeField] private Vector2 m_TestRoomSize = new Vector2(5f, 5f);

    [ContextMenu("子供をいじる親子化テストを実行！")]
    public void TestDeploy()
    {
        if (m_TestTrapPrefab == null)
        {
            Debug.LogError("【テストエラー】m_TestTrapPrefab がセットされていません！");
            return;
        }

        Vector3 spawnPos = transform.position;
        string requestName = "GasTest";

        // 1. あっちのプールから持ってきたと仮定してプレハブを生成（これが子供になる）
        GameObject spwanedTrap = Instantiate(m_TestTrapPrefab);

        if (spwanedTrap != null)
        {
            // 2. その場で「本当に空っぽ」の親オブジェクトを作成
            GameObject trapParentBox = new GameObject(requestName + "_Container");
            trapParentBox.transform.position = spawnPos;

            // 3. 攫ってきたトラップ（子供）を、空っぽの親の中に入れる
            spwanedTrap.transform.SetParent(trapParentBox.transform);

            // 4. 子供のローカル座標をリセットして親の中心に綺麗に重ねる
            spwanedTrap.transform.localPosition = Vector3.zero;

            // 👈【ここが重要！】親ではなく「子供（spwanedTrap）」からBoxColliderを引っ張ってくる
            BoxCollider childBox = spwanedTrap.GetComponent<BoxCollider>();

            // 5. 子供のコライダーが存在すれば、そのサイズを外から書き換える！
            if (childBox != null)
            {
                childBox.size = new Vector3(m_TestRoomSize.x, 1f, m_TestRoomSize.y);
            }
            else
            {
                Debug.LogWarning("【テスト警告】子供に BoxCollider が見つかりませんでした！");
            }

            // 6. 親オブジェクトをアクティブ化（これで中の子供も一緒にオンになります）
            trapParentBox.SetActive(true);

            Debug.Log($"【テスト成功】空の親 {trapParentBox.name} を作成。中の子供 {spwanedTrap.name} のコライダーサイズを {m_TestRoomSize} に変更しました！");
        }
    }
}