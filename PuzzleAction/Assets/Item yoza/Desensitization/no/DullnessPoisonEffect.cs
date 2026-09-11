//using UnityEngine;
//
///// <summary>
///// 毒がかかっている最中の方向ブレ補正を管理する内部クラス
///// </summary>
//public class DullnessPoisonEffect : MonoBehaviour
//{
//    private float m_spreadAngle;
//    private float m_timer;
//
//    public void Setup(float spreadAngle, float duration)
//    {
//        m_spreadAngle = spreadAngle;
//        m_timer = duration;
//    }
//
//    private void Update()
//    {
//        m_timer -= Time.deltaTime;
//        if (m_timer <= 0f)
//        {
//            Destroy(this); // 時間経過で自動消滅
//        }
//    }
//
//    /// <summary>
//    /// ItemRecieveData などに渡す方向ベクトルを補正して返す
//    /// </summary>
//    public Vector3 GetPoisonedDirection(Vector3 originalDir)
//    {
//        if (originalDir.sqrMagnitude < 0.001f) return originalDir;
//
//        float randomX = Random.Range(-m_spreadAngle, m_spreadAngle);
//        float randomY = Random.Range(-m_spreadAngle, m_spreadAngle);
//
//        Quaternion spreadRotation = Quaternion.Euler(randomX, randomY, 0f);
//        return spreadRotation * originalDir;
//    }
//}