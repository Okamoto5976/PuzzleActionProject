//using UnityEngine;

//public class EntityHP : MonoBehaviour
//{
//    [Header("HP設定")]
//    public int maxHP = 100;
//    public int currentHP;

//    public bool IsDead => currentHP <= 0;

//    private void Start()
//    {
//        currentHP = maxHP;
//    }

//    //ダメージを受ける
//    public void TakeDamage(int damage)
//    {
//        currentHP -= damage;
//        Debug.Log("ダメージ :" + damage + "残りHP :" + currentHP);

//        //死亡処理
//        if (currentHP <= 0)
//        {
//            Die();
//        }
//    }

//    //回復
//    public void Heal(int amount)
//    {
//        currentHP += amount;

//        //最大HPを超えないように制限
//        if (currentHP > maxHP)
//        {
//            currentHP = maxHP;
//        }

//        Debug.Log("回復：" + amount + "現在HP：" + currentHP);
//    }
//    void Die()
//    {
//        Debug.Log("死亡");
//    }
//}
