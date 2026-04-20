using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class shop : MonoBehaviour
{
    [Header("ショップ用イベント")]
    [SerializeField] private UnityEvent m_ShopOpen;
    [SerializeField] private UnityEvent m_ShopClose;

    public Transform m_ShopPoint;

    private bool m_ButtonShop = false;
    private bool m_CurrentlyOpen = false;

    [SerializeField] private InputActionReference m_actionKye;

   // private void OnTriggerEnter(Collider other)
   // {
   //     if (other.CompareTag("Player")) m_ButtonShop = true;
   // }
   //
   // private void OnTriggerExit(Collider other)
   // {
   //     if (other.CompareTag("Player")) m_ButtonShop = false;
   // }
    public void ActivationShop()
    {
      if(m_ButtonShop)
        {
            Debug.Log("ショップエリアから離脱(テスト)");
            m_ButtonShop = false;
        }
        else
        {
            Debug.Log("ショップエリアに侵入(テスト)");
            m_ButtonShop=true;
        }
    }


    void Update()
    {
        //ショップにいてキーを押したら
        if (m_ButtonShop)
        {
            if (m_actionKye.action.WasPressedThisFrame())
            {
                if (m_CurrentlyOpen == false)
                {
                  
                    m_ShopOpen.Invoke();
                    m_CurrentlyOpen = true;
                }
                else
                {
                   
                    m_CurrentlyOpen = false;
                    m_ShopClose.Invoke();
                }
            }
        }
        if(m_CurrentlyOpen&&!m_ButtonShop)
        {
           
            m_CurrentlyOpen = false;
            m_ShopClose.Invoke();
        }
    }
}
