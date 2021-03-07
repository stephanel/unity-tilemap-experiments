using System.Collections.Generic;
using UnityEngine;

public class RoomsGridRenderer : MonoBehaviour
{
    RoomsGrid grid;
    Mesh mesh;
    TextMesh[,] debugTextArray;

    bool updateMesh = true;

    public void SetGrid(RoomsGrid grid)
    {
        this.grid = grid;
        UpdateVisual();

        debugTextArray = new TextMesh[grid.Width, grid.Height];

        // grid.OnGridObjectChanged += Grid_OnGridValueChanged;
    }

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void LateUpdate()
    {
        if (updateMesh)
        {
            updateMesh = false;
            UpdateVisual();
        }
    }

    void UpdateVisual()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        int roomTriangleIndex = 0;

        var floors = grid.GetFloors();

        foreach (var floor in floors)
        {
            foreach (var room in floor)
            {
                Vector3 roomWorldPosition = grid.GetWorldPosition(room);
                int x = (int)roomWorldPosition.x;
                int y = (int)roomWorldPosition.y;

                Debug.Log($"Room location... {room.Name} - ({roomWorldPosition.x};{roomWorldPosition.y})");

                RenderRoomVertices(x, y, room.Width, grid.FloorHeight,
                    out Vector3[] roomVertices,
                    out int[] roomTriangles,
                    ref roomTriangleIndex);
                vertices.AddRange(roomVertices);
                triangles.AddRange(roomTriangles);

                RenderRoomName(room, roomWorldPosition, grid.FloorHeight);

                RenderRoomWalls(x, y, room.Width, grid.FloorHeight);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.uv = new Vector2[vertices.Count];
        mesh.triangles = triangles.ToArray();

        // draw map/building walls
        int width = grid.Width;
        int height = grid.Height;
        DebugDrawLine(0, height, width, grid.FloorHeight * floors.Count);
        DebugDrawLine(width, 0, width, height);
    }

    void RenderRoomVertices(int x, int y, int width, int height,
        out Vector3[] roomVertices,
        out int[] roomTriangles,
        ref int roomTriangleIndex)
    {
        int margin = 1;

        roomVertices = new Vector3[6];
        roomVertices[0] = new Vector3(x + margin, y + margin);
        roomVertices[1] = new Vector3(x + margin, y + height - margin);
        roomVertices[2] = new Vector3(x + width - margin, y + height - margin);

        roomVertices[3] = new Vector3(x + margin, y + margin);
        roomVertices[4] = new Vector3(x + width - margin, y + height - margin);
        roomVertices[5] = new Vector3(x + width - margin, y + margin);

        roomTriangles = new int[6];
        roomTriangles[0] = roomTriangleIndex++;
        roomTriangles[1] = roomTriangleIndex++;
        roomTriangles[2] = roomTriangleIndex++;
        roomTriangles[3] = roomTriangleIndex++;
        roomTriangles[4] = roomTriangleIndex++;
        roomTriangles[5] = roomTriangleIndex++;
    }

    void RenderRoomName(Room room, Vector3 roomWorldPosition, int floorHeight)
    {
        CreateWorldText(room.Name,
            null,
            roomWorldPosition + new Vector3(room.Width, floorHeight) * .5f,
            20,
            Color.white,
            TextAnchor.MiddleCenter,
            TextAlignment.Center);
    }

    public void CreateWorldText(string text,
        Transform parent = null,
        Vector3 localPosition = default(Vector3),
        int fontSize = 80,
        Color? color = null,
        TextAnchor textAnchor = TextAnchor.MiddleCenter,
        TextAlignment textAlignment = TextAlignment.Center,
        int sortingOrder = 0)
    {
        GameObject gameObject = new GameObject(text, typeof(TextMesh));

        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;

        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color ?? Color.white;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
    }

    void RenderRoomWalls(int x, int y, int width, int height)
    {
        DebugDrawLine(x, y, x, y + height);
        DebugDrawLine(x, y, x + width, y);

    }

    void DebugDrawLine(int x0, int y0, int x1, int y1)
    {
        Debug.DrawLine(
            grid.GetWorldPosition(x0, y0),
            grid.GetWorldPosition(x1, y1),
            Color.white,
            100f);
    }

}