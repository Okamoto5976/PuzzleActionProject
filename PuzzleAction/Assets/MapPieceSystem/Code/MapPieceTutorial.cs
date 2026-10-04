using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class MapPieceTutorial : MonoBehaviour
{
    [SerializeField] private List<string> m_tutorialText = new();

    [SerializeField] private GameObject m_tutorialPanel;
    [SerializeField] private TextMeshProUGUI m_text;

    [SerializeField] private GameObject m_guidePanel;

    private int m_num;

    [SerializeField] private MapPlaceSystem m_mapPlaceSystem;

    [SerializeField] private GameObject m_tutorialCompletedPanel;
    private TutorialSave m_tutorialSave = new();
    [SerializeField] private GameObject m_resetButton;

    private void Start()
    {
        //tutorial‚ª true‚ªŠm”F
        if (GameManager.Instance.IsTutorial)
        {
            m_mapPlaceSystem.SetCanMovePiece(false);

            m_tutorialPanel.SetActive(true);
            m_guidePanel.SetActive(false);
            m_resetButton.SetActive(false);

            m_text.text = m_tutorialText[0];
            m_num++;
        }
        else
        {
            var data = m_tutorialSave.LoadTutorialData();

            if (data == null) return;

            if(!data.m_GoalTutorialCompleted)
            {
                m_tutorialCompletedPanel.SetActive(true);
                
                data.m_GoalTutorialCompleted = true;

                m_tutorialSave.SaveTutorialData(data);
            }
        }
        
    }

    public void NextText()
    {
        if(m_num >= m_tutorialText.Count)
        {
            m_tutorialPanel.SetActive(false);
            m_guidePanel.SetActive(true);

            m_mapPlaceSystem.SetCanMovePiece(true);

            return;
        }

        m_text.text = m_tutorialText[m_num];
        m_num++;
    }

    public void SetCompletedPanel(bool value)
    {
        m_tutorialCompletedPanel.SetActive(value);
    }
}
