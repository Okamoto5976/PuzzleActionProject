using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Mniy : MonoBehaviour
{
    public int money = 0;　　　　　　　 //初期所持金
    private TMP_Text moneyText;        //所持金表示用UI

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
/*     private void Start()
     {
        
     }*/
    void Start()
    {
        money = 0;
        moneyText = GetComponent<TMP_Text>();
        moneyText.text = "$:0"; 
        UpdateMoneyUI(); 
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "$:" + money.ToString();
    }
}
