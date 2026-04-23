using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventorySlot
{
    public Item item;
    public bool IsEmpty => item == null;

    public void Clear()
    {
        item = null;
    }
}

public class SlotUI : MonoBehaviour,
    IBeginDragHandler,
    IEndDragHandler,
    IDragHandler
{
    public int x;
    public int y;

    public Image dragIconImage;
    public InventorySlot Inventory;

    [SerializeField] private InventorySystem inventory;

    [SerializeField] private InputActionReference m_action;

    void Start()
    {
        //inventory = FindFirstObjectByType<InventorySystem>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //inventory.BeginDrag(y, x);
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragIconImage.transform.position = eventData.position;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        //inventory.EndDrag(eventData.position);

    }

    private void Update()
    {
        //if(m_action.action.IsPressed())
        //{
        //    inventory.BeginDrag(y,x);
        //}
    }
}