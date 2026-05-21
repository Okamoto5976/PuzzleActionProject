using UnityEngine;
using TMPro;
public class ItemInfoText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_itemInfoText;

    public void ShowItemInfo(string itemName, int itemId, int price)
    {
        m_itemInfoText.text = $"Name : {itemName}\n" +
                              $"ID   : {itemId}\n" +
                              $"Price : {price}";

        m_itemInfoText.gameObject.SetActive(true);
    }
    public void Hide()
    {
        m_itemInfoText.gameObject.SetActive(false);
    }
}
