using UnityEngine;

public class InteractArea : MonoBehaviour
{
    [SerializeField]
    private GameObject m_interactionButton;

    private void Start()
    {
        if (m_interactionButton != null)
        {
            m_interactionButton.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        m_interactionButton.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        m_interactionButton.SetActive(false);
    }
}