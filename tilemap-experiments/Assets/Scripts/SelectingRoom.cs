using System.Collections.Generic;
using UnityEngine;

public class SelectingRoom : MonoBehaviour
{
    public Transform RoomsContainer;
    public Material DefaultMaterial;
    public Material HighlightingMaterial;

    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// OnMouseDown is called on mouse click and on touch on mobile
    /// </summary>
    void OnMouseDown()
    {
        resetOtherRoomsMaterial();
        highlightCurrentRoom();
    }

    void resetOtherRoomsMaterial()
    {
        // Debug.Log("rooms count=" + RoomsContainer.childCount);

        foreach (var transform in RoomsContainer)
        {
            var room = transform as Transform;

            if (room != this)
            {
                room.gameObject.GetComponent<Renderer>().material = DefaultMaterial;
            }
        }
    }

    void highlightCurrentRoom()
    {
        rend.material = HighlightingMaterial;
    }

}
