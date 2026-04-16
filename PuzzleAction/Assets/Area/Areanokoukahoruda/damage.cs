using System.IO.Compression;
using UnityEngine;

public class damage : MonoBehaviour
{
    public int m_AmountDamage = 1;//ダメージ量
    public float m_DamageInterval = 1.0f;//時間

    private float m_timer = 0f;//計測器

    private bool m_SlipDamageRoom = false;//入っているかどうか

    void Update()
    {
        if (m_SlipDamageRoom)
        {
            m_timer += Time.deltaTime;

            if (m_timer >= m_DamageInterval)
            {
                ApplyDamage();
                m_timer = 0f;
            }
        }
    }

    public void ActivateDamage()
    {
        m_SlipDamageRoom = !m_SlipDamageRoom;

        if (m_SlipDamageRoom)
        {
            Debug.Log("ダメージエリアに侵入(テスト)");
        }
        else
        {
            m_timer = 0f;
            Debug.Log("ダメージエリアから離脱(テスト)");

        }
    }
    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         m_timer += Time.deltaTime;
    //
    //         if (m_timer >= m_DamageInterval)
    //         {
    //             ApplyDamage();
    //             m_timer = 0f; // タイマーをリセット
    //         }
    //     }
    // }
    // private void OnTriggerExit(Collider other)
    // {
    //     m_timer = 0f;
    // }
    void ApplyDamage()
    {
        Debug.Log($"継続{m_AmountDamage}ダメ");
    }
}
