using UnityEngine;

public class ItemDropper : ComponentPoolHandler<DroppedObject>
{
    public void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// Drop object at position
    /// </summary>
    public void DropItemAtPosition(Vector3 position)
    {
        var obj = GetComponentFromPool();
        obj.transform.position = position;
        obj.gameObject.SetActive(true);
    }
}
