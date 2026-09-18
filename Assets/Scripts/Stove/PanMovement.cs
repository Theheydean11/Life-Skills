using UnityEngine;

// Attach to a pan GameObject. Requires a Collider so OnMouse* events fire.
// Click and hold to drag the pan with the mouse; movement is clamped to
// stay within the stovetopBounds area (e.g. a trigger collider covering the stovetop surface).
[RequireComponent(typeof(Collider))]
public class PanMovement : MonoBehaviour
{
    [Header("Stovetop Area")]
    [Tooltip("Collider marking the area the pan is allowed to move within (e.g. the stovetop surface).")]
    [SerializeField] private Collider stovetopBounds;

    private Camera mainCamera;
    private bool isDragging;
    private float dragHeight; // world Y the pan stays at while dragging
    private Vector3 grabOffset; // offset from mouse hit point to pan position at grab time

    // Set by PanCooking while food is cooking so the pan can't be dragged off the burner.
    public bool IsLocked { get; set; }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        if (IsLocked)
            return;

        isDragging = true;
        dragHeight = transform.position.y;

        if (TryGetPlanePoint(out Vector3 hitPoint))
            grabOffset = transform.position - hitPoint;
    }

    private void OnMouseDrag()
    {
        if (IsLocked)
        {
            isDragging = false;
            return;
        }

        if (!isDragging)
            return;

        if (!TryGetPlanePoint(out Vector3 hitPoint))
            return;

        Vector3 targetPosition = hitPoint + grabOffset;
        targetPosition.y = dragHeight;

        transform.position = ClampToStovetop(targetPosition);
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    // Raycasts the mouse position onto a horizontal plane at the pan's current height.
    private bool TryGetPlanePoint(out Vector3 point)
    {
        point = Vector3.zero;

        if (mainCamera == null)
            return false;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane dragPlane = new Plane(Vector3.up, new Vector3(0f, dragHeight, 0f));

        if (!dragPlane.Raycast(ray, out float distance))
            return false;

        point = ray.GetPoint(distance);
        return true;
    }

    // Keeps the pan's X/Z position inside the stovetop bounds, leaving Y untouched.
    private Vector3 ClampToStovetop(Vector3 position)
    {
        if (stovetopBounds == null)
            return position;

        Bounds bounds = stovetopBounds.bounds;
        position.x = Mathf.Clamp(position.x, bounds.min.x, bounds.max.x);
        position.z = Mathf.Clamp(position.z, bounds.min.z, bounds.max.z);
        return position;
    }
}
