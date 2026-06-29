using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    [SerializeField] private Slider m_HPFill;

    //Entityの頭上に表示する場合使う
    private Transform m_mainCameraTransform;

    private void Awake()
    {
        // 最初にカメラの場所を覚えておく
        //if (Camera.main != null)
        //{
        //    m_mainCameraTransform = Camera.main.transform;
        //}

        
        if (m_HPFill == null)
        {
            m_HPFill = GetComponentInChildren<Slider>();
        }
    }

    public void UpdateHPBar(int currentHP, int maxHP)
    {
        if (m_HPFill != null)
        {
            m_HPFill.maxValue = maxHP;
            m_HPFill.value = currentHP;
        }
    }

    //ビルボード処理（カメラの方向を向く）
    //private void LateUpdate()
    //{
    //    if (m_mainCameraTransform != null)
    //    {
    //        transform.LookAt(m_mainCameraTransform);
    //        transform.Rotate(0, 180, 0);
    //    }
    //}
}