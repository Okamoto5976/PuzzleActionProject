using UnityEngine;

/// <summary>
/// GameManagerにマージする予定
/// Playerの検知したAreaTypeがShopだった場合の処理の起動システム
/// </summary>

public class ShopObj : MonoBehaviour, IInteractable
{
    [Header("Shop")]
    [SerializeField] private InstanceCounter _shopInstanceCounter;
    [SerializeField] private int _shopId;
    [SerializeField] private float m_InteractDistance = 3.0f;


    [SerializeField] private IntEventSO m_showShopId;
    [SerializeField] private BoolEventSO m_showShopUI;

    [SerializeField] private GameObject[] m_areaObject;

    [SerializeField] private BoolEventSO m_palyerIgnoreInput;

    private void Awake()
    {
        _shopId = _shopInstanceCounter.Register();

    }

    private void Start()
    {
        foreach (var obj in m_areaObject)
        {
            Vector3 scale = obj.transform.localScale;
            scale *= m_InteractDistance * 2;
            obj.transform.localScale = scale;
        }
    }

    public void OnInteract(Entity entity)
    {
        m_showShopId.Raise(_shopId);
        m_showShopUI.Raise(true);

        GameManager.Instance.OnSetStop(true);
        m_palyerIgnoreInput.Raise(true);
    }
}
