using System;
using System.Collections.Generic;
using UnityEngine;

public class MapClass
{
    private Vector2Int size = new(0,0);
    public Vector2Int Size => size;
    private List<Floor> floors;
    public List<Floor> Floors => floors;

    public MapClass(in int x, in int y)
    {
        floors = new();
        size = new Vector2Int(x, y);
        Floor def = new();
        def.SetId(-1);
        def.SetState(Floor.FloorState.empty);
        for (int i = 0; i < (x + 1) * (y + 1); i++)
        {
            floors.Add(def.Clone());
        }
    }

    public void SetFloors(in List<Tuple<int, bool>> inFloors, in Vector2Int size, in Vector2Int origin)
    {
        Vector2Int newBounds = origin + size;
        if (
            newBounds.x > this.size.x ||
            newBounds.y > this.size.y
            )
        {
            Debug.LogError("Cannot Set floors at position: exceeding bounds");
            return;
        }

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Vector2Int pos = origin + new Vector2Int(x, y);
                int floorIndex = pos.x + pos.y * (this.size.x + 1);
                var roomFloorIndex = x + y * size.x;
                (int id, bool state) = inFloors[roomFloorIndex];
                floors[floorIndex].SetId(id);
                floors[floorIndex].SetState(state ? Floor.FloorState.full : Floor.FloorState.empty);
            }
        }
    }

    public void DebugPrintFloors()
    {
        for (int y = 0; y < size.y + 1; y++)
        {
            for (int x = 0; x < size.x + 1; x++)
            {
                int floorIndex = x + y * (size.x + 1);
                var floor = floors[floorIndex];
                Debug.Log($"[{x},{y}] : {floor.Id} : {floor.State}, {(floor.wallWest.State != Wall.WallState.empty ? "W" : "")}{(floor.wallSouth.State != Wall.WallState.empty ? "S" : "")}");
            }
        }
    }

    public List<Room> Rooms = new();
    private int roomID = -1;

    public bool IsRoomColliding(in Room room, in Vector2Int origin)
    {
        // check bounds
        Vector2Int newBounds = origin + room.Size;
        if (
            newBounds.x > size.x ||
            newBounds.y > size.y
            ) return true;

        // check each floor
        for (int y = 0; y < room.Size.y; y++)
        {
            for (int x = 0; x < room.Size.x; x++)
            {
                Vector2Int pos = origin + new Vector2Int(x, y);
                int floorIndex = pos.x + pos.y * (size.x + 1);
                int roomFloorIndex = x + y * room.Size.x;
                if (room.Floors[roomFloorIndex].State == Floor.FloorState.empty) continue;
                if (Floors[floorIndex].Id != -1) return true;
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
                Vector2Int pos = origin + new Vector2Int(x, y);
                int floorIndex = pos.x + pos.y * (size.x + 1);
                int roomFloorIndex = x + y * room.Size.x;
                if (room.Floors[roomFloorIndex].State == Floor.FloorState.empty) continue;
                Floors[floorIndex] = room.Floors[roomFloorIndex];
                Floors[floorIndex].SetId(roomID);
            }
        }

        UpdateFloors();
    }

    public void RemoveRoom(in int id)
    {
        bool isRoomYoungest = roomID == id;
        foreach (var floor in Floors)
        {
            if (floor.Id == id)
            {
                floor.SetId(-1);
                continue;
            }
            if (!isRoomYoungest && floor.Id > id)
            {
                floor.SetId(floor.Id - 1);
            }
        }

        Rooms.RemoveAt(id);
        roomID--;

        UpdateFloors();
    }

    public void UpdateFloors()
    {
        for (int y = 0; y < size.y + 1; y++)
        {
            for (int x = 0; x < size.x + 1; x++)
            {
                var index = x + y * (size.x + 1);
                if (Floors[index].Id == -1 || Floors[index].State == Floor.FloorState.empty)
                {
                    Floors[index].SetState(Floor.FloorState.empty);
                    Floors[index].SetId(-1);
                }

                if (x == 0)
                {
                    Floors[index].wallWest.SetState((Wall.WallState)Floors[index].State);
                }
                else
                {
                    if (Floors[index - 1].Id == Floors[index].Id)
                    {
                        Floors[index].wallWest.SetState(Wall.WallState.empty);
                    }
                    else
                    {
                        Floors[index].wallWest.SetState(Wall.WallState.full);
                    }
                }

                if (y == 0)
                {
                    Floors[index].wallSouth.SetState((Wall.WallState)Floors[index].State);
                }
                else
                {
                    if (Floors[index - (size.x + 1)].Id == Floors[index].Id)
                    {
                        Floors[index].wallSouth.SetState(Wall.WallState.empty);
                    }
                    else
                    {
                        Floors[index].wallSouth.SetState(Wall.WallState.full);
                    }
                }
            }
        }
    }
}

public class Room
{
    private Vector2Int size = new(0,0);
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
    public int Id => id;

    public void SetId(int id)
    {
        this.id = id;
    }

    public void SetState(FloorState state)
    {
        this.state = state;
    }

    public Wall wallSouth = new();
    public Wall wallWest = new();

    public Floor Clone()
    {
        Floor res = new();
        res.SetId(id);
        res.SetState(state);
        return res;
    }
}

public class Wall
{
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

