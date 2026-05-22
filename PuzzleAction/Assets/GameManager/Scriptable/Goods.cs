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

    private bool m_soldOn = false;

    //仮　ItemDataに変更
    private TestItemData m_data;

    private ShopManager m_shopManager;

    //[SerializeField] private InfoText m_infoTextPrefab;

    private void Start()
    {
        m_soldImage.SetActive(false);
    }

    //仮　ItemDataの中のデータ（Iconと値段）を受け取る
    public void Init(TestItemData data,ShopManager manager)
    {
        m_data = data;
        m_shopManager = manager;

        m_icon.sprite = data.m_icon;
        m_priceText.text = data.m_price.ToString() + " $";
    }

    //カーソルが上に乗った時
    public void OnPointerEnter(PointerEventData eventData)
    {
        //anim再生
        //SE再生

        //info　表示
        m_shopManager.OnInfoPanelFromGoods(m_data);


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
        //購入済みの際　買えない
        if (m_soldOn) return;

        if(m_shopManager.PurchaseItem(m_data))
        {
            m_soldOn = true;

            m_soldImage.SetActive(true);
        }
    }
}
