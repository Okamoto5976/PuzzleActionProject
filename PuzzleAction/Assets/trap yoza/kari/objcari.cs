using UnityEngine;

// テスト用に、指定された座標にプレハブを生成して返すだけの仮マネージャー
public class objcari : MonoBehaviour
{
    [Header("ここに共通プレハブ（Trap_Common）をドラッグ＆ドロップ")]
    public GameObject m_TrapPrefab;

    /// <summary>
    /// テスト用の仮の生成関数（ObjectConsolidationから呼ばれるやつ）
    /// </summary>
    public GameObject kari(string requestName, Vector3 spawnPos)
    {
        if (m_TrapPrefab == null)
        {
            Debug.LogError("[TEST] 共通プレハブがセットされていません！");
            return null;
        }

        // 指定された座標に共通プレハブを生成する（Pool)
        GameObject instance = Instantiate(m_TrapPrefab, spawnPos, Quaternion.identity);

        // テスト時に区別しやすいように、クローンしたオブジェクトの名前をリクエスト名に変えておく
        instance.name = requestName + "_Clone";

        return instance;
    }
}