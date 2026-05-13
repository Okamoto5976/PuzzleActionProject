using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    [SerializeField] private HitCollider m_hitCollider;
    [SerializeField] private RayCollider m_rayCollider;
    private DamageData data;
    public GameObject GameObject;

    // ▼仮----
    Vector3 pos1 = new(2, 2, 2);
    Vector3 pos2 = new(0, 0, 0);
    float distance;
    // ▲------

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        distance = Mathf.Sqrt((pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.y - pos2.y) * (pos1.y - pos2.y) + (pos1.z - pos2.z) * (pos1.z - pos2.z));
    }

    // Update is called once per frame
    void Update()
    {
        // 現在のキーボード情報
        var current = Keyboard.current;

        // キーボード接続チェック
        if (current == null)
        {
            // キーボードが接続されていないと
            // Keyboard.currentがnullになる
            return;
        }

        // Aキーの入力状態取得
        var aKey = current.aKey;
        var sKey = current.sKey;

        // Aキーが押された瞬間かどうか
        if (aKey.wasPressedThisFrame)
        {
            Debug.Log("Press A key");
            //m_hitCollider.AttackCollider(data, this.GameObject.GetComponent<ITeam>().Team, this.GameObject.GetComponent<Test_chara>().attackHitBox);
            Debug.Log($"this.gameObject: {this.GameObject}");
            m_rayCollider.AttackCollider(data, this.GameObject.GetComponent<Entity>().Team);
        }
        // Sキーが押された瞬間かどうか
        if (sKey.wasPressedThisFrame)
        {
            Vector3 vec = new Vector3(2, 3, 0);
            //Debug.Log($"pos1: {pos1}\npos2: {pos2}\n相対座標(3d): {distance}");
            Debug.Log($"normalize(2, 3, 0): {vec.normalized}");
        }
    }
}
