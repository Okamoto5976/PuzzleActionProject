using UnityEngine;

public class RoomPieceObj : MonoBehaviour
{
    //grid上に設置されているか
    //trueなら　grid上から外す場合　MapClassで　RemoveRoomを走らせる
    private bool m_isPlace;

    private Vector2Int m_index;

    public bool IsPlace { get => m_isPlace; }
    public Vector2Int Index { get => m_index; }

    private Renderer m_ren;

    private void Awake()
    {
        m_ren = GetComponent<Renderer>();
    }

    //RoomObj親から子に状態を渡す
    public void SetPlace(bool value)
    {
        m_isPlace = value;
    }

    public void SetIndex(Vector2Int value)
    {
        m_index = value;
    }

    public void SetColor(Color color)
    {
        m_ren.material.color = color;
    }
}
