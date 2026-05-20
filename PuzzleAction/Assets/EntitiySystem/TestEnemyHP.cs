using UnityEngine;

public class TestEnemyHP : MonoBehaviour
{
    [SerializeField]
    private int m_hp = 100;

    public void TakeDamage(DamageData data)
    {
        m_hp -= data.Damage;

        Debug.Log(data.Attacker.name + "から" + data.Damage + "ダメージ");
        Debug.Log("残りHP:" + m_hp);

        if (m_hp <= 0)
        {
            Debug.Log("死亡");

            Destroy(gameObject);

        }
    }
}
