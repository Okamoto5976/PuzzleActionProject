using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageItem : MonoBehaviour
{
    [SerializeField] private int m_index;
    [SerializeField] private ItemBox m_boxData;

    [SerializeField] private Image m_image;

    [SerializeField] private TMP_Text m_text;
    public void SetValue(int value)
    {
        m_index = value;
    }

    public void SetData(ItemBox boxData)
    {
        m_boxData = boxData;
        SetImage(boxData.data.icon);
    }

    public void SetImage(Sprite image)
    {
        m_image.sprite = image;
    }

    public void OnViewInfo()
    {
        m_text.text = m_boxData.data.info;
    }
}
