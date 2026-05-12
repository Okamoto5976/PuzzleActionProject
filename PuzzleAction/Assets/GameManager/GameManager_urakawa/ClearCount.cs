using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ClearCount", menuName = "Scriptable Objects/ClearCount")]
public class ClearCount : ScriptableObject
{
    [SerializeField] public int m_clearcount;

    public int Value { get => m_clearcount; }

    public void SetValue(int value)
    {
        m_clearcount = value;
    }

    public void Add(int value)
    {
        m_clearcount += value;
    }
}