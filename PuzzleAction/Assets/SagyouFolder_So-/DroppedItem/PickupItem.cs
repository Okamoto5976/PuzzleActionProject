using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private float m_pickupRange;
    [SerializeField] private int m_maxColliderDetectCount;

    protected List<DroppedObject> m_pickedUpItems = new();
    private Transform m_transform;

    public bool HasQueue => m_pickedUpItems.Count > 0;


    private void Awake()
    {
        Initialize(transform);
    }

    private void FixedUpdate()
    {
        PickUpItemsInRange(m_pickupRange, m_transform.position);
    }

    private void Initialize(Transform transform)
    {
        m_transform = transform;
    }

    private void PickUpItemsInRange(float pickUpRange, Vector3 position)
    {
        Collider[] hits = new Collider[m_maxColliderDetectCount];
        Physics.OverlapSphereNonAlloc(position, pickUpRange, hits);
        foreach (Collider hit in hits)
        {
            if (hit == null) continue;
            if (hit.TryGetComponent(out DroppedObject droppedItem))
            {
                droppedItem.PickupItem(this);
            }
        }
    }
    private void ForcePickUpItemsInRange(float pickUpRange, Vector3 position)
    {
        Collider[] hits = new Collider[m_maxColliderDetectCount];
        Physics.OverlapSphereNonAlloc(position, pickUpRange, hits);
        foreach (Collider hit in hits)
        {
            if (hit == null) continue;
            if (hit.TryGetComponent(out DroppedObject droppedItem))
            {
                droppedItem.PickupItem(this);
            }
        }
    }

    public void DoPickupItem(DroppedObject item)
    {
        m_pickedUpItems.Add(item);
    }

    public T GetObjectFromQueue<T>()
    {
        foreach (DroppedObject _object in m_pickedUpItems)
        {
            if (_object is T component)
            {
                _object.Release();
                m_pickedUpItems.Remove(_object);
                return component;
            }
        }
        return default;
    }

    public bool QueueHasObject<T>() where T : DroppedObject
    {
        foreach (DroppedObject _object in m_pickedUpItems)
        {
            if (_object is T)
            {
                return true;
            }
        }
        return false;
    }
}
