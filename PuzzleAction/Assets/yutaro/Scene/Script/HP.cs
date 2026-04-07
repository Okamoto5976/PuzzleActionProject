using UnityEngine;

public class HP:MonoBehaviour 
{
    [Header("HP設定")]
    public int maxHP = 100;
    public int currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    //ダメージを受ける
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("ダメージ :" + damage + "残りHP :" + currentHP);
        
        //死亡処理
        if (currentHP <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("死亡");
    }
}
