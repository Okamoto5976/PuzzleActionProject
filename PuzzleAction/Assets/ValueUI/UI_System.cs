using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class NewMonoBehaviourScript : MonoBehaviour
{
    private Slider HP_Slider;
    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;

    private void Awake()
    {
        HP_Slider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        //maxHP = data.HP
        //currentHP = Player.Runtimevalue HP
        HP_Slider.maxValue = maxHP;
        HP_Slider.minValue = 0;

    }
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);
        HP_Slider.value = currentHP;
    }
    void Update()
    {
        //transform.LookAt(Camera.main.transform);
        //transform.Rotate(0, 180, 0);
        HP_Slider.value = currentHP;
    }
}
