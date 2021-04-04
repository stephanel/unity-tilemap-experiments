using System;
using UnityEngine;

public class Room
{
    public EventHandler<OnSelectEventArgs> OnSelect;
    public class OnSelectEventArgs { }

    public RoomType RoomType { get; private set; }
    public string Name => RoomType.ToString();
    public int Width { get; private set; }

    private Room() { }

    public static Room Create(RoomType roomType, int width)
    {
        return new Room()
        {
            RoomType = roomType,
            Width = width
        };
    }

    public void Select()
    {
        if (OnSelect != null)
        {
            OnSelect(this, new OnSelectEventArgs());
        };
    }


    public override string ToString()
    {
        return Name;
    }
}