using UnityEngine;
using UnityEngine.UI;

public class RoomPiece : MonoBehaviour
{
    //component------------
    private Image m_img;
    private RoomPieceParent m_parent;
    //---------------------

    private Vector2Int m_index;

    public RoomPieceParent Parent { get => m_parent; }
    public Vector2Int Index { get => m_index; }

    private void Awake()
    {
        m_img = GetComponent<Image>();
        m_parent = GetComponentInParent<RoomPieceParent>();
    }

    //when generate piece, set index. use offset
    public void SetIndex(Vector2Int value)
    {
        m_index = value;
    }

    public void SetColor(Color color)
    {
        m_img.color = color;
    }
}
