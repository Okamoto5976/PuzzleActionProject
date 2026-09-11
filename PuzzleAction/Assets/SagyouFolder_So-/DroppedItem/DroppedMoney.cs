using UnityEngine;

public class DroppedMoney : DroppedObject
{
    private int amount;

    private void Awake()
    {
        amount = Random.Range(0, 100);
    }

    public void PrintMoney()
    {
        Debug.Log($"MONEY :{amount}");
    }
}
