using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    [SerializeField] private Slider HP_Slider;

    // カメラ参照のキャッシュ（動作を軽くするため）
    private Transform _mainCameraTransform;

    private void Awake()
    {
        // 最初にカメラの場所を覚えておく
        if (Camera.main != null)
        {
            _mainCameraTransform = Camera.main.transform;
        }

        // Sliderが未設定なら自動取得を試みる（予備策）
        if (HP_Slider == null)
        {
            HP_Slider = GetComponentInChildren<Slider>();
        }
    }

    /// <summary>
    /// DisplayManagerから呼ばれる表示更新用メソッド
    /// </summary>
    public void UpdateHPBar(int currentHP, int maxHP)
    {
        if (HP_Slider != null)
        {
            HP_Slider.maxValue = maxHP;
            HP_Slider.value = currentHP;
        }
    }

    // ビルボード処理（カメラの方向を向く）
    private void LateUpdate()
    {
        if (_mainCameraTransform != null)
        {
            transform.LookAt(_mainCameraTransform);
            transform.Rotate(0, 180, 0);
            transform.rotation=Camera.main.transform.rotation;
        }
    }
}