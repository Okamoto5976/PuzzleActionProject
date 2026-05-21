using UnityEngine;
using TMPro;

public class MoneyUi : MonoBehaviour
{
    [SerializeField] private Money moneyManager;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Update()
    {
        moneyText.text = "money :" + moneyManager.m_money; 
    }
}
