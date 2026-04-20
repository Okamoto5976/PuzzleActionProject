using UnityEngine;

public class Map3DBuilder : MonoBehaviour
{
    [SerializeField] private MapDataAsset m_map;
    [SerializeField] private GameObject m_floorPrefab;
    [SerializeField] private GameObject m_wallPrefab;
    [SerializeField] private float m_tileSize = 1f;

    [ContextMenu("Build 3D Map")]
    public void Build()
    {
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        for(int y = 0;y < m_map.m_height;y++)
        {
            for(int x = 0; x < m_map.m_width;x++)
            {

                TileType type = m_map.Get(x, y);
                GameObject prefab = null;
                float height = 0f;

                switch (type)
                {
                    case TileType.Floor:
                        prefab = m_floorPrefab;
                        height = 0f;
                        break;

                    case TileType.Wall:
                        prefab = m_wallPrefab;
                        height = 0.5f;
                        break;
                }

                if (prefab == null) continue;

                Vector3 pos = new Vector3(
                    x * m_tileSize,
                    height,
                    y * m_tileSize
                );

                Instantiate(prefab, pos, Quaternion.identity, transform);
            }
        }
    }
}

