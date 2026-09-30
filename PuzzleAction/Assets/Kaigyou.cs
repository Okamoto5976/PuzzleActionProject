using UnityEngine;
using TMPro;
using JetBrains.Annotations;
public class Excheng : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private int maxCharsPerLine ;

    public void excheng(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            textUI.text = "";
            return;
        }

        string result = InsertLineBreaks(text, maxCharsPerLine);
        textUI.text = result;


    }
    private string InsertLineBreaks(string text, int maxChars)
    {
        if (maxChars <= 0) return text;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        int count = 0;

        foreach (char c in text)
        {
            sb.Append(c);
            count++;

            // Žw’è•¶Žš”‚É’B‚µ‚½‚ç‰üs
            if (count > maxChars)
            {
                sb.Append('\n');
                count = 0;
            }
        }
        return sb.ToString();
    }
    private void Start()
    {
        string text = textUI.text;
        excheng(text);
        textUI.text = text;
    }
}


