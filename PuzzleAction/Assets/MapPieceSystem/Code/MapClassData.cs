using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapClassData", menuName = "Scriptable Objects/MapClassData")]
public class MapClassData : ScriptableObject
{
    private MapClass m_mapClass;
    private List<RoomData> m_roomDatas;

    private Vector2Int m_goalPos;

    public MapClass MapClass { get => m_mapClass; }
    public List<RoomData> roomDatas { get => m_roomDatas; }
    public Vector2Int GoalPos { get =>  m_goalPos; }

    public void SetMapClass(MapClass mapClass)
    {
        m_mapClass = mapClass;
    }

    public void SetRoomDatas(List<RoomData> roomDatas)
    {
        m_roomDatas = roomDatas;
    }

    public void SetGoalPos(Vector2Int goalPos)
    {
        m_goalPos = goalPos;
    }
}
