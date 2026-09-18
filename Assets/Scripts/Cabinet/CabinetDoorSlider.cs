using UnityEngine;

// Attach to a sliding cabinet piece (Left/Right Cabinet Frame, or a Single Cabinet).
// Click to slide it between its original (closed) position and an open position offset
// left/right along its own local X axis. Click again to slide it back. Only ever rests
// at one of those two positions, so it can never be dragged past either end.
[RequireComponent(typeof(Collider))]
public class CabinetDoorSlider : MonoBehaviour
{
    [Header("Slide")]
    [Tooltip("Local X distance to slide to when opened, relative to the starting position. Positive slides right, negative slides left.")]
    [SerializeField] private float openOffset = 0.2f;
    [Tooltip("Units per second the piece slides when toggled between open and closed.")]
    [SerializeField] private float slideSpeed = 0.5f;

    private float closedLocalX;
    private float openLocalX;
    private bool isOpen;

    private void Awake()
    {
        closedLocalX = transform.localPosition.x;
        openLocalX = closedLocalX + openOffset;
    }

    private void Update()
    {
        float targetX = isOpen ? openLocalX : closedLocalX;

        Vector3 localPos = transform.localPosition;
        localPos.x = Mathf.MoveTowards(localPos.x, targetX, slideSpeed * Time.deltaTime);
        transform.localPosition = localPos;
    }

    private void OnMouseDown()
    {
        isOpen = !isOpen;
    }
}
