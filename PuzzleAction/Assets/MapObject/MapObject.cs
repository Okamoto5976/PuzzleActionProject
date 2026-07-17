using System;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    public event Action<MapObject> OnDestroyed;

    private int currentHitCount;

    public GameObject ItemPrefab {  get; private set; }

    public void Initialize(int maxHit,GameObject item,GameObject visualPrefab)
    {
        currentHitCount = maxHit;
        ItemPrefab = item;

        SetVisual(visualPrefab);
    }

    private void SetVisual(GameObject visualPrefab)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        if (visualPrefab != null)
        {
            Instantiate(visualPrefab, transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            RegisterHit();
        }
    }

    private void RegisterHit()
    {
        currentHitCount--;
        Debug.Log($"{gameObject.name}‚Éƒqƒbƒg!Žc‚è‘Ï‹v:{currentHitCount}");

        if(currentHitCount<=0)
        {
            OnDestroyed?.Invoke(this);
        }
    }
}
