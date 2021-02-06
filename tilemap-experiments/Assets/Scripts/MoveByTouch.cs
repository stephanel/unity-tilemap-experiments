using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByTouch : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // get touch position - in screen coordinates
            Vector2 screenPosition = touch.position;

            // so, need to convert to screen to world space
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            // now worldPosition has the same z as the main camera
            // so need to set worldPosition.z to 0
            worldPosition.z = 0f;

            // move sprite
            transform.position = worldPosition;
        }
    }
}
