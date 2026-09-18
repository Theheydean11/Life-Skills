using UnityEngine;

// Attach to a window's latch GameObject. Requires a Collider so OnMouseDown
// fires. Clicking the latch flips it between its down (locked) and up
// (unlocked) positions and locks/unlocks the linked WindowOpen to match, so
// the window can only be opened once the latch has been flipped up.
[RequireComponent(typeof(Collider))]
public class WindowLatch : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float upOffset = 0.05f; // local distance the latch rises when flipped up
    [SerializeField] private float moveSpeed = 0.5f; // units per second

    [Header("Window")]
    [Tooltip("Window this latch locks. Down = window locked; up = window unlocked.")]
    [SerializeField] private WindowOpen window;

    private Vector3 downLocalPosition;
    private Vector3 upLocalPosition;
    private bool isUp;

    public bool IsUp => isUp;

    private void Awake()
    {
        downLocalPosition = transform.localPosition;
        upLocalPosition = downLocalPosition + Vector3.up * upOffset;
        ApplyLock();
    }

    private void Update()
    {
        Vector3 target = isUp ? upLocalPosition : downLocalPosition;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, moveSpeed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        isUp = !isUp; // clicking again while up flips it back down
        ApplyLock();
    }

    private void ApplyLock()
    {
        if (window == null)
            return;

        if (isUp)
            window.Unlock();
        else
            window.Lock();
    }
}
