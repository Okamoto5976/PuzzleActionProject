using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewMap", menuName = "Map/Create")]
public class MapSettingSO : ScriptableObject
{
    public string mapName;

    public Vector2Int size;
    public Vector2Int startPos;
    public Vector2Int goalPos;

    public List<Vector2Int> activeTiles;

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
}