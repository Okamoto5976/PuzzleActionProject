using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map2DGenerator : MonoBehaviour
{
    [SerializeField] private int m_width = 16;
    [SerializeField] private int m_height = 16;

    [SerializeField]private Tilemap m_tileMap;
    [SerializeField] private Tile m_floorTile;
    [SerializeField] private Tile m_wallTile;

    [SerializeField] private MapDataAsset m_saveAsset;

    private MapData m_map;

    [ContextMenu("Generate 2D Map")]
    public void Generate()
    {
        m_map = new MapData(m_width, m_height);
        m_tileMap.ClearAllTiles();

        for (int x = 0; x < m_width; x++)
        {
            for (int y = 0; y < m_height; y++)
            {
                bool wall =
                    x == 0 || y == 0 ||
                    x == m_width - 1 || y == m_height - 1;

                m_map.m_tiles[x, y] =
                    wall ? TileType.Wall : TileType.Floor;

                m_tileMap.SetTile(
                    new Vector3Int(x, y, 0),
                    wall ? m_wallTile : m_floorTile
                );
            }
        }
    }

    [ContextMenu("Save To Asset")]
    public void Save()
    {
        m_saveAsset.m_width = m_width;
        m_saveAsset.m_height = m_height;
        m_saveAsset.m_tiles = new TileType[m_width * m_height];

        for (int y = 0; y < m_height; y++)
        {
            for (int x = 0; x < m_width; x++)
            {
                m_saveAsset.Set(x, y, m_map.m_tiles[x, y]);
            }
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(m_saveAsset);
#endif
    }
}

