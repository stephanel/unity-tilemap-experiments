using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridUsingGenericsTesting : MonoBehaviour
{
    GridUsingGenerics<int> grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new GridUsingGenerics<int>(8, 6, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = GetMouseWorldPosition();
            int currentValue = grid.GetValue(worldPosition);
            int newValue = currentValue == 0 ? 1 : 0;
            grid.SetValue(worldPosition, newValue);
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
