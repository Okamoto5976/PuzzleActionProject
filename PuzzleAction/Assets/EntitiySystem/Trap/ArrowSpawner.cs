using UnityEngine;

//テスト用
public class ArrowSpawner : MonoBehaviour
{
    [SerializeField]
    private ArrowTrap m_arrowPrefab;

    private void Update()
    {
        // Spaceキー
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnArrow();
        }
    }

    private void SpawnArrow()
    {
        ArrowTrap arrow =
            Instantiate(
                m_arrowPrefab
            );

        ArrowSpawnData data = new();

        // 出現位置
        data.Position =
            transform.position;

        // 前方向
        data.Direction =
            transform.forward;

        // 飛距離
        data.Range = 10f;

        arrow.Initialize(data);
    }
}