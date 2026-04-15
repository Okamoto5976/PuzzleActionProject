using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mniy : MonoBehaviour
{
    public int money = 0;　　　　　　　//所持金表示用UI
    private TMP_Text moneyText;        //初期所持金
 
    //所持金を増やす
    public void AddMoniy(int amount)
    {
        //マイナス値拒否
        if (amount < 0) return;
        money += amount;
        UpdateMoneyUI();
    }
    //所持金を減らす
    public void SubtractMoney(int amount)
    {
        //マイナス値拒否
        if (amount > 0) return;
        money -= amount;
        //所持金がマイナスにならないように
        if (money < 0) money = 0;
        UpdateMoneyUI();
    }
    //お金UIの更新
    private void UpdateMoneyUI()
    {
        if(moneyText != null)
        {
            moneyText.text = "所持金" + money + "円";
        }

    }
    //ゲーム開始時UIの初期化
    private void Start()
    {
        UpdateMoneyUI();
    }
}
