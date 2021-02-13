using System;
using UnityEngine;

public class GenericGridTesting : MonoBehaviour
{
    GenericGrid<HeatmapGridObject> grid;

    // Start is called before the first frame update
    void Start()
    {
        // https://www.youtube.com/watch?v=8jrAWtI8RXg&list=PLzDRvYVwl53uhO8yhqxcyjDImRjO9W722&index=2
        grid = new GenericGrid<HeatmapGridObject>(8, 6, 10f, () => new HeatmapGridObject());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = GetMouseWorldPosition();

            HeatmapGridObject currentObject = grid.GetGridObject(worldPosition);
            if (currentObject != null)
            {
                currentObject.addValue(5);
                grid.SetGridObject(worldPosition, currentObject);
            }
        }
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