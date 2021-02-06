using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{
    public float dragSpeed = 0.003f; // 0.003 looks like the best value
    public float zoomSpeed = 0.2f; // 0.2 looks like the best value
    public float orthographicSizeMin = 1f;
    public float orthographicSizeMax = 8f;


    float xMin = -3f;
    float xMax = 15.0f;
    float yMin = -1.7f;
    float yMax = 6.3f;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touchZero = Input.GetTouch(0);

            if (Input.touchCount == 1 && touchZero.phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = touchZero.deltaPosition;

                transform.Translate(-touchDeltaPosition.x * dragSpeed, -touchDeltaPosition.y * dragSpeed, 0);

                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, xMin, xMax),
                    Mathf.Clamp(transform.position.y, yMin, yMax),
                    transform.position.z
                );
            }
            else if (Input.touchCount == 2)
            {
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPosition = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPosition = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMagnitude = (touchZeroPrevPosition - touchOnePrevPosition).magnitude;
                float touchDeltaMagnitude = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMagnitude - touchDeltaMagnitude;

                if (deltaMagnitudeDiff > 0)
                {
                    Camera.main.orthographicSize += zoomSpeed;
                }
                else if (deltaMagnitudeDiff < 0)
                {
                    Camera.main.orthographicSize -= zoomSpeed;
                }

                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, orthographicSizeMin, orthographicSizeMax);
            }
        }
    }
}
