using UnityEngine;

public class DEBUG_TestMoneyAndItemPickup : MonoBehaviour
{
    private PickupItem m_pickUpItem;

    private void Awake()
    {
        m_pickUpItem = GetComponent<PickupItem>();
    }

    private void FixedUpdate()
    {
        if (m_pickUpItem.HasQueue)
        {
            GrabItem();
            GrabMoney();
        }
    }

    private void GrabItem()
    {
        if (m_pickUpItem.QueueHasObject<DroppedItem>())
        {
            var obj = m_pickUpItem.GetObjectFromQueue<DroppedItem>();
            obj.PrintData();
        }
    }

    private void GrabMoney()
    {
        if (m_pickUpItem.QueueHasObject<DroppedMoney>())
        {
            var obj = m_pickUpItem.GetObjectFromQueue<DroppedMoney>();
            obj.PrintMoney();
        }
    }
}
