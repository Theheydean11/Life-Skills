using UnityEngine;

// Attach to a bowl or plate GameObject. Click it (detected by TeleportClickManager,
// which searches past any unrelated colliders in the way) to send it to an assigned
// placement point (e.g. an empty "Bowl Placement" or "Plate Placement" GameObject).
public class ClickToTeleport : MonoBehaviour
{
    [Tooltip("Where this object jumps to when clicked, e.g. Bowl Placement or Plate Placement.")]
    [SerializeField] private Transform destination;

    private void Awake()
    {
        // The Kharnyx bowl/plate meshes ship without a Collider, so OnMouseDown
        // has nothing to raycast against unless one is added here.
        if (GetComponent<Collider>() != null) return;

        Renderer meshRenderer = GetComponentInChildren<Renderer>();
        if (meshRenderer == null)
        {
            Debug.LogWarning(name + " has no Renderer to size a collider from — add one manually.");
            return;
        }

        BoxCollider box = gameObject.AddComponent<BoxCollider>();
        box.center = transform.InverseTransformPoint(meshRenderer.bounds.center);
        Vector3 lossyScale = transform.lossyScale;
        box.size = new Vector3(
            meshRenderer.bounds.size.x / lossyScale.x,
            meshRenderer.bounds.size.y / lossyScale.y,
            meshRenderer.bounds.size.z / lossyScale.z);
    }

    public void Teleport()
    {
        if (destination == null)
        {
            Debug.LogWarning("No destination assigned on " + name);
            return;
        }

        transform.position = destination.position;
    }
}
