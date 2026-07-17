using UnityEngine;

[CreateAssetMenu(fileName = "FloatRunTime", menuName = "Scriptable Objects/RunTimes/FloatRunTime")]
public class FloatRunTime : ScriptableObject
{
    [SerializeField] private float m_value;

    public float Value { get => m_value; }


    public void SetValue(float value)
    {
        m_value = value;
    }

    public void AddValue(float value)
    {
        m_value += value;
    }

    public void SubtractValue(float value)
    {
        m_value -= value;
        if (m_value < 0) m_value = 0; //所持金がマイナスにならないよう
    }
}
