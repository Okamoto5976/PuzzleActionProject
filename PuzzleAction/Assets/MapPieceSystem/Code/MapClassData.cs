using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapClassData", menuName = "Scriptable Objects/MapClassData")]
public class MapClassData : ScriptableObject
{
    private MapClass m_mapClass;
    private List<RoomData> m_roomDatas;

    private Vector2Int m_goalPos;
    private Vector2Int m_startPos;

    public MapClass MapClass => m_mapClass;
    public List<RoomData> roomDatas => m_roomDatas;
    public Vector2Int GoalPos => m_goalPos;
    public Vector2Int StartPos => m_startPos;

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

    public void SetStartPos(Vector2Int startPos)
    {
        m_startPos = startPos;
    }
}
