using UnityEngine;

public class RoomObj : MonoBehaviour
{
    private Vector3 m_originalPos;

    public Vector3 OriginalPos { get => m_originalPos; }

    private RoomPieceObj[] m_pieces;

    public void Init()
    {
        m_pieces = GetComponentsInChildren<RoomPieceObj>();
    }

    public void SetOriginalPos()
    {
        m_originalPos = transform.position;
    }

    public void SetIsPlace(bool value)
    {
        foreach(var piece in m_pieces)
        {
            piece.SetPlace(value);
        }
    }
}
