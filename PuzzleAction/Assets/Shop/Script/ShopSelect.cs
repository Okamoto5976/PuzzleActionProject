using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ShopSelect : MonoBehaviour,
    IPointerEnterHandler, 
    IPointerExitHandler,
    IPointerClickHandler
{
    private void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        //Debug.Log(mousePos);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + "からマウスが重なった");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + "からマウスが離れた");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + "がクリックされた");
    }
}
