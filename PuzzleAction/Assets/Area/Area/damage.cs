//using UnityEngine;

//public class damage : MonoBehaviour
//{
//    public int m_AmountDamage = 1;//ダメージ量
//    public float m_DamageInterval = 1.0f;//時間

//    private float m_timer = 0f;//計測器

//    private enm m_TaregetEntity;//エネミーのあの判定のやつ

//    void Update()
//    {
//        if (m_TaregetEntity !=null)
//        {
//            m_timer += Time.deltaTime;

//            if (m_timer >= m_DamageInterval)
//            {
//                ApplyDamage();
//                m_timer = 0f;
//            }
//        }
//    }
//    private void OnTriggerEnter(Collider other)
//    {
//        enm entity = other.GetComponent<enm>();

//        if (entity!=null)
//        {
//            m_TaregetEntity = entity;
//            m_timer = 0;
//            Debug.Log($"{other.name}がダメージエリアに侵入");
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if(other.GetComponent<enm>()==m_TaregetEntity)
//        {
//            m_TaregetEntity = null;
//            m_timer = 0;
//            Debug.Log("ダメージエリアから離脱");
//        }
//    }

//    void ApplyDamage()
//    {
//        if(m_TaregetEntity!=null)
//        {
//        m_TaregetEntity.TakeDamage(m_AmountDamage);
//        Debug.Log($"継続{m_AmountDamage}ダメ");
//        }
//    }
//}
