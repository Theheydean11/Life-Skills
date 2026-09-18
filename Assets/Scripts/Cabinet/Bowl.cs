using System.Collections.Generic;
using UnityEngine;

// Attach to the bowl GameObject inside CabinetDset. Holds the ingredients
// placed into it (via its Place 1 / Place 2 / Place 3... slots) and how many it can hold.
public class Bowl : MonoBehaviour
{
    [Header("Placement Slots (Place 1, Place 2, Place 3...)")]
    [SerializeField] private Transform[] slots;

    private readonly List<FoodObject> foodItems = new List<FoodObject>();

    public bool IsFull => foodItems.Count >= slots.Length;
    public IReadOnlyList<FoodObject> FoodItems => foodItems;

    // Parents food into the next open slot and disables its collider so it
    // can no longer be clicked/interacted with. Returns false if the bowl is full.
    public bool TryAddFood(FoodObject food)
    {
        if (IsFull) return false;

        Transform slot = slots[foodItems.Count];

        food.transform.SetParent(slot);
        food.transform.localPosition = Vector3.zero;
        food.transform.localRotation = Quaternion.identity;

        Collider foodCollider = food.GetComponentInChildren<Collider>();
        if (foodCollider != null) foodCollider.enabled = false;

        food.IsInBowl = true;
        foodItems.Add(food);
        return true;
    }
}
