using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;

    private void Awake()
    {
        //levelText = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// DisplayManagerから呼ばれるスコア表示更新用メソッド
    /// </summary>
    /// <param name="level">現在の合計スコア</param>
    public void UpdateScoreDisplay(int level)
    {
        if (levelText == null) levelText = GetComponent<TMP_Text>();

        if (levelText != null)
        {
            levelText.text = "Level:" + level.ToString();
        }
    }
}