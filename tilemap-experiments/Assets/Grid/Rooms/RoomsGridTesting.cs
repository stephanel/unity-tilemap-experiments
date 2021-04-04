using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomsGridTesting : MonoBehaviour
{
    [SerializeField] private RoomsGridRenderer roomsGridRenderer;

    RoomsGrid grid;

    bool randomize = true;

    // Start is called before the first frame update
    void Start()
    {
        List<List<Room>> rooms = new List<List<Room>>();

        //= new List<List<Room>>()
        //{
        //    // 4th floor
        //    new List<Room>() {
        //        Room.Create("Room16", 20),
        //        Room.Create("Room12", 20),
        //        Room.Create("Room14", 10),
        //        Room.Create("Room13", 50),
        //        Room.Create("Room12", 20),
        //        Room.Create("Room11", 20),
        //    },
        //    // 3rd floor
        //    new List<Room>() {
        //        Room.Create("Room10", 30),
        //        Room.Create("Room9", 50),
        //        Room.Create("Room8", 60),
        //    },
        //    // 2nd floor
        //    new List<Room>() {
        //        Room.Create("Room7", 20),
        //        Room.Create("Room6", 30),
        //        Room.Create("Room5", 40),
        //        Room.Create("Room4", 50),
        //    },
        //    // 1st floor
        //    new List<Room>() {
        //        Room.Create("Room3", 60),
        //        Room.Create("Room2", 50),
        //        Room.Create("Room1", 30),
        //    }
        //};

        if (randomize)
        {
            var roomRandomizer = new RoomRandomizer(150);
            rooms = roomRandomizer.Randomize();
        }

        grid = new RoomsGrid(rooms, 20);

        roomsGridRenderer.SetGrid(grid);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = GetMouseWorldPosition();

            Room room = grid.GetGridObject(worldPosition);

            if (room != null)
            {
                room.Select();
                //     HeatmapGridObject currentObject = grid.GetGridObject(worldPosition);
                //     if (currentObject != null)
                //     {
                //         currentObject.addValue(5);
                //         grid.SetGridObject(worldPosition, currentObject);
                //     }
            }
        }
    }

    void OnApplicationQuit()
    {
        grid.Kill();
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0;
        return vec;
    }

    Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        return worldCamera.ScreenToWorldPoint(screenPosition);
    }
}