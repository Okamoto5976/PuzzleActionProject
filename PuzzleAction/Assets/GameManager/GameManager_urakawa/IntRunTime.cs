using UnityEngine;

[CreateAssetMenu(fileName = "IntRunTime", menuName = "Scriptable Objects/RunTimes/IntRunTime")]
public class IntRunTime : ScriptableObject
{
    [SerializeField] private int m_value;

    public int Value { get => m_value; }


    public void SetValue(int value)
    {
        m_value = value;
    }

    public void AddValue(int value)
    {
        Debug.Log("Add Level");
        m_value += value;
        Debug.Log($"{name} : {m_value}  InstanceID={GetInstanceID()}");

    }

    public void SubtractValue(int value)
    {
        m_value -= value;
        if (m_value < 0) m_value = 0; //所持金がマイナスにならないよう
    }
}
