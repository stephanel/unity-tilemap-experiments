using UnityEngine;

public class GridUsingGenerics<TPrimitive>
{
    int width;
    int height;
    float cellSize;
    Vector3 originPosition;

    TPrimitive[,] gridArray;
    TextMesh[,] debugTextArray;

    public GridUsingGenerics(int width, int height, float cellSize)
        : this(width, height, cellSize, Vector3.zero)
    { }

    public GridUsingGenerics(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TPrimitive[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x, y] = CreateWorldText(
                    gridArray[x, y].ToString(),
                    null,
                    GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f,
                    20,
                    Color.white,
                    TextAnchor.MiddleCenter,
                    TextAlignment.Center);

                DebugDrawLine(x, y, x, y + 1);
                DebugDrawLine(x, y, x + 1, y);
            }
        }

        DebugDrawLine(0, height, width, height);
        DebugDrawLine(width, 0, width, height);
    }

    void DebugDrawLine(int x0, int y0, int x1, int y1)
    {
        Debug.DrawLine(
            GetWorldPosition(x0, y0),
            GetWorldPosition(x1, y1),
            Color.white,
            100f);
    }

    TextMesh CreateWorldText(string text,
        Transform parent = null,
        Vector3 localPosition = default(Vector3),
        int fontSize = 40,
        Color? color = null,
        TextAnchor textAnchor = TextAnchor.MiddleCenter,
        TextAlignment textAlignment = TextAlignment.Center,
        int sortingOrder = 0)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));

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

        return textMesh;
    }

    Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetValue(int x, int y, TPrimitive value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }


    public void SetValue(Vector3 worldPosition, TPrimitive value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public TPrimitive GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    public TPrimitive GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }

        return default(TPrimitive);
    }
}
