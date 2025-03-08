using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target; // Assign the player here in the Inspector
    public float smoothSpeed = 0.125f;
    public Vector3 offset; // Adjust this in the Inspector to control the camera position

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
