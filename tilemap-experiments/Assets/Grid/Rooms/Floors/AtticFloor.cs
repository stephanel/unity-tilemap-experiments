using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AtticFloor : BaseFloor
{
    public override RoomType GetRandomRoomType()
    {
        return RoomType.Attic;
    }
}
