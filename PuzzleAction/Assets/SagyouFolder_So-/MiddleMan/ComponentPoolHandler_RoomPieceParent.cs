using UnityEngine;

public class ComponentPoolHandler_RoomPieceParent : ComponentPoolHandler<RoomPieceParent>
{
    public void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// Item object at position
    /// </summary>
    public void DropItemAtPosition(Vector3 position)
    {
        var obj = GetComponentFromPool();
        obj.transform.position = position;
        obj.gameObject.SetActive(true);
    }
}
