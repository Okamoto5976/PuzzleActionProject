using UnityEngine;

public class EntityHP : MonoBehaviour
{
    [SerializeField] private int m_maxHP = 100;
    public int CurrentHP { get; private set; }

    private void Awake()
    {
        CurrentHP = m_maxHP;
    }

    public void TakeDamage(int damage )
    {
        CurrentHP -= damage;

        if( CurrentHP <= 0 ) 
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        CurrentHP = Mathf.Min(CurrentHP + amount, m_maxHP);
    }

    protected virtual void Die() 
    {
        Destroy(gameObject);
    }
}
