
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [SerializeField] private Slider m_HPSlider;

    public void UpdateHP(int currentHP, int maxHP)
    {
        if(m_HPSlider != null)
        {
            m_HPSlider.maxValue = maxHP;
            m_HPSlider.value = currentHP;
        }
    }
}
