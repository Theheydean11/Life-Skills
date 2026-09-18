using UnityEngine;

// Attach to an empty GameObject. Assign every Plate in the scene, in the
// priority order cooked food should fill them (first empty plate wins).
public class PlateManager : MonoBehaviour
{
    public static PlateManager Instance { get; private set; }

    [SerializeField] private Plate[] plates;
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public bool TryPlaceFood(FoodObject food)
    {
        foreach (Plate plate in plates)
        {
            if (plate == null || plate.IsOccupied) continue;
            if (plate.TryAddFood(food)) return true;
        }

        Debug.Log("All plates are full.");
        return false;
    }
}
