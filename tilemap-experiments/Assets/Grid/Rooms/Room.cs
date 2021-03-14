using System;
using UnityEngine;

public class Room
{
    public EventHandler<OnSelectEventArgs> OnSelect;
    public class OnSelectEventArgs { }

    public string Name { get; private set; }
    public int Width { get; private set; }

    private Room() { }

    public static Room Create(string name, int width)
    {
        return new Room()
        {
            Name = name,
            Width = width
        };
    }

    // public TextMesh CreateWorldText(Transform parent = null,
    //     Vector3 localPosition = default(Vector3),
    //     int fontSize = 80,
    //     Color? color = null,
    //     TextAnchor textAnchor = TextAnchor.MiddleCenter,
    //     TextAlignment textAlignment = TextAlignment.Center,
    //     int sortingOrder = 0)
    // {
    //     GameObject gameObject = new GameObject(Name, typeof(TextMesh));

    //     Transform transform = gameObject.transform;
    //     transform.SetParent(parent, false);
    //     transform.localPosition = localPosition;

    //     TextMesh textMesh = gameObject.GetComponent<TextMesh>();
    //     textMesh.anchor = textAnchor;
    //     textMesh.alignment = textAlignment;
    //     textMesh.text = Name;
    //     textMesh.fontSize = fontSize;
    //     textMesh.color = color ?? Color.white;
    //     textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

    //     return textMesh;
    // }

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