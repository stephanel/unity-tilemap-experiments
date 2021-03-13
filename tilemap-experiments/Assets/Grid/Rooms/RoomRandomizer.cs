using System;
using System.Collections.Generic;
using System.Linq;

public class RoomRandomizer
{
    public class FloorCounts
    {
        public const int Two = 2;
        public const int Six = 6;
    }

    private readonly Random rng = new Random();

    private int totalWidth;
    private int roomIndex = 0;

    public RoomRandomizer(int totalWidth)
    {
        this.totalWidth = totalWidth;
    }

    public List<List<Room>> Randomize()
        => RandomizeFloors();

    private List<List<Room>> RandomizeFloors()
    {
        var floorCount = rng.Next(FloorCounts.Two, FloorCounts.Six);

        return Enumerable.Range(1, floorCount)
            .Select(floorIndex => new List<Room>(RandomizeRoomsOnFloor()))
            .ToList();
        // List<List<Room>> floors = new List<List<Room>>();

        // for (int i = 0; i < floorCount; i++)
        // {
        //     floors.Add(RandomizeRoomsOnFloor());
        // }

        // return floors;
    }

    List<Room> RandomizeRoomsOnFloor()
    {
        int floorWidth = 0;

        List<Room> floor = new List<Room>();
        do
        {
            var roomWidth = rng.Next(Room.MinWidth, Room.MaxWidth);

            var availableWidth = totalWidth - floorWidth;

            roomWidth = Math.Min(roomWidth, availableWidth);

            var remainingWidthAfter = totalWidth - (floorWidth + roomWidth);
            if(remainingWidthAfter <= Room.MinWidth) {
                roomWidth += remainingWidthAfter;
            }

            var room = Room.Create($"Room{roomIndex++}", roomWidth);
            floor.Add(room);

            floorWidth += roomWidth;
        }
        while (floorWidth < this.totalWidth);

        return floor;
    }
}