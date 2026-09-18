using System;
using System.Collections.Generic;
using UnityEngine;

// Central click handler for door handles. TextBoxPromptDoor calls SetActive
// whenever the Door Camera view is entered/exited, and raycasts from whichever
// camera the player is actually looking through — not Camera.main — the same
// way FoodInteraction and TeleportClickManager handle their own interaction
// cameras. One instance covers every DoorHandle in the scene (both doors).
public class DoorHandleInteraction : MonoBehaviour
{
    public static DoorHandleInteraction Instance { get; private set; }

    [SerializeField] private float maxDistance = 200f;

    private Camera raycastCamera;
    private bool isActive;
    private RaycastHit[] hitBuffer = new RaycastHit[16];

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // Called by TextBoxPromptDoor with (true, Door Camera) on entering the
    // interaction view, and (false, _) on leaving it.
    public void SetActive(bool active, Camera camera)
    {
        isActive = active;
        if (active)
            raycastCamera = camera;
    }

    private void Update()
    {
        if (!isActive || raycastCamera == null) return;
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);
        int hitCount = Physics.RaycastNonAlloc(ray, hitBuffer, maxDistance);
        Array.Sort(hitBuffer, 0, hitCount, Comparer<RaycastHit>.Create((a, b) => a.distance.CompareTo(b.distance)));

        // Walks every hit along the ray (not just the closest) so the door's
        // own body collider sitting behind/around the handle never blocks the click.
        for (int i = 0; i < hitCount; i++)
        {
            DoorHandle handle = hitBuffer[i].collider.GetComponentInParent<DoorHandle>();
            if (handle != null)
            {
                handle.Toggle();
                return;
            }
        }
    }
}
