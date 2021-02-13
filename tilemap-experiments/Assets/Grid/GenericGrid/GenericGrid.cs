using System;
using UnityEngine;
public class GenericGrid<TGridObject>
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs
    {
        public int x;
        public int y;
    }

    int width;
    int height;
    float cellSize;
    Vector3 originPosition;

    TGridObject[,] gridArray;
    TextMesh[,] debugTextArray;

    public GenericGrid(int width, int height, float cellSize, Func<TGridObject> createGridObject)
        : this(width, height, cellSize, Vector3.zero, createGridObject)
    { }

    public GenericGrid(int width, int height, float cellSize, Vector3 originPosition,
        Func<TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject();
            }
        }

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

        OnGridValueChanged += (object sender, OnGridValueChangedEventArgs e) =>
        {
            debugTextArray[e.x, e.y].text = gridArray[e.x, e.y].ToString();
        };
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

    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
            if (OnGridValueChanged != null)
            {
                OnGridValueChanged(this, new OnGridValueChangedEventArgs() { x = x, y = y });
            }
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }

        return default(TGridObject);
    }
}
