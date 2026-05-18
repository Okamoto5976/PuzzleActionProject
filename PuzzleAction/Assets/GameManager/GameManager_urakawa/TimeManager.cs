using UnityEngine;

[CreateAssetMenu(fileName = "TimeManager", menuName = "Scriptable Objects/TimeManager")]
public class TimeManager : ScriptableObject
{
    [SerializeField] private float m_value;

    public float Value { get => m_value; }

    public void SetValue(float value)
    {
        m_value = value;
    }

    public void DecreaseValue(float amount)
    {
        m_value -= amount;
        if (m_value < 0f) m_value = 0f;
    }

    public void IncreaseValue(float amount)
    {
        m_value += amount;
    }
}
