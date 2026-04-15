using UnityEngine;

public class NewMonoBehaviourScript1 : MonoBehaviour
{
    public ItemData ItemData;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{ItemData.ItemName}");
            Destroy(gameObject);
            
        }
    }
}

