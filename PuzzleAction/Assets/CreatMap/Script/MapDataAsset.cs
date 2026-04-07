using UnityEngine;

[CreateAssetMenu(fileName = "MapDataAsset", menuName = "Scriptable Objects/MapDataAsset")]
public class MapDataAsset : ScriptableObject
{
    public int m_width;
    public int m_height;
    public TileType[] m_tiles;

    public TileType Get(int x, int y)
    {
        return m_tiles[y * m_width + x];
    }

    public void Set(int x, int y, TileType value)
    {
        m_tiles[y * m_width + x] = value;
    }
}
