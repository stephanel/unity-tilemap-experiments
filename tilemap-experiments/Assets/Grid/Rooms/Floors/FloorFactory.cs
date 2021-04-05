using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class FloorFactory
{
    public static BaseFloor Create(int floorIndex, int lastFloorIndex)
    {
        if(floorIndex == lastFloorIndex)
        {
            return new AtticFloor();
        }

        if (floorIndex == RoomRandomizer.FirstFloorIndex)
        {
            return new BasementFloor();
        }

        return new DefaultFloor();
    }
}
