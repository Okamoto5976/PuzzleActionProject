using TMPro;
using UnityEngine;

public class Mniy : MonoBehaviour
{
    private TMP_Text moneyText;

    private void Awake()
    {
        // 最初にコンポーネントを取得
        moneyText = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// DisplayManagerから呼ばれるお金表示更新用メソッド
    /// </summary>
    /// <param name="totalMoney">現在の合計所持金</param>
    public void UpdateMoneyDisplay(int totalMoney)
    {
        // テキストが未取得なら取得を試みる
        if (moneyText == null) moneyText = GetComponent<TMP_Text>();

        if (moneyText != null)
        {
            // 表示形式を整える（$: 500 のような形式）
            // "N0" を使うと 1,000 のようにカンマが入って見やすくなります
            moneyText.text = "$:" + totalMoney.ToString("N0");
        }
    }
}