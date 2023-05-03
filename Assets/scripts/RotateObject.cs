using UnityEngine;

public class RotateObject : MonoBehaviour
{

    [SerializeField] private bool rotate;
    [SerializeField] private float rotationSpeed = 10f;

    //rotates the transform the script is attached to around the y-axis
    void Update()
    {
        if (!rotate) return;
        transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);

    }

    // toggles rotation
    public void ToggleRotate()
    {
        rotate = !rotate;
    }


}