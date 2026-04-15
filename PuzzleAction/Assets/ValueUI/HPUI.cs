using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HPUI : MonoBehaviour
{
    [SerializeField] private Slider HP_Slider; 
    [SerializeField] private int maxHP;
    private int currentHP;

    private void Start()
    {
        currentHP = maxHP;
        HP_Slider.maxValue = maxHP;
        HP_Slider.value = currentHP;
    }
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);
        HP_Slider.value = currentHP;
    }
 
    public void Heal( int heal)
    {
        currentHP += heal;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        HP_Slider.value = currentHP;
    }
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
