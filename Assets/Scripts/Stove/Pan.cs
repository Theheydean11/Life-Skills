using System;
using System.Collections.Generic;
using UnityEngine;

// Attach to a pan GameObject (alongside PanMovement). Holds the food items
// currently cooking in this pan and how many it can hold.
public class Pan : MonoBehaviour
{
    [Header("Placement Slots (points inside the pan)")]
    [SerializeField] private Transform[] slots;

    private readonly List<FoodObject> foodItems = new List<FoodObject>();

    public bool IsFull => foodItems.Count >= slots.Length;
    public IReadOnlyList<FoodObject> FoodItems => foodItems;

    // True once FinishCooking has spawned a dish that's still sitting in the
    // pan, waiting to be plated. Blocks new raw ingredients until it's gone.
    public bool HasCookedDish { get; private set; }

    // Raised once the cooked dish has been moved onto a plate, so PanCooking
    // can unlock the burner and allow this pan to be used again.
    public event Action CookedDishRemoved;

    // Parents food into the next open slot and disables its collider so it
    // can no longer be clicked/interacted with. Returns false if the pan is
    // full or already holds a finished dish waiting to be plated.
    public bool TryAddFood(FoodObject food)
    {
        if (IsFull || HasCookedDish) return false;

        Transform slot = slots[foodItems.Count];

        food.transform.SetParent(slot);
        food.transform.localPosition = Vector3.zero;
        food.transform.localRotation = Quaternion.identity;

        Collider foodCollider = food.GetComponentInChildren<Collider>();
        if (foodCollider != null) foodCollider.enabled = false;

        food.IsInPan = true;
        foodItems.Add(food);
        return true;
    }

    // Destroys every raw ingredient currently in the pan and, if a prefab is
    // given, instantiates it in the first slot as the finished dish. Called
    // once by PanCooking when the cook timer completes.
    public FoodObject FinishCooking(GameObject cookedFoodPrefab)
    {
        foreach (FoodObject ingredient in foodItems)
        {
            if (ingredient != null) Destroy(ingredient.gameObject);
        }
        foodItems.Clear();

        if (cookedFoodPrefab == null) return null;

        Transform slot = slots.Length > 0 ? slots[0] : transform;
        GameObject dishObject = Instantiate(cookedFoodPrefab, slot.position, slot.rotation, slot);
        dishObject.transform.localPosition = Vector3.zero;
        dishObject.transform.localRotation = Quaternion.identity;

        FoodObject dish = dishObject.GetComponent<FoodObject>();
        if (dish == null) dish = dishObject.AddComponent<FoodObject>();

        dish.IsInPan = true;
        dish.IsCooked = true;
        dish.SourcePan = this;

        HasCookedDish = true;
        return dish;
    }

    // Called once the cooked dish has been moved onto a plate; frees the pan
    // up so it can cook another batch of ingredients.
    public void RemoveCookedDish()
    {
        HasCookedDish = false;
        CookedDishRemoved?.Invoke();
    }
}
