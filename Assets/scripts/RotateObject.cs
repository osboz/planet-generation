using UnityEngine;

public class RotateObject : MonoBehaviour
{

    [SerializeField] private bool rotate;
    [SerializeField] private float rotationSpeed = 10f;

    void Update()
    {
        if (!rotate) return;
        transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);

    }

    public void ToggleRotate()
    {
        rotate = !rotate;
    }


}