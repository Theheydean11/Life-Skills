using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform targetPlayer; 

    [Header("Elevation Settings")]
    [Tooltip("Distance from the player character")]
    [SerializeField] private float targetDistance = 10.0f;
    
    [Tooltip("Angle looking down at the player (0 = horizontal, 90 = top-down)")]
    [Range(0f, 90f)]
    [SerializeField] private float elevationAngle = 45f;

    [Header("Smoothing")]
    [SerializeField] private bool useSmoothing = true;
    [SerializeField] private float smoothSpeed = 0.125f; 

    private Vector3 currentOffset;

    void Start()
    {
        if (targetPlayer == null)
        {
            Debug.LogError("Camera Controller: No target player assigned!");
            return;
        }

        CalculateOffset();
    }

    // Automatically recalculates values if you adjust them in the Inspector during play mode
    void OnValidate()
    {
        CalculateOffset();
    }

    private void CalculateOffset()
    {
        // Convert the elevation angle into radians for trigonometry
        float angleRad = elevationAngle * Mathf.Deg2Rad;

        // Calculate height (Y) and horizontal depth (Z) relative to the player
        float yOffset = targetDistance * Mathf.Sin(angleRad);
        float zOffset = -targetDistance * Mathf.Cos(angleRad);

        // This creates a fixed camera direction facing forward and tilted down
        currentOffset = new Vector3(0, yOffset, zOffset);

        // Apply the exact rotation needed to look down at that specific angle
        transform.rotation = Quaternion.Euler(elevationAngle, 0f, 0f);
    }

    void LateUpdate()
    {
        if (targetPlayer == null) return;

        // Target position maintains the strict directional offset from the player
        Vector3 targetPosition = targetPlayer.position + currentOffset;

        if (useSmoothing)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        }
        else
        {
            transform.position = targetPosition;
        }
    }
}

