using UnityEngine;

[CreateAssetMenu(fileName = "MapClassData", menuName = "Scriptable Objects/MapClassData")]
public class MapClassData : ScriptableObject
{
    private MapClass m_mapClass;

    public MapClass MapClass { get => m_mapClass; }

    public void SetValue(MapClass mapClass)
    {
        m_mapClass = mapClass;
    }
}
