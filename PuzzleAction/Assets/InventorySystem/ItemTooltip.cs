using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject m_tooltipPanel;
    //表示専用のganeObject(パネル)
    public TextMeshProUGUI m_tooltipText;
    //表示位置　vector3
    //表示内容  name text etc...


    [TextArea]
    public string itemDescription;//表示内容

    void Start()
    {
        //パネル作成
        m_tooltipPanel = new GameObject("Tooltip");
        m_tooltipPanel.transform.SetParent(GameObject.Find("Canvas").transform);

        //背景
        var m_image = m_tooltipPanel.AddComponent<UnityEngine.UI.Image>();
        m_image.color = new Color(0, 0, 0, 0.8f);

        //サイズ
        RectTransform m_panelRect = m_tooltipPanel.GetComponent<RectTransform>();
        m_panelRect.sizeDelta = new Vector2(300, 100);

        //テキスト作成
        GameObject m_textObj = new GameObject("Text");
        m_textObj.transform.SetParent(m_tooltipPanel.transform);

        m_tooltipText = m_textObj.AddComponent<TextMeshProUGUI>();
        m_tooltipText.fontSize = 24;
        //m_tooltipText.color = Color.white; 
        m_tooltipText.alignment = TextAlignmentOptions.Center;

        RectTransform m_textRect = m_textObj.GetComponent<RectTransform>();
        m_textRect.sizeDelta = m_panelRect.sizeDelta;
        m_textRect.localPosition = Vector3.zero;

        //非表示
        m_tooltipPanel.SetActive(false);

    }
    void Update()
    {
        if (m_tooltipPanel != null && m_tooltipPanel.activeSelf)
        {
            Vector3 m_pos = Input.mousePosition;
            m_tooltipPanel.transform.position = m_pos + new Vector3(150, -50, 0);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_tooltipPanel.SetActive(true);
        m_tooltipText.text = itemDescription;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_tooltipPanel.SetActive(false);
    }
}