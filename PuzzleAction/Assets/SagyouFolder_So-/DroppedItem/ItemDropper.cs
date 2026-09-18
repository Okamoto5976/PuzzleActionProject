using UnityEngine;

public class ItemDropper : ComponentPoolHandler<DroppedObject>
{
    [SerializeField] private DropMoneyEventSO m_dropMoneyEventSO;

    private void OnEnable()
    {
        m_dropMoneyEventSO.Register(DropItemAtPosition);
    }

    private void OnDisable()
    {
        m_dropMoneyEventSO.Unregister(DropItemAtPosition);

    }


    public void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// Drop object at position
    /// </summary>
    public void DropItemAtPosition(Vector3 position, int money)
    {
        var obj = GetComponentFromPool();
        obj.SetValue(money);
        obj.transform.position = position;
        obj.gameObject.SetActive(true);
    }
}
