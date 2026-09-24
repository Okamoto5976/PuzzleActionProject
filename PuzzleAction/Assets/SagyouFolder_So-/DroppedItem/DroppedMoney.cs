using UnityEngine;

public class DroppedMoney : DroppedObject
{
    private int m_amount;

    private void Awake()
    {
        //amount = Random.Range(0, 100);
    }

    public override void SetValue(int value)
    {
        m_amount = value;
    }

    /// <summary>
    /// example function
    /// </summary>
    public void PrintMoney()
    {
        Debug.Log($"MONEY :{m_amount}");
    }

    public int GetMoney()
    {
        return m_amount;
    }

}
