using System;
using System.Collections.Generic;
using System.Linq;

public class RoomMaxWidthSettingNotValide : System.Exception
{ }
public class RoomMinWidthSettingNotValide : System.Exception
{ }

public class RoomsTotalWidthSettingNotValide : System.Exception
{ }

public class InvalidFloorIndex : System.Exception
{ }

public class RoomRandomizer
{
    public class FloorCounts
    {
        public const int Three = 3;
        public const int Six = 6;
    }

    public const int FirstFloorIndex = 1;
    public const int MinWidth = 20; // has to be a multiple of WidthFractional
    public const int MaxWidth = 55; // has to be a multiple of WidthFractional
    public const int WidthFractional = 5;
    private readonly Random rng = new Random();

    private int totalWidth;

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
        var floorCount = GetRandomFloorsCount();

        return Enumerable.Range(FirstFloorIndex, floorCount)
            .Select(floorIndex => new List<Room>(RandomizeRoomsOnFloor(floorIndex)))
            .ToList();
    }

    private int GetRandomFloorsCount() => rng.Next(FloorCounts.Three, FloorCounts.Six);

    List<Room> RandomizeRoomsOnFloor(int floorIndex)
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

            var room = Room.Create(
                GetRandomRoomType(floorIndex), 
                roomWidth);
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

    public RoomType GetRandomRoomType(int floorIndex)
    {
        if (floorIndex < FirstFloorIndex)
        {
            throw new InvalidFloorIndex();
        }

        if (floorIndex == 1)
        {
            return RoomType.Basement;
        }

        var roomTypes = GenerateArrayOfPossibleRoomTypes()
            .Where(roomType => roomType != RoomType.Basement)
            .ToArray();

        int roomType = rng.Next(0, roomTypes.Length);
        return roomTypes[roomType];
    }

    private RoomType[] GenerateArrayOfPossibleRoomTypes() 
        => Enum.GetValues(typeof(RoomType))
            .Cast<RoomType>()
            .ToArray();
}