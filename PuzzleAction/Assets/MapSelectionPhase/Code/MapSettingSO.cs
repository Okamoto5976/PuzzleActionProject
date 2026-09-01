using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewMap", menuName = "Map/Create")]
public class MapSettingSO : ScriptableObject
{
    public string mapName;
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!string.IsNullOrEmpty(mapName))
        {
            string path = AssetDatabase.GetAssetPath(this);

            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.RenameAsset(path, mapName);
            }
        }
    }
#endif

    public Vector2Int size;
    public Vector2Int startPos;
    public Vector2Int goalPos;

    [TextArea(10, 20)]
    public string mapShape;

    public bool IsActiveTile(int x, int y)
    {
        string[] lines = mapShape
            .Replace("\r", "")
            .Split('\n');

        //y = lines.Length - 1 - y;

        if (y < 0 || y >= lines.Length) return false;
        if (x < 0 || x >= lines[y].Length) return false;

        return lines[y][x] == '1';
    }

}