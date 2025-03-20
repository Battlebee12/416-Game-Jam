using UnityEngine;

public class ArrowRotator : MonoBehaviour
{
    public Transform targetObject;     // The object that should rotate to face the arrow
    public float rotationSpeed = 30f;  // Speed at which the arrow rotates (degrees per second)
    public float radius = 2f;          // Distance between the arrow and the object

    private float currentAngle = 0f;   // Tracks the arrow's rotation angle

    void Update()
    {
        // Increment the angle based on time and speed
        currentAngle += rotationSpeed * Time.deltaTime;

        // Calculate arrow's position in a circular motion
        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius;
        float y = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius;
        transform.position = targetObject.position + new Vector3(x, y, 0f);

        // Make the target object face the arrow
        Vector2 directionToArrow = (transform.position - targetObject.position).normalized;
        float angle = Mathf.Atan2(directionToArrow.y, directionToArrow.x) * Mathf.Rad2Deg;
        targetObject.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
