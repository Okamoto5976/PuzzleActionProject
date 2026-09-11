using UnityEngine;

public class DEBUG_DropItemsAtRandomPositions : MonoBehaviour
{
    [SerializeField] private float radius;

    [ContextMenu("Drop Items")]
    private void DropItems()
    {
        if (!TryGetComponent<ItemDropper>(out var pool))
        {
            Debug.LogError("pool is null");
            return;
        }
        Debug.Log(pool);
        for (var i = 0; i < pool.ObjectsInPool; i++)
        {
            var random = Random.insideUnitCircle * radius;
            Vector3 position = new(random.x, 0, random.y);
            pool.DropItemAtPosition(position);
        }
    }
}
