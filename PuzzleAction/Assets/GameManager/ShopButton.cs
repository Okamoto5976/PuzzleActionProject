using UnityEngine;

public class ShopButton : MonoBehaviour
{
    [SerializeField] private EventSO m_shopOpenEvent;

    public void OpenShop()
    {
        m_shopOpenEvent.Raise();
    }
}
