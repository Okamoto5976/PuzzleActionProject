using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class NewMonoBehaviourScript : MonoBehaviour
{

    [SerializeField] private Image hpfill;
    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;

    private void Awake()
    {
        if(hpfill==null)
            hpfill=GetComponentInChildren<Image>();
    }

    private void Start()
    {
        currentHP = maxHP;
        UpdateHP();

    }
    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;

        currentHP = Mathf.Clamp(currentHP - damage, 0, maxHP);
    }
    private void UpdateHP()
    {
        if (hpfill != null)
            hpfill.fillAmount = currentHP / maxHP;
    }
}
