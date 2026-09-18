using UnityEngine;

// Attach to an empty GameObject. Assign every Bowl in the scene, in the
// priority order clicked food should fill them (first non-full bowl wins).
public class BowlManager : MonoBehaviour
{
    public static BowlManager Instance { get; private set; }

    [SerializeField] private Bowl[] bowls;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public bool TryPlaceFood(FoodObject food)
    {
        foreach (Bowl bowl in bowls)
        {
            if (bowl == null || bowl.IsFull) continue;
            if (bowl.TryAddFood(food)) return true;
        }

        Debug.Log("All bowls are full.");
        return false;
    }
}
