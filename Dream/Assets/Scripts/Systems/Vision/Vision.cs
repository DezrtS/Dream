using UnityEngine;

public class Vision : MonoBehaviour
{
    [SerializeField] private float maxDistance = 100f;

    [Header("Settings")]
    [SerializeField] private float sensitivity = 1f;
    [SerializeField] private float maxLookAngle = 80f;
    [SerializeField] private float lookSpeed = 5f;

    [Header("References")]
    [SerializeField] private Transform root;

    private Quaternion targetRotation;
    private Quaternion currentRotation;
    private float pitch;
    private float yaw;

    private void Awake()
    {
        targetRotation = Quaternion.identity;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Get raw mouse input
        Vector2 targetMouseDelta = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );

        // Calculate rotation angles
        yaw += targetMouseDelta.x * sensitivity;
        pitch -= targetMouseDelta.y * sensitivity;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        // Create target rotation
        targetRotation = Quaternion.Euler(pitch, yaw, 0f);

        // Apply smoothed rotation
        currentRotation = Quaternion.Slerp(
            currentRotation,
            targetRotation,
            lookSpeed * Time.deltaTime
        );

        Vector3 currentForward = currentRotation * Vector3.forward;
        Vector3 targetForward = targetRotation * Vector3.forward;
        Debug.DrawRay(transform.position, currentForward, Color.red);
        Debug.DrawRay(transform.position, targetForward, Color.green);

        root.rotation = currentRotation;
    }

    public Vector3 GetLookDirection()
    {
        return currentRotation * Vector3.forward;
    }

    public RaycastHit[] QueryVision()
    {
        if (Physics.Raycast(transform.position, GetLookDirection(), out RaycastHit hit, maxDistance))
        {
            return new RaycastHit[] { hit };
        }

        return new RaycastHit[0];
    }
}