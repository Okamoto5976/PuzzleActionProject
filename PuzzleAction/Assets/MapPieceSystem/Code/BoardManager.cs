using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Audio.ProcessorInstance;
using static UnityEngine.UI.Image;

public enum GridKind
{
    Enmty,
    Full,
    Blocked
}

public class BoardManager : MonoBehaviour
{
    //component-----------
    private MapPlaceSystem m_mapSystem;
    //--------------------

    private List<RoomPieceParent> m_roomPieces = new();

    private GridKind[,] m_gridState;

    private GridObject[,] m_gridObjects;

    [SerializeField] private GameObject m_gridObj;

    [SerializeField] private Transform m_parent;

    private void Awake()
    {
        m_mapSystem = GetComponent<MapPlaceSystem>();
    }

    private void Update()
    {
        Vector2Int origine = m_mapSystem.Origin;
        //if (!m_gridObjects[origine.x, origine.y]) return;
        //var gridObj = m_gridObjects[origine.x, origine.y];
    }

    public void Generate(Vector2Int size)
    {
        m_gridObjects = new GridObject[size.x, size.y];

        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                var obj = InstantiateGridMap();
                GridObject gridObj = obj.GetComponent<GridObject>();

                gridObj.Initialize(this);
                m_gridObjects[x,z] = gridObj;

                obj.transform.localPosition = new Vector3((x + 0.5f) * 1.5f, 0, (z + 0.5f) * 1.5f);
            }
        }
    }

    private GameObject InstantiateGridMap()
    {
        var obj = Instantiate(m_gridObj, m_parent);

        return obj;
    }

    public RoomPieceParent OnFindRoomPiece(Vector2Int index)
    {
        RoomPieceParent Piece = null;

        foreach(var piece in m_roomPieces)
        {
            foreach(var Index  in piece.FloorIndex)
            {
                if (Index == index)
                {   
                    Piece = piece;
                }
            }
        }

        if (Piece == null) return null;

        m_roomPieces.Remove(Piece);

        Piece.gameObject.SetActive(true);

        foreach(var Index in Piece.FloorIndex)
        {
            m_gridObjects[Index.x, Index.y].SetRemoveFloor();
        }

        Piece.ResetFloorIndex();

        return Piece;
        
    }

    public RoomPieceParent OnRoomRemove(Vector2Int index)
    {
        RoomPieceParent piece = OnFindRoomPiece(index);

        return piece;
    }

    public void OnRoomPlace(Room room, AreaType type, Vector2Int origin, RoomPieceParent piece)
    {
        m_roomPieces.Add(piece);
        piece.gameObject.SetActive(false);

        for (int y = 0; y < room.Size.y; y++)
        {
            for (int x = 0; x < room.Size.x; x++)
            {
                if (room.GetFloor(x, y).State == Floor.FloorState.empty) continue;


                Vector2Int pos = origin + new Vector2Int(x, y);

                piece.SetFloorIndex(pos);

                m_gridObjects[pos.x, pos.y].SetPlaceFloor(type);
                m_gridObjects[pos.x, pos.y].SetIndex(new Vector2Int(x,y));
            }
        }
    }
}
