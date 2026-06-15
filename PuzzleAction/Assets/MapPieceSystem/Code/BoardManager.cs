using System.Collections.Generic;
using UnityEngine;

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

    private MapClass m_mapClass;
    //--------------------

    private List<RoomPieceParent> m_roomPieces = new();

    //if isSelecting, glow list(queue)
    private HashSet<GridObject> m_IsGlowRooms = new();
    private HashSet<GridObject> m_currentGlowRooms = new();
    private HashSet<GridObject> m_recodeGlowRooms = new();

    private Vector2Int m_size;

    private GridObject[,] m_gridObjects;

    [SerializeField] private GameObject m_gridObj;

    [SerializeField] private Transform m_parent;

    private void Awake()
    {
        m_mapSystem = GetComponent<MapPlaceSystem>();
    }

    private void Update()
    {
        float value = Mathf.PingPong(Time.time, 1f);
        GlowGrid(value);


        Vector2Int origine = m_mapSystem.Origin;
        Room room = m_mapSystem.HaveRoom;

        m_currentGlowRooms.Clear();



        if (room == null)
        {
            foreach (var obj in m_currentGlowRooms)
            {
                if (!m_recodeGlowRooms.Contains(obj))
                {
                    obj.SetGlow(true);
                }
            }

            foreach (var obj in m_recodeGlowRooms)
            {
                if (!m_currentGlowRooms.Contains(obj))
                {
                    obj.SetGlow(false);
                }
            }

            (m_currentGlowRooms, m_recodeGlowRooms) = (m_recodeGlowRooms, m_currentGlowRooms);
            return;
        }

        for (int y = 0; y < room.Size.y; y++)
        {
            for(int x = 0; x < room.Size.x; x++)
            {
                if (room.GetFloor(x, y).State == Floor.FloorState.empty) continue; 

                Vector2Int index = new Vector2Int(origine.x + x, origine.y + y);

                if (index.x < 0 || index.x >= m_size.x ||
                    index.y < 0 || index.y >= m_size.y) continue;

                if (m_gridObjects[index.x, index.y] == null) continue;

                var gridObj = m_gridObjects[index.x, index.y];
                m_currentGlowRooms.Add(gridObj);
            }
        }

        foreach(var obj in m_currentGlowRooms)
        {
            if(!m_recodeGlowRooms.Contains(obj))
            {
                obj.SetGlow(true);
            }
        }

        foreach (var obj in m_recodeGlowRooms)
        {
            if (!m_currentGlowRooms.Contains(obj))
            {
                obj.SetGlow(false);
            }
        }

        (m_currentGlowRooms, m_recodeGlowRooms) = (m_recodeGlowRooms, m_currentGlowRooms);
    }

    public void Generate(MapClass map)
    {
        m_mapClass = map;
        Vector2Int size = map.Size;
        m_size = size;

        m_gridObjects = new GridObject[size.x, size.y];

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                var floor = m_mapClass.GetFloor(x, y);
                //Debug.Log($"{floor.State}");


                if (floor.State == Floor.FloorState.blocked) continue;


                var obj = InstantiateGridMap();
                GridObject gridObj = obj.GetComponent<GridObject>();

                gridObj.Initialize(this);
                gridObj.SetPieceIndex(new Vector2Int(x, y));
                m_gridObjects[x,y] = gridObj;
                m_IsGlowRooms.Add(gridObj);

                obj.transform.localPosition = new Vector3((x + 0.5f) * 1.5f, 0, (y + 0.5f) * 1.5f);
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
                m_gridObjects[pos.x, pos.y].SetPieceIndex(new Vector2Int(x,y));
            }
        }
    }

    private void GlowGrid(float value)
    {
        foreach(var room in m_currentGlowRooms)
        {
            room.OnGlowGrid(value);
        }
    }
}
