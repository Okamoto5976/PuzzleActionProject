using UnityEngine;

public class RoomPieceObj : MonoBehaviour
{
    //grid上に設置されているか
    //trueなら　grid上から外す場合　MapClassで　RemoveRoomを走らせる
    private bool m_isPlace;

    public bool IsPlace { get => m_isPlace; }

    //RoomObj親から子に状態を渡す
    public void SetPlace(bool value)
    {
        m_isPlace = value;
    }
}
