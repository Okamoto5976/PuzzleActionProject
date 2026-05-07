using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public PlayerItem itemData;

    public void TryPickup(GameObject player)
    {
        Hotbar hotbar =player.GetComponent<Hotbar>();

        if(hotbar != null )
        {
            bool success = hotbar.AddItem(itemData);

            if(success)
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("ホットバーがいっぱい");
            }
        }
    }
}
