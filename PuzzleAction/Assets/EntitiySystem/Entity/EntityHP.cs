using UnityEngine;

abstract public class EntityHP : MonoBehaviour
{
    private Entity m_entity;

    private int m_currentHP;
    //HPBerÇ»Ç«Ç…
    public int CurrentHP { get => m_currentHP;}

    private void Awake()
    {
        m_entity = GetComponent<Entity>();

        if (m_entity == null) return;
        m_currentHP = (int)m_entity.HP;
    }

    public void TakeDamage(int damage )
    {
        m_currentHP -= damage;

        m_currentHP = Mathf.Max( m_currentHP, 0 );

        Debug.Log($"{gameObject.name} : {damage}damage");

        Debug.Log($"åªç›HP : {m_currentHP}");

        if( m_currentHP <= 0 ) 
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        m_currentHP = Mathf.Min(m_currentHP + amount, (int)m_entity.HP);
    }

    protected abstract void Die();
}
