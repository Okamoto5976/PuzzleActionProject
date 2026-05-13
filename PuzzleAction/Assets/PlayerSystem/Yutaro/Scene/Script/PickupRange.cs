using UnityEngine;

public class PickupRange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ItemPickUp item = other.GetComponent<ItemPickUp>();

        if (item != null)
        {
            item.TryPickup(transform.root.gameObject);
        }
    }
}
