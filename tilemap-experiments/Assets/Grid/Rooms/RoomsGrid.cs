using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomsGrid
{
    List<List<Room>> floors;
    Dictionary<Vector2, Room> RoomByPosition = new Dictionary<Vector2, Room>();
    public int FloorHeight { get; private set; } = 40;
    public int Width { get; private set; }
    public int Height { get; private set; }
    Vector3 originPosition;

    public RoomsGrid(List<List<Room>> floors, int floorHeight)
        : this(floors, floorHeight, Vector3.zero)
    { }

    public RoomsGrid(List<List<Room>> floors, int floorHeight, Vector3 originPosition)
    {
        this.floors = floors;
        this.FloorHeight = floorHeight;
        this.originPosition = originPosition;

        Width = floors.Max(floor => floor.Sum(room => room.Width));
        Height = floorHeight * floors.Count;

        Debug.Log($"(width,height) = ({Width},{Height})");

        int x = (int)this.originPosition.x;
        int y = (int)this.originPosition.y;

        foreach (var floor in floors)
        {
            foreach (var room in floor)
            {
                Debug.Log($"{room.Name} - {room.Width} ({x};{y})");

                RoomByPosition.Add(new Vector2(x, y), room);

                x += room.Width;

                room.OnSelect += Room_OnSelect;
            }

            x = 0;
            y += floorHeight;
        }
    }

    void Room_OnSelect(object sender, Room.OnSelectEventArgs e)
    {
        Room selectedRoom = (Room)sender;

        Debug.Log($"{selectedRoom.Name} was clicked!");

        // highlight

        // unhighlight others
    }

    public void Kill()
    {
        foreach (var floor in floors)
        {
            foreach (var room in floor)
            {
                room.OnSelect -= Room_OnSelect;
            }
        }
    }

    public List<List<Room>> GetFloors() => this.floors;

    public Vector3 GetWorldPosition(Room room)
    {
        Vector2 position = RoomByPosition
            .Where(pair => pair.Value == room)
            .Select(pair => pair.Key)
            .FirstOrDefault();

        if (position == null)
        {
            Debug.Log($"No position found for room {room.Name}.");
        }

        return GetWorldPosition((int)position.x, (int)position.y);
        // return position;
    }

    public Vector3 GetWorldPosition(int x, int y)
        => new Vector3(x, y) + originPosition;

    void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x);
        y = Mathf.FloorToInt((worldPosition - originPosition).y);
    }

    // public void SetGridObject(int x, int y, TGridObject value)
    // {
    //     if (x >= 0 && y >= 0 && x < width && y < height)
    //     {
    //         gridArray[x, y] = value;
    //         debugTextArray[x, y].text = gridArray[x, y].ToString();
    //         if (OnGridValueChanged != null)
    //         {
    //             OnGridValueChanged(this, new OnGridValueChangedEventArgs() { x = x, y = y });
    //         }
    //     }
    // }

    // public void SetGridObject(Vector3 worldPosition, TGridObject value)
    // {
    //     int x, y;
    //     GetXY(worldPosition, out x, out y);
    //     SetGridObject(x, y, value);
    // }

    public Room GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public Room GetGridObject(int x, int y)
    {
        if (x >= (int)originPosition.x && y >= (int)originPosition.y && x < Width && y < Height)
        {
            int? floorIndex = GetFloorIndex(x, y);

            return GetRoomOnFloor(x, floorIndex);
        }

        return default(Room);
    }

    int? GetFloorIndex(int x, int y)
    {
        int foorCeilingY = (int)originPosition.y + FloorHeight;

        for (int floorIndex = 0; floorIndex < floors.Count; floorIndex++)
        {
            var floor = floors[floorIndex];

            if (y < foorCeilingY)
            {
                return floorIndex;
            }

            foorCeilingY += FloorHeight;
        }

        return null;
    }

    Room GetRoomOnFloor(int x, int? floorIndex)
    {
        if (floorIndex != null)
        {
            List<Room> roomsOfFloor = floors[(int)floorIndex];

            int roomX = (int)originPosition.x;

            for (int i = 0; i < roomsOfFloor.Count; i++)
            {
                var room = roomsOfFloor[i];

                if (x < roomX + room.Width)
                {
                    return room;
                }

                roomX += room.Width;
            }

        }

        return default(Room);
    }
}

