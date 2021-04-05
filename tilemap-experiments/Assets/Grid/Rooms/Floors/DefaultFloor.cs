using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DefaultFloor : BaseFloor
{
    private readonly RoomType[] excludeRoomTypes = new[] { RoomType.Attic, RoomType.Basement };

    public override RoomType GetRandomRoomType()
    {
        var roomTypes = GenerateArrayOfPossibleRoomTypes()
            .Where(roomType => !excludeRoomTypes.Contains(roomType))
            .ToArray();

        int roomType = rng.Next(0, roomTypes.Length);
        return roomTypes[roomType];
    }

    private RoomType[] GenerateArrayOfPossibleRoomTypes()
        => Enum.GetValues(typeof(RoomType))
            .Cast<RoomType>()
            .ToArray();
}
