using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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
        m_image.raycastTarget = false;

        //サイズ
        RectTransform m_panelRect = m_tooltipPanel.GetComponent<RectTransform>();
        m_panelRect.sizeDelta = new Vector2(150, 100);

        //テキスト作成
        GameObject m_textObj = new GameObject("Text");
        m_textObj.transform.SetParent(m_tooltipPanel.transform);

        m_tooltipText = m_textObj.AddComponent<TextMeshProUGUI>();
        m_tooltipText.fontSize = 24;
        m_tooltipText.color = Color.white; 
        m_tooltipText.alignment = TextAlignmentOptions.Center;

        RectTransform m_textRect = m_textObj.GetComponent<RectTransform>();
        m_textRect.sizeDelta = m_panelRect.sizeDelta;
        m_textRect.localPosition = Vector3.zero;

        CanvasGroup m_froup = m_tooltipPanel.AddComponent<CanvasGroup>();
        m_froup.blocksRaycasts = false;

        //非表示
        m_tooltipPanel.SetActive(false);

    }
    void Update()
    {

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        m_tooltipPanel.SetActive(true);
        m_tooltipText.text = itemDescription;

        RectTransform itemRect = transform as RectTransform;
        RectTransform tooltipRect = m_tooltipPanel.GetComponent<RectTransform>();



        // ★ アイテムの右横に固定表示
        tooltipRect.position = itemRect.position + new Vector3(200, 0, 0);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_tooltipPanel.SetActive(false);
    }
}