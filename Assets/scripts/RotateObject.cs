using UnityEngine;

public class RotateObject : MonoBehaviour
{

    [SerializeField] private bool rotate;
    [SerializeField] private bool rotateUpX;
    [SerializeField] private bool rotateDownX;
    [SerializeField] private bool rotateUpZ;
    [SerializeField] private bool rotateDownZ;
    [SerializeField] private bool rotateLeft;
    [SerializeField] private bool rotateRight;
    [SerializeField] private float rotationSpeed = 10f;

    void Update()
    {
        if(!rotate) return;
        if (rotateUpX)
            transform.RotateAround(transform.position, Vector3.right, rotationSpeed * Time.deltaTime);
        if (rotateDownX)
            transform.RotateAround(transform.position, Vector3.left, rotationSpeed * Time.deltaTime);
        if (rotateUpZ)
            transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        if (rotateDownZ)
            transform.RotateAround(transform.position, Vector3.back, rotationSpeed * Time.deltaTime);
        if (rotateLeft)
            transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        if (rotateRight)
            transform.RotateAround(transform.position, Vector3.down, rotationSpeed * Time.deltaTime);
    }
}
