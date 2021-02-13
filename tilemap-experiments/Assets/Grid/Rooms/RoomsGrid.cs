using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomsGrid
{
    List<List<Room>> floors;
    int floorHeight = 40;
    int width, height;
    Vector3 originPosition;

    TextMesh[,] debugTextArray;

    public RoomsGrid(List<List<Room>> floors, int floorHeight)
        : this(floors, floorHeight, Vector3.zero)
    { }

    public RoomsGrid(List<List<Room>> floors, int floorHeight, Vector3 originPosition)
    {
        this.floors = floors;
        this.floorHeight = floorHeight;
        this.originPosition = originPosition;

        width = floors.Max(floor => floor.Sum(room => room.Width));
        height = floorHeight * floors.Count;

        Debug.Log($"(width,height) = ({width},{height})");

        debugTextArray = new TextMesh[width, height];

        int x = (int)this.originPosition.x;
        int y = (int)this.originPosition.y;

        foreach (var floor in floors)
        {
            foreach (var room in floor)
            {
                Debug.Log($"{room.Name} - {room.Width} ({x};{y})");

                debugTextArray[x, y] = room.CreateWorldText(
                    null,
                    GetWorldPosition(x, y) + new Vector3(room.Width, floorHeight) * .5f,
                    20,
                    Color.white,
                    TextAnchor.MiddleCenter,
                    TextAlignment.Center);

                DebugDrawLine(x, y, x, y + floorHeight);
                DebugDrawLine(x, y, x + room.Width, y);

                x += room.Width;

                room.OnSelect += Room_OnSelect;
            }

            x = 0;
            y += floorHeight;
        }

        DebugDrawLine(0, height, width, floorHeight * floors.Count);
        DebugDrawLine(width, 0, width, height);
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

    void DebugDrawLine(int x0, int y0, int x1, int y1)
    {
        Debug.DrawLine(
            GetWorldPosition(x0, y0),
            GetWorldPosition(x1, y1),
            Color.white,
            100f);
    }

    Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) + originPosition;
    }

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
        if (x >= (int)originPosition.x && y >= (int)originPosition.y && x < width && y < height)
        {
            int? floorIndex = GetFloorIndex(x, y);

            return GetRoomOnFloor(x, floorIndex);
        }

        return default(Room);
    }

    int? GetFloorIndex(int x, int y)
    {
        int foorCeilingY = (int)originPosition.y + floorHeight;

        for (int floorIndex = 0; floorIndex < floors.Count; floorIndex++)
        {
            var floor = floors[floorIndex];

            if (y < foorCeilingY)
            {
                return floorIndex;
            }

            foorCeilingY += floorHeight;
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

