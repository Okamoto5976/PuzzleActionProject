using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Excheng : MonoBehaviour
{
   
    [SerializeField] private TextMeshProUGUI tmp;
    //[SerializeField] private int maxCharsPerLine ;
    public void SetText(string text)
    {
        tmp.text = text;
    }

    [SerializeField] private string m_text;
    public void Start()
    {
        tmp.text = m_text;
    }



}


