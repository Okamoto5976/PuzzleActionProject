using UnityEngine;

public class RoomPieceObj_copy : MonoBehaviour
{
    private bool m_isPlace;

    public bool IsPlace { get => m_isPlace; }

    public void SetPlace(bool value)
    {
        m_isPlace = value;
    }
}
