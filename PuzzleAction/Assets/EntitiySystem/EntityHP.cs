using UnityEngine;

abstract public class EntityHP : MonoBehaviour
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

        CurrentHP = Mathf.Max( CurrentHP, 0 );

        Debug.Log($"{gameObject.name} : {damage}damage");

        Debug.Log($"åªç›HP : {CurrentHP}");

        if( CurrentHP <= 0 ) 
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        CurrentHP = Mathf.Min(CurrentHP + amount, m_maxHP);
    }

    protected abstract void Die();
}
