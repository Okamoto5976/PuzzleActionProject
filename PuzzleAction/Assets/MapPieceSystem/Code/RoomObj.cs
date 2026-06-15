using UnityEngine;

public class RoomObj : MonoBehaviour
{
    private Vector3 m_originalPos;
    private Room m_room;
    private AreaType m_areaType;

    public Vector3 OriginalPos { get => m_originalPos; }
    public Room Room { get => m_room; }
    public AreaType AreaType { get => m_areaType; }

    private RoomPieceObj[] m_pieces;


　　//初期化　子にIsPlaceのboolを持たせる
    public void Init()
    {
        m_pieces = GetComponentsInChildren<RoomPieceObj>();
    }

    //オブジェクトがシーン上で元の場所の保存
    //オブジェクトをマウスで放した際元の場所に戻るため
    public void SetOriginalPos()
    {
        m_originalPos = transform.position;
    }

    //roomの情報を親に持たせる
    public void SetRoom(Room room)
    {
        m_room = room;
    }

    //areaTypeの情報を親に持たせる
    public void SetAreaType(AreaType type)
    {
        m_areaType = type;
    }

    //子にIsPlaceの状態を渡す
    public void SetIsPlace(bool value)
    {
        foreach(var piece in m_pieces)
        {
            piece.SetPlace(value);
        }
    }

    //子に色を渡す
    public void SetColor(Color color)
    {
        foreach(var piece in m_pieces)
        {
            piece.SetColor(color);
        }
    }
}
