using UnityEngine;

public class GridObject : MonoBehaviour
{
    //component-----------------
    private Renderer m_ren;
    private BoardManager m_boardManager;

    [SerializeField] private GridFloor m_floor;
    [SerializeField] private GameObject m_grid;
    //--------------------------

    private Vector2Int m_pieceIndex;

    private bool m_isPlace;

    public Vector2Int PieceIndex { get => m_pieceIndex; }
    public bool IsPlace { get => m_isPlace; }

    private void Awake()
    {
        m_ren = GetComponent<Renderer>();
    }

    private void Start()
    {
        m_floor.gameObject.SetActive(false);
    }

    public void Initialize(BoardManager component)
    {
        m_boardManager = component;
    }

    public void SetIndex(Vector2Int value)
    {
        m_pieceIndex = value;
    }

    public void OnSelectGrid()
    {
        //mat glow
    }

    //frome call mapsystem
    public void OnPlaceFloor(Room room, AreaType type, Vector2Int origin, RoomPieceParent piece)
    {
        m_boardManager.OnRoomPlace(room, type, origin, piece);
    }

    //frome call mapsystem
    public RoomPieceParent OnRemoveFloor(Vector2Int index)
    {
        RoomPieceParent piece = m_boardManager.OnRoomRemove(index);

        if(piece == null)
        {
            return null;
        }
        m_isPlace = false;
        return piece;
    }

    public void SetPlaceFloor(AreaType type)
    {
        m_isPlace = true;
        OnShowFloor(type);
    }

    public void SetRemoveFloor()
    {
        OnHideFloor();
    }

    public void OnShowFloor(AreaType type)
    {
        //floorMaterial color chage 
        m_floor.gameObject.SetActive(true);

        Color color = Color.white;

        switch(type)
        {
            case AreaType.None:
                color = Color.white;
                break;
            case AreaType.Summon:
                color = Color.red;
                break;
            case AreaType.Shop:
                color = Color.green;
                break;
            case AreaType.Damage:
                color = Color.blue;
                break;
        }
        m_floor.SetColor(color);
    }

    public void OnHideFloor()
    {
        m_floor.gameObject.SetActive(false);
    }
}
