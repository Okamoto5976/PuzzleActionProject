using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    [SerializeField] private Slider HP_Fill;

    // カメラ参照のキャッシュ（動作を軽くするため）
    private Transform _mainCameraTransform;

    private void Awake()
    {
        // 最初にカメラの場所を覚えておく
        if (Camera.main != null)
        {
            _mainCameraTransform = Camera.main.transform;
        }

        
        if (HP_Fill == null)
        {
            HP_Fill = GetComponentInChildren<Slider>();
        }
    }

    public void UpdateHPBar(int currentHP, int maxHP)
    {
        if (HP_Fill != null)
        {
            HP_Fill.maxValue = maxHP;
            HP_Fill.value = currentHP;
        }
    }

    // ビルボード処理（カメラの方向を向く）
    private void LateUpdate()
    {
        if (_mainCameraTransform != null)
        {
            transform.LookAt(_mainCameraTransform);
            transform.Rotate(0, 180, 0);
        }
    }
}