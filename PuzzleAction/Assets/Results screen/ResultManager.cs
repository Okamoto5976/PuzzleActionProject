using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Net;

public class ResultManager : MonoBehaviour
{
    [System.Serializable]
    private struct TextElement
    {
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private string text;
        [SerializeField] private bool animationEnabled;

        public readonly TextMeshProUGUI TextMeshPro => textMeshPro;
        public readonly string Text => text;
        public readonly bool AnimationEnabled => animationEnabled;

        public void SetAnimEnabled(bool value)
        {
            animationEnabled = value;
        }
    }

    private enum TextContent
    {
        ClearFloor,
        ClearTime,
        KillCount,
    }


    // constant
    private const int MPH = 60; // minute per hour
    private const int SPM = 60; // second per minute

    [Header("Setting")]
    [SerializeField] private int m_decimalPlaces;   // for the clear time
    [SerializeField] private int m_resultDisplayInterval;
    [SerializeField] private int m_countUpTime; // time to count number

    [Header("State")]
    [SerializeField] private int m_floor;
    [SerializeField] private int m_clearTime;
    [SerializeField] private int m_killCount;
    [SerializeField] private int m_textTimerIndex;
    [SerializeField] private int[] m_countUpTimers;

    [Header("Object")]
    [SerializeField] private List<TextElement> m_textElements;
    


    private void Awake()
    {
        Application.targetFrameRate = 60;

        m_countUpTimers = new int[m_textElements.Count];

        for (int i = 0; i < m_textElements.Count; i++)
        {
            if (int.TryParse(m_textElements[i].Text, out int num)) continue;
            if (m_textElements[i].AnimationEnabled) m_textElements[i].SetAnimEnabled(false);
        }

        SetText();
    }

    private void Update()
    {
        TimerCountUp();
    }


    private void SetText()
    {
        m_textElements[(int)TextContent.ClearFloor].TextMeshPro.text = m_textElements[(int)TextContent.ClearFloor].Text.Replace(":value:", m_floor.ToString());
        m_textElements[(int)TextContent.ClearTime].TextMeshPro.text = m_textElements[(int)TextContent.ClearTime].Text.Replace(":value:", m_clearTime.ToString());
        m_textElements[(int)TextContent.KillCount].TextMeshPro.text = m_textElements[(int)TextContent.KillCount].Text.Replace(":value:", m_killCount.ToString());
    }

    private void CountUp(int _index, int _count, int _timer)
    {
        Debug.Log($"_index: {_index}, _count: {_count}, _timer: {_timer}");

        if (m_textElements[_index].AnimationEnabled == false) return;

        int _progressNum = _timer * _count / m_countUpTime;
        if (_timer == m_countUpTime) _progressNum = _count;
        m_textElements[_index].TextMeshPro.text = m_textElements[(int)TextContent.ClearFloor].Text.Replace(":value:", _progressNum.ToString());
    }

    private void TimerCountUp()
    {
        if (m_textTimerIndex < m_textElements.Count)
        {
            if (m_textElements[m_textTimerIndex].AnimationEnabled == false)
            {
                m_countUpTimers[m_textTimerIndex] = m_countUpTime;
            }

            if (m_countUpTimers[m_textTimerIndex] < m_countUpTime)
            {
                m_countUpTimers[m_textTimerIndex]++;

                switch (m_textTimerIndex)
                {
                    case (int)TextContent.ClearFloor:
                        CountUp(m_textTimerIndex, m_floor, m_countUpTimers[m_textTimerIndex]);
                        break;

                    case (int)TextContent.ClearTime:
                        CountUp(m_textTimerIndex, m_clearTime, m_countUpTimers[m_textTimerIndex]);
                        break;

                    case (int)TextContent.KillCount:
                        CountUp(m_textTimerIndex, m_killCount, m_countUpTimers[m_textTimerIndex]);
                        break;
                }
            }
            else
            {
                m_textTimerIndex++;
            }
        }
    }

    // button

    public void ButtonNext()
    {
        // load scene
    }

    public void ButtonTitle()
    {
        // load scene
    }
}
