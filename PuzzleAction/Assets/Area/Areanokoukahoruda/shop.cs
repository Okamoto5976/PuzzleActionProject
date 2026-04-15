using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class shop : MonoBehaviour
{
    [Header("ショップ用イベント")]
    public UnityEvent m_ShopOpen;
    public UnityEvent m_ShopClose;

    public void ActivationShop()
    {
        Debug.Log("ショップエリアに入ったお");
    }
    private bool m_ButtonShop = false;
    private bool m_CurrentlyOpen = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) m_ButtonShop = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) m_ButtonShop = false;
    }


    void Update()
    {
        //ショップにいてキーを押したら
        if (m_ButtonShop && Keyboard.current != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (m_CurrentlyOpen == false)
                {
                    Debug.Log("らっしゃい!!");
                    m_ShopOpen.Invoke();
                    m_CurrentlyOpen = true;
                }
                else
                {
                    Debug.Log("ありがとうございました");
                    m_CurrentlyOpen = false;
                    m_ShopClose.Invoke();
                }
            }
        }
        if(m_CurrentlyOpen&&!m_ButtonShop)
        {
            Debug.Log("ありがとうございました");
            m_CurrentlyOpen = false;
            m_ShopClose.Invoke();
        }
    }
}
