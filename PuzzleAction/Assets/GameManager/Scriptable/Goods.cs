using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Goods : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    [SerializeField] private Image m_icon;
    [SerializeField] private GameObject m_soldImage;
    [SerializeField] private TextMeshProUGUI m_priceText;

    private int m_slotId;

    private bool m_soldOn = false;

    //仮　ItemDataに変更
    private Data m_data;

    private ShopManager m_shopManager;

    //[SerializeField] private InfoText m_infoTextPrefab;

    private void Start()
    {
        m_soldImage.SetActive(false);
    }

    /// <summary>
    /// Initialize Slot
    /// </summary>
    /// <param name="shopManager"></param>
    /// <param name="id"></param>
    public void Init(ShopManager shopManager, int id)
    {
        InjectShopManager(shopManager);
        m_slotId = id;
    }

    /// <summary>
    /// Inject SlotId
    /// </summary>
    /// <param name="id"></param>
    public void InjectSlotId(int id) => m_slotId = id;

    /// <summary>
    /// Inject ShopManager
    /// </summary>
    /// <param name="manager"></param>
    public void InjectShopManager(ShopManager manager) => m_shopManager = manager;

    // Set Data
    public void SetData(ShopItem shopItem)
    {
        m_data = shopItem.data;
        m_icon.sprite = m_data.Data.ItemIcon;
        m_priceText.text = m_data.Data.Price.ToString() + " $";
        SetSoldVisibility(shopItem.IsSold);
    }

    //カーソルが上に乗った時
    public void OnPointerEnter(PointerEventData eventData)
    {
        //anim再生
        //SE再生

        //info　表示
        m_shopManager.OnInfoPanelFromGoods(m_data.Data);


        //infoの量に応じて　大きさ変更したい
    }

    //カーソルが退いた時
    public void OnPointerExit(PointerEventData eventData)
    {
        //info　非表示
        m_shopManager.OffInfoPanelFromGoods();
    }

    //クリック時
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked!");
        //購入済みの際　買えない
        if (m_soldOn) return;
        Debug.Log("Buying...");
        if (m_shopManager.PurchaseItem(m_slotId))
        {
            SetSoldVisibility(true);
        }
    }

    private void SetSoldVisibility(bool state)
    {
        m_soldOn = state;
        m_soldImage.SetActive(m_soldOn);
    }
}
