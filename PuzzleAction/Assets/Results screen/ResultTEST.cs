using UnityEngine;
using TMPro;
public class ResultTEST : MonoBehaviour
{
    public TextMeshProUGUI m_ScoreText;
    public int m_DummyScore = 12345;
    void Start()
    {
        UpdateScoreDisplay(m_DummyScore);
    }

    void UpdateScoreDisplay(int score)
    {
        if (m_ScoreText != null)
        {
            
            m_ScoreText.text = "Score: " + score.ToString();
        }
    }
}

