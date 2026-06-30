using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int m_maxHP = 100;

    public int CurrentHP { get; private set; }

    private void Awake()
    {
        CurrentHP = m_maxHP;
    }

    public void Damage(int value)
    {
        CurrentHP = Mathf.Max(CurrentHP - value, 0);
    }

    public void Heal(int value)
    {
        CurrentHP = Mathf.Min(CurrentHP + value, m_maxHP);
    }

    public float NormalizedHP
    {
        get
        {
            return (float)CurrentHP / m_maxHP;
        }
    }
}