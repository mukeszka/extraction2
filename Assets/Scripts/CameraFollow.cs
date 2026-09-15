using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 3f, -5f);
    public float smoothSpeed = 10f;

    private void LateUpdate()
    {
        if (target == null) return;

        // Statikus pozícióból követi a karaktert (nem forog vele a kamera)
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1f);
    }
}