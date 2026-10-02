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
        //êÿÇËè„Ç∞
        int count = Mathf.CeilToInt((float)money / 50f);
        count = Mathf.Clamp(count, 1, 16);

        int baseMoney = money / count;
        int remainder = money % count;

        for(int i  = 0; i < count; i++)
        {
            int dropMoney = baseMoney;

            if(i < remainder)
            {
                dropMoney++;
            }

            var obj = GetComponentFromPool();
            obj.SetValue(dropMoney);
            obj.transform.position = position;
            obj.gameObject.SetActive(true);
            obj.AddForce();
        }
        
    }
}
