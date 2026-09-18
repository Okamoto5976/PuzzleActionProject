using UnityEngine;

public class MoneyPickup : MonoBehaviour
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
            GrabMoney();
        }
    }

    //private void GrabItem()
    //{
    //    if (m_pickUpItem.QueueHasObject<DroppedItem>())
    //    {
    //        var obj = m_pickUpItem.GetObjectFromQueue<DroppedItem>();
    //        obj.PrintData();
    //    }
    //}

    private void GrabMoney()
    {
        if (m_pickUpItem.QueueHasObject<DroppedMoney>())
        {
            var obj = m_pickUpItem.GetObjectFromQueue<DroppedMoney>();
            int money = obj.GetMoney();

            GameManager.Instance.ModifyMoney(money);
        }
    }
}
