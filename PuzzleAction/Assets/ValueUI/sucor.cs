using TMPro;
using UnityEngine;

public class sufor : MonoBehaviour
{
    private TMP_Text scoreText;

    private void Awake()
    {
        // 最初にテキストコンポーネントを取得しておく
        scoreText = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// DisplayManagerから呼ばれるスコア表示更新用メソッド
    /// </summary>
    /// <param name="totalScore">現在の合計スコア</param>
    public void UpdateScoreDisplay(int totalScore)
    {
        // テキストが未取得なら取得を試みる
        if (scoreText == null) scoreText = GetComponent<TMP_Text>();

        if (scoreText != null)
        {
            // 文字列の更新（ score:100 のような形式）
            scoreText.text = "score:" + totalScore.ToString();
        }
    }
}