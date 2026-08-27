using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractArea : MonoBehaviour
{
    [SerializeField] private InputActionReference m_action;
    private InputDisplayName m_inputDisplayNameClass;

    [SerializeField] private GameObject m_interactionButton;
    [SerializeField] private TMP_Text m_text;

    [SerializeField] private Vector3Asset m_playerPos;

    [SerializeField] private float m_range = 3f;

    private void Start()
    {
        m_inputDisplayNameClass = new InputDisplayName();

        m_text.text = m_inputDisplayNameClass.GetInputName(m_action);

        if (m_interactionButton != null)
        {
            m_interactionButton.SetActive(false);
        }
    }

    private void Update()
    {
        float distance = Vector3.Distance(
            m_playerPos.Value,
            transform.position);

        if(distance <= m_range)
        {
            m_interactionButton.SetActive(true);
        }
        else
        {
            m_interactionButton.SetActive(false);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!other.CompareTag("Player"))
    //        return;

    //    m_interactionButton.SetActive(true);
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (!other.CompareTag("Player"))
    //        return;

    //    m_interactionButton.SetActive(false);
    //}
}