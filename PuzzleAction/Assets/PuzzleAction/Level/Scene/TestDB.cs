using UnityEngine;

[CreateAssetMenu(fileName = "TestDB", menuName = "Scriptable Objects/TestDB")]
public class TestDB : ScriptableObject
{
    [SerializeField] private int m_value;

    public void AddValue(int value)
    {
        m_value += value;
    }
}
