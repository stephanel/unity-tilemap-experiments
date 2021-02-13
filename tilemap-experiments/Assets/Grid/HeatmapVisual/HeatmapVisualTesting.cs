using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapVisualTesting : MonoBehaviour
{
    GenericGrid<HeatmapGridObject> grid;

    Mesh mesh;
    public bool updateMesh;

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetGrid(GenericGrid<HeatmapGridObject> grid)
    {
        this.grid = grid;
        UpdateHeatmapVisual();

        this.grid.OnGridValueChanged += Grid_OnGridValueChanged;

    }

    void Grid_OnGridValueChanged(object sender, GenericGrid<HeatmapGridObject>.OnGridValueChangedEventArgs e)
    {
        updateMesh = true;
    }

    private void LateUpdate()
    {
        if (updateMesh)
        {
            updateMesh = false;
            UpdateHeatmapVisual();
        }
    }

    void UpdateHeatmapVisual()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        // https://www.youtube.com/watch?v=8jrAWtI8RXg&list=PLzDRvYVwl53uhO8yhqxcyjDImRjO9W722&index=2
        // 17:22
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
