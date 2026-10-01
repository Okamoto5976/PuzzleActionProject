using UnityEngine;
using TMPro;

public class TestScript : MonoBehaviour
{
    [SerializeField] private string m_testText;

    [SerializeField] private TextMeshProUGUI m_text;

    private void Start()
    {
        m_text.text = m_testText;
    }
}
