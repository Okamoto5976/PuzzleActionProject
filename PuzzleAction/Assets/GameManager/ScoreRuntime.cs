using UnityEngine;

[CreateAssetMenu(fileName = "ScoreRuntime", menuName = "Scriptable Objects/ScoreRuntime")]
public class ScoreRuntime : ScriptableObject
{
    [SerializeField] private int m_Value;

    public int Value { get => m_Value; }

    public void SetValue(int value)
    {
        m_Value = value;
    }

    public void AddValue(int value)
    {
        m_Value += value;
    }
}
