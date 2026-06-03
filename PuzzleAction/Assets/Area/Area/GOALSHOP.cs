using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// GameManagerにマージする予定
/// Playerの検知したAreaTypeがShopまたはGoalだった場合の処理の起動システム
/// </summary>

public class GOALSHOP : MonoBehaviour
{
    [SerializeField] private AreaType m_type;

    [SerializeField] private InstanceCounter _shopInstanceCounter;
    private int _shopId;


    private Transform m_playerTransform;
    [SerializeField] private float m_InteractDistance = 3.0f;//Player検知範囲
    [SerializeField] private IntEventSO m_showShopId;
    [SerializeField] private BoolEventSO m_showShopUI;

    [SerializeField] private GameObject[] m_areaObject;

    private bool m_active = false;
    private void Awake()
    {
        _shopId = _shopInstanceCounter.Register();
    }


    private void Start()
    {
        //プレイヤーの場所取得soに変更予定だよ
        //プレイヤーのTransform.Positionさえ取れればいい
        var player = Object.FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            m_playerTransform = player.transform;
            Debug.Log("Player Found!");
        }

        foreach (var obj in m_areaObject)
        {
            Vector3 scale = obj.transform.localScale;
            scale *= m_InteractDistance * 2;
            obj.transform.localScale = scale;
        }
    }

    private void Update()
    {
        if (m_playerTransform == null) return;
        //Playerとこのスクリプトがついたオブジェクトの距離を求めてる
        float distance = Vector3.Distance(transform.position, m_playerTransform.position);

        if (distance <= m_InteractDistance)
        {
            //debug用 ：　EKeyを押したら
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                //おそらくGameManagerで検知予定
                DoAction();
            }
        } else if (m_active)
        {
            ForceAction(false);
        }
    }

    /// <summary>
    /// AreaTypeがShopとGoalだった時の処理の起動
    /// </summary>
    private void DoAction()
    {
        switch (m_type)
        {
            case AreaType.Shop:
                Debug.Log("らっしゃい！");
                //AreaTypeがShopだった時の処理追加（Shopの起動）
                //gameManager ShopUI true
                m_active = !m_active;
                m_showShopId.Raise(_shopId);
                m_showShopUI.Raise(m_active);
                break;

            case AreaType.Goal:
                Debug.Log("ゴールおめ");
                //AreaTypeがGoalだった時の処理追加（Goalの起動）
                //gameManager GoalUI true
                break;
        }
    }

    private void ForceAction(bool state)
    {
        m_active = state;
        switch (m_type)
        {
            case AreaType.Shop:
                Debug.Log("らっしゃい！");
                //AreaTypeがShopだった時の処理追加（Shopの起動）
                //gameManager ShopUI true
                m_showShopId.Raise(_shopId);
                m_showShopUI.Raise(m_active);
                break;

            case AreaType.Goal:
                Debug.Log("ゴールおめ");
                //AreaTypeがGoalだった時の処理追加（Goalの起動）
                //gameManager GoalUI true
                break;
        }

    }

    //debug用
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_InteractDistance);
    }
}
