using System.Collections.Generic;
using System;
using UnityEngine;
using System.Drawing;

public class ClassMap
{
    private Vector2Int m_size = new(0, 0);
    public Vector2Int Size => m_size;
    private List<Floor> m_floors;
    public List<Floor> Floors => m_floors;

    public ClassMap(in int x, in int y)
    {
        m_floors = new();
        m_size = new Vector2Int(x, y);
        Floor def = new();
        def.SetID(-1);
        def.SetState(Floor.FloorState.empty);
        for (int i = 0; i < (x + 1) * (y + 1); i++)
        {
            m_floors.Add(def.Clone());
        }
    }

    public Floor GetFloor(in int x, in int y)
    {
        int floorIndex = x + y * (m_size.x + 1);
        return Floors[floorIndex];
    }

    private void SetFloor(in int x, in int y, in Floor floor)
    {
        int floorIndex = x + y * (m_size.x + 1);
        m_floors[floorIndex] = floor.Clone();
    }
    public int GetFloorID(in int x, in int y)
    {
        return GetFloor(x, y).ID;
    }

    public int GetRoomCount()
    {
        return Rooms.Count;
    }

    public Wall GetWall(in int x, in int y, in Wall.Side side)
    {
        return GetFloor(x, y).GetWall(side);
    }
    public void SetFloors(in List<Tuple<int, bool>> inFloors, in Vector2Int size, in Vector2Int origin)
    {
        Vector2Int newBounds = origin + size;
        if (
            newBounds.x > this.m_size.x ||
            newBounds.y > this.m_size.y
            )
        {
            Debug.LogError("Cannot Set floors at position: exceeding bounds");
            return;
        }

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                var roomFloorIndex = x + y * size.x;

                (int id, bool state) = inFloors[roomFloorIndex];

                Vector2Int pos = origin + new Vector2Int(x, y);

                int floorIndex = pos.x + pos.y * (this.m_size.x + 1);

                GetFloor(pos.x, pos.y).SetID(id);
                GetFloor(pos.x, pos.y).SetState(state ? Floor.FloorState.full : Floor.FloorState.empty);
            }
        }
    }

    public void DebugPrintFloors()
    {
        for (int y = 0; y < m_size.y + 1; y++)
        {
            for (int x = 0; x < m_size.x + 1; x++)
            {
                var floor = GetFloor(x, y);
                Debug.Log($"[{x},{y}] : {floor.ID} : {floor.State}, {(floor.GetWall(Wall.Side.West).State != Wall.WallState.empty ? "W" : "")}{(floor.GetWall(Wall.Side.South).State != Wall.WallState.empty ? "S" : "")}");
            }
        }
    }
    private static string FormatWallState(Wall.WallState state)
    {
        return state switch
        {
            Wall.WallState.empty => "empty",
            Wall.WallState.full => "FULL",
            Wall.WallState.door => ">>> DOOR <<<",
            _ => "UNKNOWN"
        };
    }

    public void DebugPrintWalls()
{
    Debug.Log("==== DebugPrintWalls START ====");

    for (int y = 0; y < m_size.y + 1; y++)
    {
        for (int x = 0; x < m_size.x + 1; x++)
        {
            Floor floor = GetFloor(x, y);

            Wall west  = floor.GetWall(Wall.Side.West);
            Wall south = floor.GetWall(Wall.Side.South);

            string westState  = FormatWallState(west.State);
            string southState = FormatWallState(south.State);

            Debug.Log(
                $"[Floor ({x},{y})] " +
                $"ID:{floor.ID}, State:{floor.State} | " +
                $"West:{westState}, South:{southState}"
            );
        }
    }

    Debug.Log("==== DebugPrintWalls END ====");
}

    public List<Room> Rooms = new();
    private int roomID = -1;

    public bool IsRoomColliding(in Room room, in Vector2Int origin)
    {
        // check bounds
        // if negative
        if (origin.x < 0 || origin.y < 0) return true;
        Vector2Int newBounds = origin + room.Size;
        // if extends outside
        if (newBounds.x > m_size.x || newBounds.y > m_size.y) return true;

        // check each floor
        for (int y = 0; y < room.Size.y; y++)
        {
            for (int x = 0; x < room.Size.x; x++)
            {
                Vector2Int pos = origin + new Vector2Int(x, y);
                if (room.GetFloor(x, y).State == Floor.FloorState.empty) continue;
                if (GetFloor(pos.x, pos.y).ID != -1) return true;
            }
        }

        return false;
    }

    public void PlaceRoom(in Room room, in Vector2Int origin)
    {
        roomID++;
        Rooms.Add(room);

        for (int y = 0; y < room.Size.y; y++)
        {
            for (int x = 0; x < room.Size.x; x++)
            {
                if (room.GetFloor(x, y).State == Floor.FloorState.empty) continue;
                Vector2Int pos = origin + new Vector2Int(x, y);
                SetFloor(pos.x, pos.y, room.GetFloor(x, y));
                GetFloor(pos.x, pos.y).SetID(roomID);
            }
        }

        UpdateFloors();
    }

    public void RemoveRoom(in int id)
    {
        bool isRoomYoungest = roomID == id;
        foreach (var floor in Floors)
        {
            if (floor.ID == id)
            {
                floor.SetID(-1);
                continue;
            }
            if (!isRoomYoungest && floor.ID > id)
            {
                floor.SetID(floor.ID - 1);
            }
        }

        Rooms.RemoveAt(id);
        roomID--;

        UpdateFloors();
    }

    public void UpdateFloors()
    {
        for (int y = 0; y < m_size.y + 1; y++)
        {
            for (int x = 0; x < m_size.x + 1; x++)
            {
                var floor = GetFloor(x, y);
                var wallWest = GetWall(x, y, Wall.Side.West);
                var wallSouth = GetWall(x, y, Wall.Side.South);

                if (floor.ID == -1 || floor.State == Floor.FloorState.empty)
                {
                    floor.SetState(Floor.FloorState.empty);
                    floor.SetID(-1);
                }

                if (x == 0)
                {
                    wallWest.SetState((Wall.WallState)floor.State);
                }
                else
                {

                    if (GetFloor(x - 1, y).ID == floor.ID)
                    {
                        wallWest.SetState(Wall.WallState.empty);
                    }
                    else if (floor.wallWest.State != Wall.WallState.door)
                    {
                        wallWest.SetState(Wall.WallState.full);
                    }
                    else if (floor.wallWest.State == Wall.WallState.door)
                    {
                        wallWest.SetState(Wall.WallState.door);
                    }
                }

                if (y == 0)
                {
                    wallSouth.SetState((Wall.WallState)floor.State);
                }
                else
                {
                    if (GetFloor(x, y - 1).ID == floor.ID)
                    {
                        wallSouth.SetState(Wall.WallState.empty);
                    }
                    else if(floor.wallSouth.State != Wall.WallState.door)
                    {
                        wallSouth.SetState(Wall.WallState.full);
                    }
                    else if (floor.wallSouth.State == Wall.WallState.door)
                    {
                        wallSouth.SetState(Wall.WallState.door);
                    }
                }
            }
        }
    }
}
public class Room
{
    private Vector2Int size = new(0, 0);
    public Vector2Int Size => size;
    private List<Floor> floors = new();
    public List<Floor> Floors => floors;

