using System.Collections;
using UnityEngine;
using TMPro;

public class TextDisplay_02 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_messageText;
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_audioClip;
    [SerializeField] private float m_pitch = 1.0f;

    public void ShowMessage(string message)
    {
        m_messageText.text = message;
    }

    public void ShowMessageGradually(string message, float speed = 0.04f)
    {
        Debug.Log("ShowMessageGradually : " + message);
        StopAllCoroutines();
        StartCoroutine(TypeText(message, speed));
    }




    private IEnumerator TypeText(string message, float speed)
    {
        m_messageText.text = "";

        foreach (char c in message)
        {
            m_messageText.text += c;
            if (m_audioClip != null)
            {
                m_audioSource.pitch = m_pitch;
                m_audioSource.PlayOneShot(m_audioClip);
            }

            yield return new WaitForSeconds(speed);
        }
    }
}