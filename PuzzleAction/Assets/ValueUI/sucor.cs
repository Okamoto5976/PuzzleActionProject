using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class sufor : MonoBehaviour
{
    public int score = 0;        //初期スコア
    private TMP_Text scoreText;  //スコア表示用UI
    int point = 0;  //追加スコア

    public void ScorePoint(int point)
    {
        this.point = point;
    }
    void Start()
    {
        score = 0;
        scoreText = GetComponent<TMP_Text>();
        scoreText.text = "score:0";
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "score:" + (score + point).ToString();
    }
}
