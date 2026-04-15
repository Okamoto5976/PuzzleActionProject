using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class sufor : MonoBehaviour
{
    public int score = 0;
    private TMP_Text scoreText;

    void Start()
    {
        score = 0;
        scoreText = GetComponent<TMP_Text>();
        scoreText.text = "score:0";
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "score:" + score.ToString();
    }
}
