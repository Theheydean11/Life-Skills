using UnityEngine;

// Attach to a plate GameObject. Holds the single finished dish placed on it.
public class Plate : MonoBehaviour
{
    [Tooltip("Point on the plate where the dish is centered once placed (defaults to this transform).")]
    [SerializeField] private Transform foodSlot;

    public bool IsOccupied { get; private set; }

    // Parents the cooked dish onto this plate and disables its collider so
    // it's no longer clickable. Returns false if the plate is already occupied.
    public bool TryAddFood(FoodObject food)
    {
        if (IsOccupied) return false;

        Transform slot = foodSlot != null ? foodSlot : transform;

        food.transform.SetParent(slot);
        food.transform.localPosition = Vector3.zero;
        food.transform.localRotation = Quaternion.identity;

        Collider foodCollider = food.GetComponentInChildren<Collider>();
        if (foodCollider != null) foodCollider.enabled = false;

        food.IsInPan = false;
        food.SourcePan = null;

        IsOccupied = true;
        return true;
    }
}
