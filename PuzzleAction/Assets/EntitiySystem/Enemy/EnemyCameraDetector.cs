using UnityEngine;

public class EnemyCameraDetector : MonoBehaviour
{
    public Camera mainCamera;
    [SerializeField] private LayerMask wallLayer;
    public bool EnemyVisible { get; private set; }
    public bool BossVisible { get; private set; }
    private bool previousEnemyVisible;
    private bool previousBossVisible;

    void Update()
    {
        bool currentEnemyVisible = IsEnemyVisible();
        bool currentBossVisible = IsBossVisible();

        if (!previousEnemyVisible && currentEnemyVisible)
        {
            Debug.Log("敵が画面内に入りました");
        }

        if (!previousBossVisible && currentBossVisible)
        {
            Debug.Log("ボスが画面内に入りました");
        }

        previousEnemyVisible = currentEnemyVisible;
        previousBossVisible = currentBossVisible;

        EnemyVisible = currentEnemyVisible;
        BossVisible = currentBossVisible;
    }

    bool IsEnemyVisible()
    {
        EnemyController[] enemies =
            FindObjectsByType<EnemyController>(FindObjectsSortMode.None);

        foreach (var enemy in enemies)
        {
            Renderer renderer = enemy.GetComponentInChildren<Renderer>();

            if (renderer == null)
                continue;

            // ① 画面内か判定
            Vector3 pos = mainCamera.WorldToViewportPoint(enemy.transform.position);

            if (pos.z <= 0 ||
                pos.x < 0 || pos.x > 1 ||
                pos.y < 0 || pos.y > 1)
            {
                continue;
            }

            // ② カメラから敵へ線を描く（デバッグ用）
            Debug.DrawLine(mainCamera.transform.position,
                           enemy.transform.position,
                           Color.red);

            // ③ 壁があるなら見えていない
            if (Physics.Linecast(mainCamera.transform.position,
                                 enemy.transform.position,
                                 wallLayer))
            {
                continue;
            }

            // ④ 画面内で壁もない
            return true;
        }

        return false;
    }
    bool IsBossVisible()
    {
        BossEnemyController[] bosses =
            FindObjectsByType<BossEnemyController>(FindObjectsSortMode.None);

        foreach (var boss in bosses)
        {
            Renderer renderer = boss.GetComponentInChildren<Renderer>();

            if (renderer == null)
                continue;

            // ① 画面内か判定
            Vector3 pos = mainCamera.WorldToViewportPoint(boss.transform.position);

            if (pos.z <= 0 ||
                pos.x < 0 || pos.x > 1 ||
                pos.y < 0 || pos.y > 1)
            {
                continue;
            }

            // ② カメラからボスへ線を描く（デバッグ用）
            Debug.DrawLine(mainCamera.transform.position,
                           boss.transform.position,
                           Color.blue);

            // ③ 壁があるなら見えていない
            if (Physics.Linecast(mainCamera.transform.position,
                                 boss.transform.position,
                                 wallLayer))
            {
                continue;
            }

            // ④ 画面内で壁もない
            return true;
        }

        return false;
    }
}