    public Room(in List<Floor.FloorState> floors, in Vector2Int size)
    {
        this.size = size;
        this.floors = new();
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                var floorIndex = x + y * size.x;
                Floor floor = new();
                floor.SetState(floors[floorIndex]);
                this.floors.Add(floor);
            }
        }
    }

    public Floor GetFloor(in int x, in int y)
    {
        int floorIndex = x + y * size.x;
        return floors[floorIndex];
    }
}


public class Floor
{
    public enum FloorState
    {
        empty,
        full,
        path
    }

    private FloorState state;
    public FloorState State => state;
    private int id;
    public int ID => id;

    public void SetID(int id)
    {
        this.id = id;
    }

    public void SetState(FloorState state)
    {
        this.state = state;
    }

    public Wall GetWall(Wall.Side side)
    {
        return side switch
        {
            Wall.Side.South => wallSouth,
            Wall.Side.West => wallWest,
            _ => null,
        };
    }

    public Wall wallSouth = new();
    public Wall wallWest = new();

    public Floor Clone()
    {
        Floor res = new();
        res.SetID(id);
        res.SetState(state);
        return res;
    }
}

public class Wall
{
    public enum Side
    {
        South, 
        West,
    }

    public enum WallState
    {
        empty,
        full,
        door,
    }

    private WallState state = WallState.empty;
    public WallState State => state;

    public void SetState(WallState state)
    {
        this.state = state;
    }
}
