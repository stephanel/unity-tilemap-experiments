using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public abstract class BaseFloor
{
    protected static readonly Random rng = new Random();

    public List<Room> RandomizeRoomsOnFloor(int totalWidth)
    {
        int floorWidth = 0;

        List<Room> floor = new List<Room>();
        do
        {
            var roomWidth = GetNextRandomRoomWidth();

            var availableWidth = totalWidth - floorWidth;

            roomWidth = Math.Min(roomWidth, availableWidth);

            var remainingWidthAfter = totalWidth - (floorWidth + roomWidth);
            if (remainingWidthAfter <= RoomRandomizer.MinWidth)
            {
                roomWidth += remainingWidthAfter;
            }

            var room = Room.Create(
                GetRandomRoomType(),
                roomWidth);
            floor.Add(room);

            floorWidth += roomWidth;
        }
        while (floorWidth < totalWidth);

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
        var value = RoomRandomizer.MinWidth;

        do
        {
            possibleWidth.Add(value);
            value += RoomRandomizer.WidthFractional;

        } while (value <= RoomRandomizer.MaxWidth);

        return possibleWidth.ToArray();

    }

    public abstract RoomType GetRandomRoomType();
} 