using UnityEngine;
using UnityEngine.InputSystem;



public class GOALSHOP : MonoBehaviour
{
    public enum AreaType{shop,goal}
    public AreaType m_type;
    private Transform m_playerTransform;
    private float m_InteractDistance = 3.0f;//3m

    void Start()
    {
        var player = Object.FindAnyObjectByType<omae>();
        if (player != null)
        {
            m_playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (m_playerTransform == null) return;
        float distance = Vector3.Distance(transform.position, m_playerTransform.position);

        if (distance <= m_InteractDistance)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                DoAction();
            }
        }
    }

    void DoAction()
    {
        if (m_type == AreaType.shop) Debug.Log("‚ç‚Á‚µ‚á‚¢I");
        else Debug.Log("ƒS[ƒ‹‚¨‚ß");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_InteractDistance);
    }
}
