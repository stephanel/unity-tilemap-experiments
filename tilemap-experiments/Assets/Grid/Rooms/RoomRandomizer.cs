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
    {
        var floorCount = GetRandomFloorsCount();

        return Enumerable.Range(FirstFloorIndex, floorCount)
            .Select(floorIndex => RandomizeRoomsOnFloor(floorIndex, lastFloorIndex: floorCount))
            .ToList();
    }

    private int GetRandomFloorsCount() => rng.Next(FloorCounts.Three, FloorCounts.Six);

    List<Room> RandomizeRoomsOnFloor(int floorIndex, int lastFloorIndex)
    {
        if (floorIndex < RoomRandomizer.FirstFloorIndex)
        {
            throw new InvalidFloorIndex();
        }

        return FloorFactory
            .Create(floorIndex, lastFloorIndex)
            .RandomizeRoomsOnFloor(totalWidth);
    }
}