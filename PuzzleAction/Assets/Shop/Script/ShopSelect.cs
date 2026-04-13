using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ShopSelect : MonoBehaviour,
    IPointerEnterHandler, 
    IPointerExitHandler,
    IPointerClickHandler
{

    [SerializeField] private Button m_button;

    [SerializeField, TextArea] private string m_itemDescription;
    // [SerializeField] private Text m_itemInfoText;
    [SerializeField] private ItemInfoText m_itemInfoText;
    private Button m_buttonData; //商品データ用

    private void Awake()
    {
        m_buttonData = GetComponent<Button>();
    }


    private void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        //Debug.Log(mousePos);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log(gameObject.name + "からマウスが重なった");
        if (m_buttonData == null) return;

        m_itemInfoText.ShowItemInfo(
            m_buttonData.ItemName,
            m_buttonData.ItemId,
            m_buttonData.Price
            );
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log(gameObject.name + "からマウスが離れた");
        m_itemInfoText.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_button.BuyItem();
        //Debug.Log(gameObject.name + "がクリックされた");
    }
}
