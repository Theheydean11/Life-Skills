using UnityEngine;
using SojaExiles;

// Attach to the "Handle" child of an Int_RR_DoorFrame_01_Man door. Clicked via
// DoorHandleInteraction (only while the Door Camera view is active), it swings
// the door by playing the same Animator states the door shipped with — the
// clip itself already rotates the door outward, this just triggers it from the
// handle instead of anywhere on the door body.
//
// Unlocked by default (matches every door that doesn't gate itself behind a
// code, e.g. the Computer scene's door). KeypadScript locks its own handle in
// Awake and calls Unlock() once the correct code is entered, so a keypad door
// only becomes clickable after that — every other DoorHandle is unaffected.
public class DoorHandle : MonoBehaviour
{
    [Tooltip("Animator state played when opening (matches the door's built-in Animator Controller).")]
    [SerializeField] private string openStateName = "Opening 1";
    [Tooltip("Animator state played when closing (matches the door's built-in Animator Controller).")]
    [SerializeField] private string closeStateName = "Closing 1";

    // Fired whenever Toggle() closes the door (not on Open()/initial state).
    // KeypadScript listens for this to decide whether to relock the handle
    // based on which side of the doorway the player closed it from.
    public event System.Action DoorClosed;

    private Animator doorAnimator;
    private bool isOpen;
    private bool isLocked;

    private void Awake()
    {
        // The Handle mesh ships without a Collider, so there's nothing for
        // DoorHandleInteraction's raycast to hit until one is added here.
        if (GetComponent<Collider>() == null)
        {
            Renderer meshRenderer = GetComponentInChildren<Renderer>();
            if (meshRenderer == null)
            {
                Debug.LogWarning(name + " has no Renderer to size a collider from — add one manually.");
            }
            else
            {
                BoxCollider box = gameObject.AddComponent<BoxCollider>();
                box.center = transform.InverseTransformPoint(meshRenderer.bounds.center);
                Vector3 lossyScale = transform.lossyScale;
                box.size = new Vector3(
                    meshRenderer.bounds.size.x / lossyScale.x,
                    meshRenderer.bounds.size.y / lossyScale.y,
                    meshRenderer.bounds.size.z / lossyScale.z);
            }
        }

        doorAnimator = GetComponentInParent<Animator>();

        // The door also ships with opencloseDoor, which opens on OnMouseOver +
        // click anywhere on the door body, at any time. That conflicts with
        // handle-only, Door-Camera-gated opening, so it's turned off here.
        // opencloseDoor1 is left active on purpose.
        opencloseDoor legacyDoor = GetComponentInParent<opencloseDoor>();
        if (legacyDoor != null) legacyDoor.enabled = false;
    }

    public void Toggle()
    {
        if (isLocked) return;

        if (doorAnimator == null)
        {
            Debug.LogWarning(name + " has no Animator in its parent chain to play door states on.");
            return;
        }

        isOpen = !isOpen;
        doorAnimator.Play(isOpen ? openStateName : closeStateName);

        if (!isOpen)
            DoorClosed?.Invoke();
    }

    // Explicit open, as opposed to Toggle(), for callers that only ever want
    // to force the door open and don't track its prior state.
    public void Open()
    {
        if (doorAnimator == null)
        {
            Debug.LogWarning(name + " has no Animator in its parent chain to play door states on.");
            return;
        }

        isOpen = true;
        doorAnimator.Play(openStateName);
    }

    // Called by KeypadScript once the correct code is entered, so
    // DoorHandleInteraction's clicks start reaching Toggle() above (letting
    // the player open AND close the door by clicking the handle again).
    public void Unlock()
    {
        isLocked = false;
    }

    // Called by KeypadScript on Awake so its handle starts unusable until the
    // code is solved.
    public void Lock()
    {
        isLocked = true;
    }
}
