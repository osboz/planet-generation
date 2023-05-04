using UnityEngine;

public class MouseRotateZoom : MonoBehaviour
{
    public float rotationSensitivity = 1.0f; // Adjusts the speed of rotation
    public float zoomSensitivity = 5.0f; // Adjusts the speed of zooming
    public float minZoom = 10.0f; // The minimum FOV
    public float maxZoom = 90.0f; // The maximum FOV

    private Vector3 mouseReference;
    private Vector3 mouseOffset;
    private Vector3 rotation = Vector3.zero;
    private bool isRotating;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Set the mouse reference position
            mouseReference = Input.mousePosition;
            isRotating = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Stop rotating
            isRotating = false;
        }

        if (isRotating)
        {
            // Calculate the mouse offset
            mouseOffset = (Input.mousePosition - mouseReference);

            // Apply the rotation based on the mouse movement
            transform.RotateAround(transform.position, Vector3.up, rotationSensitivity * -mouseOffset.x);
            transform.RotateAround(transform.position, Vector3.forward, rotationSensitivity * -mouseOffset.y);

            // Rotate the object
            transform.Rotate(rotation);

            // Store the new mouse reference position
            mouseReference = Input.mousePosition;
        }

        // Zoom the camera
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel != 0)
        {
            float fov = mainCamera.fieldOfView;
            fov -= scrollWheel * zoomSensitivity;
            fov = Mathf.Clamp(fov, minZoom, maxZoom);
            mainCamera.fieldOfView = fov;
        }
    }
}
