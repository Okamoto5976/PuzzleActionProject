using UnityEngine;

public class RoomObj_copy : MonoBehaviour
{
    private Vector3 m_originalPos;
    private Room m_room;

    public Vector3 OriginalPos { get => m_originalPos; }
    public Room Room { get => m_room; }

    private RoomPieceObj[] m_pieces;

    public void Init()
    {
        m_pieces = GetComponentsInChildren<RoomPieceObj>();
    }

    public void SetOriginalPos()
    {
        m_originalPos = transform.position;
    }

    public void SetRoom(Room room)
    {
        m_room = room;
    }

    public void SetIsPlace(bool value)
    {
        foreach(var piece in m_pieces)
        {
            piece.SetPlace(value);
        }
    }
}
