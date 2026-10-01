using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InfoText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_nameText;
    [SerializeField] private TextMeshProUGUI m_infoText;

    //仮　後に引数はItemData
    public void GetItemDataInfo(ItemData data, Vector3 goodsPos)
    {
        m_nameText.text = data.ItemName;
        m_infoText.text = data.Description;

        SetPlace(goodsPos);
    }

    public void Reset()
    {
        m_nameText.text = null;
        m_infoText.text = null;
    }

    //仮　場所によって配置を変えてほしい
    public void SetPlace(Vector3 goodsPos)
    {
        Canvas.ForceUpdateCanvases();

        RectTransform rect = GetComponent<RectTransform>();

        float panelWidth = rect.rect.width;

        Vector3 pos = goodsPos;

        if (goodsPos.x > Screen.width * 0.5f)
        {
            pos.x -= panelWidth + 30f;
        }
        else
        {
            pos.x += 100f;
        }

        transform.position = pos;
    }
}
