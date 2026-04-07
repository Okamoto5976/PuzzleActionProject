public class MapData
{
    public int m_width;
    public int m_height;
    public TileType[,] m_tiles;

    public MapData(int w, int h)
    {
        m_width = w;
        m_height = h;
        m_tiles = new TileType[w, h];
    }
}
