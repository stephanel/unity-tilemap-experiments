using System;
using System.Collections.Generic;
using System.Linq;

public class RoomMaxWidthSettingNotValide : System.Exception
{ }
public class RoomMinWidthSettingNotValide : System.Exception
{ }

public class RoomsTotalWidthSettingNotValide : System.Exception
{ }

public class RoomRandomizer
{
    public class FloorCounts
    {
        public const int Three = 3;
        public const int Six = 6;
    }

    public const int MinWidth = 20; // has to be a multiple of WidthFractional
    public const int MaxWidth = 55; // has to be a multiple of WidthFractional
    public const int WidthFractional = 5;
    private readonly Random rng = new Random();

    private int totalWidth;
    private int roomIndex = 0;

    /// <summary>
    /// Generates rooms with random floors count and rooms width.
    /// The parameter totalWidth has to be a multiple of WidthFractional
    /// </summary>
    public RoomRandomizer(int totalWidth)
    {
        if (MinWidth % WidthFractional != 0)
        {
            throw new RoomMinWidthSettingNotValide();
        }

        if (MaxWidth % WidthFractional != 0)
        {
            throw new RoomMaxWidthSettingNotValide();
        }

        if (totalWidth % WidthFractional != 0)
        {
            throw new RoomsTotalWidthSettingNotValide();
        }

        this.totalWidth = totalWidth;
    }

    public List<List<Room>> Randomize()
        => RandomizeFloors();

    private List<List<Room>> RandomizeFloors()
    {
        var floorCount = rng.Next(FloorCounts.Three, FloorCounts.Six);

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
            var roomWidth = GetNextRandomRoomWidth();

            var availableWidth = totalWidth - floorWidth;

            roomWidth = Math.Min(roomWidth, availableWidth);

            var remainingWidthAfter = totalWidth - (floorWidth + roomWidth);
            if (remainingWidthAfter <= MinWidth)
            {
                roomWidth += remainingWidthAfter;
            }

            var room = Room.Create($"Room{roomIndex++}", roomWidth);
            floor.Add(room);

            floorWidth += roomWidth;
        }
        while (floorWidth < this.totalWidth);

        return floor;
    }

    int GetNextRandomRoomWidth()
    {
        int[] array = GenerateArrayOfPossibleWidthForRooms();
        int minutes = rng.Next(0, array.Length);
        return array[minutes];
    }

    public int[] GenerateArrayOfPossibleWidthForRooms()
    {
        List<int> possibleWidth = new List<int>();
        var value = MinWidth;

        do
        {
            possibleWidth.Add(value);
            value += WidthFractional;

        } while (value <= MaxWidth);

        return possibleWidth.ToArray();

    }
}