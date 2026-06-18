using UnityEngine;
using System.Collections.Generic;

public class RoomPieceParent : MonoBehaviour
{
    //component--------------
    private RectTransform m_rect;
    private RoomPieceManager m_roomPieceManager;
    //-----------------------

    private Room m_room;

    private Vector2 m_recodePos;

    private AreaType m_areaType;

    private RoomPiece[] m_pieces;

    private List<Vector2Int> m_floorIndex = new();

    public RectTransform Rect { get => m_rect; }
    public Room Room { get => m_room; }
    public AreaType AreaType { get => m_areaType; }
    public List<Vector2Int> FloorIndex { get => m_floorIndex; }

    private void Awake()
    {
        m_rect = GetComponent<RectTransform>();
    }

    public void Init(RoomPieceManager manager)
    {
        m_pieces = GetComponentsInChildren<RoomPiece>();
        m_roomPieceManager = manager;
    }

    public void SetRoom(Room room)
    {
        m_room = room;
    }

    public void SetRecodePos()
    {
        m_recodePos = m_rect.anchoredPosition;
    }

    public void SetAreaType(AreaType type)
    {
        m_areaType = type;
    }

    public void SetFloorIndex(Vector2Int index)
    {
        m_floorIndex.Add(index);
    }

    public void ResetFloorIndex()
    {
        m_floorIndex.Clear();
    }

    //set color and give color for children
    public void SetColor(Color color)
    {
        foreach (var piece in m_pieces)
        {
            piece.SetColor(color);
        }
    }

    public void CallResetTransform()
    {
        m_roomPieceManager.OnResetTransform(this, m_recodePos);
    }
}
