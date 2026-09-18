using System;
using System.Collections.Generic;
using UnityEngine;

// Place on any GameObject in the scene. On click, raycasts through every
// collider along the ray (not just the closest one) and teleports the first
// ClickToTeleport object found, so unrelated colliders (shelves, other props,
// nearby plates) sitting in front of it never block the click.
public class TeleportClickManager : MonoBehaviour
{
    [SerializeField] private Camera raycastCamera;
    [SerializeField] private float maxDistance = 200f;

    private RaycastHit[] hitBuffer = new RaycastHit[16];

    private void Awake()
    {
        if (raycastCamera == null)
            raycastCamera = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);
        int hitCount = Physics.RaycastNonAlloc(ray, hitBuffer, maxDistance);
        Array.Sort(hitBuffer, 0, hitCount, Comparer<RaycastHit>.Create((a, b) => a.distance.CompareTo(b.distance)));

        for (int i = 0; i < hitCount; i++)
        {
            ClickToTeleport target = hitBuffer[i].collider.GetComponentInParent<ClickToTeleport>();
            if (target != null)
            {
                target.Teleport();
                return;
            }
        }
    }
}
