using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Distance")]
    public float distance = 10f;
    public float minDistance = 6f;
    public float maxDistance = 16f;
    public float zoomSpeed = 5f;

    [Header("Height")]
    public float height = 2f;

    [Header("Rotation")]
    public float mouseSensitivity = 3f;
    public float minPitch = -30f;
    public float maxPitch = 70f;

    [Header("Collision")]
    public LayerMask collisionMask;
    public float sphereRadius = 0.3f;
    public float collisionOffset = 0.2f;
    public float cameraSmooth = 10f;

    private float yaw;
    private float pitch;

    private float currentDistance;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Vector3 rot = transform.eulerAngles;

        yaw = rot.y;
        pitch = rot.x;

        currentDistance = distance;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        RotateCamera();
        HandleZoom();
        FollowTarget();
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        distance -= scroll * zoomSpeed;

        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    void FollowTarget()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 targetPos = target.position + Vector3.up * height;

        Vector3 desiredPos =
            targetPos + rotation * new Vector3(0, 0, -distance);

        Vector3 dir = (desiredPos - targetPos).normalized;

        float targetDistance = distance;

        // 카메라 충돌 체크
        if (Physics.SphereCast(
            targetPos,
            sphereRadius,
            dir,
            out RaycastHit hit,
            distance,
            collisionMask))
        {
            targetDistance = hit.distance - collisionOffset;
        }

        targetDistance = Mathf.Clamp(
            targetDistance,
            minDistance,
            distance
        );

        currentDistance = Mathf.Lerp(
            currentDistance,
            targetDistance,
            Time.deltaTime * cameraSmooth
        );

        Vector3 finalPos =
            targetPos + rotation * new Vector3(0, 0, -currentDistance);

        transform.position = finalPos;

        transform.LookAt(targetPos);
    }
}