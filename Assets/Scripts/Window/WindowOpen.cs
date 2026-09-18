using UnityEngine;

// Attach to a window GameObject. Requires a Collider so OnMouseDown fires.
// Clicking the window toggles it open/closed by rotating around its hinge,
// unless it has been locked by a WindowLatch.
[RequireComponent(typeof(Collider))]
public class WindowOpen : MonoBehaviour
{
    [Header("Window")]
    [SerializeField] private Transform windowTransform;
    [SerializeField] private float openAngle = 90f; // degrees around the hinge's Y axis
    [SerializeField] private float rotateSpeed = 120f; // degrees per second

    private Quaternion closedLocalRotation;
    private Quaternion openLocalRotation;
    private bool isOpen;
    private bool isLocked;

    public bool IsOpen => isOpen;
    public bool IsLocked => isLocked;

    private void Awake()
    {
        if (windowTransform == null)
            windowTransform = transform;

        closedLocalRotation = windowTransform.localRotation;
        openLocalRotation = closedLocalRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    private void Update()
    {
        Quaternion target = isOpen ? openLocalRotation : closedLocalRotation;
        windowTransform.localRotation = Quaternion.RotateTowards(windowTransform.localRotation, target, rotateSpeed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        if (isLocked) return;

        isOpen = !isOpen; // clicking again while open swings it back closed
    }

    // Called by WindowLatch when the latch is flipped down. Swings the
    // window shut and ignores clicks until Unlock is called.
    public void Lock()
    {
        isLocked = true;
        isOpen = false;
    }

    // Called by WindowLatch when the latch is flipped up.
    public void Unlock() => isLocked = false;
}
