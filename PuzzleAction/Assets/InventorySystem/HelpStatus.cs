using UnityEngine;
using UnityEngine.EventSystems;

public class HelpStatus : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject m_ui;

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_ui.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_ui.SetActive(false);
    }
}
