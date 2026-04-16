using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    [SerializeField] private HitCollider m_hitCollider;
    private DamageData data;
    public GameObject GameObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
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
            m_hitCollider.AttackCollider(data, this.GameObject.GetComponent<ITeam>().Team);
        }
        // Sキーが押された瞬間かどうか
        if (sKey.wasPressedThisFrame)
        {

        }
    }
}
