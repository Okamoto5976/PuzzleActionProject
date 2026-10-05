using TMPro;
using UnityEngine;

using UnityEngine.UI;
public class LevelDisplay : MonoBehaviour
{
    [Header("Level")]
    private int m_level;
   
    [SerializeField] private LevelUI levelUI;

    void Start()
    {
        m_level = GameManager.Instance.Level;

        if (levelUI != null) levelUI.UpdateScoreDisplay(m_level);

        
    }
   
    


}
