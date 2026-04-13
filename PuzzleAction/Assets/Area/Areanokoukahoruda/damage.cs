using System.IO.Compression;
using UnityEngine;

public class damage : MonoBehaviour
{
    public int m_AmountDamage = 1;//ダメージ量
    public float m_DamageInterval = 1.0f;//時間

    private float m_timer = 0f;//計測器
    public void ActivateDamage()
    {
        Debug.Log($"継続ダメエリア有効だ");
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_timer += Time.deltaTime;

            if (m_timer >= m_DamageInterval)
            {
                ApplyDamage();
                m_timer = 0f; // タイマーをリセット
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        m_timer = 0f;
    }
    void ApplyDamage()
    {
        Debug.Log($"継続{m_AmountDamage}ダメ");
    }
}
