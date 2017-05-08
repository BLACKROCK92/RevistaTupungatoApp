using UnityEngine;
using System.Collections;

public class CameraHandler : MonoBehaviour
{

    private static readonly float PanSpeed = 7f;
    private static readonly float ZoomSpeedTouch = 0.003f;
    private static readonly float ZoomSpeedMouse = 0.5f;

    private static readonly float[] BoundsX = new float[] { 346f, 350f };
    private static readonly float[] BoundsY = new float[] { 712f, 720f };
    private static readonly float[] ZoomBounds = new float[] { 1f, 5f };

    private Camera cam;

    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only

    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only

    public GameObject RevistaObj;
    public Transform BackgroundTransform;
    float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 InitialPosition;

    void Awake()
    {
        cam = GetComponent<Camera>();
        InitialPosition = new Vector3(348f, 716f, -10f);
    }

    void Update()
    {
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            HandleTouch();
        }
        else
        {
            HandleMouse();
        }
    }

    void HandleTouch()
    {
        switch (Input.touchCount)
        {

            case 1: // Panning
                wasZoomingLastFrame = false;

                // If the touch began, capture its position and its finger ID.
                // Otherwise, if the finger ID of the touch doesn't match, skip it.
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began && cam.orthographicSize < 3f)
                {
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved && cam.orthographicSize < 3f)
                {
                    PanCamera(touch.position);
                }
                break;

            case 2: // Zooming

                Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };

                if (!wasZoomingLastFrame)
                {
                    lastZoomPositions = newPositions;
                    wasZoomingLastFrame = true;
                }
                else
                {
                    // Zoom based on the distance between the new positions compared to the 
                    // distance between the previous positions.

                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                    float offset = newDistance - oldDistance;

                    if (RevistaObj.activeSelf)
                    {
                        ZoomCamera(offset, ZoomSpeedTouch);
                    }
                    else
                    {
                        cam.orthographicSize = ZoomBounds[1];
                        cam.transform.position = InitialPosition;
                    }
                    //lastZoomPositions = newPositions;

                }
                break;

            default:
                wasZoomingLastFrame = false;
                break;
        }
    }

    void HandleMouse()
    {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0) && cam.orthographicSize < 3f)
        {
            PanCamera(Input.mousePosition);
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (RevistaObj.activeSelf)
        {
            ZoomCamera(scroll, ZoomSpeedMouse);
        }
        else
        {
            cam.orthographicSize = ZoomBounds[1];
            cam.transform.position = InitialPosition;
        }
    }

    void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, offset.y * PanSpeed, 0);

        // Perform the movement
        transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.y = Mathf.Clamp(transform.position.y, BoundsY[0], BoundsY[1]);
        transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - (offset * speed), ZoomBounds[0], ZoomBounds[1]);

        if (cam.orthographicSize >= 4f && cam.transform.position != InitialPosition)
        {

            cam.transform.position = InitialPosition;
            cam.orthographicSize = ZoomBounds[1];
        }
    }

    private void FixedUpdate()
    {
        //BackgroundTransform.position = this.transform.position - new Vector3(0f, 0f, transform.position.z);
        BackgroundTransform.position = Vector3.SmoothDamp(BackgroundTransform.transform.position, transform.position - new Vector3(0f, 0f, transform.position.z), ref velocity, smoothTime);
    }
}
