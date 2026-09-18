using UnityEngine;

// Attach to an empty GameObject. Assign every Pan in the scene, in the
// priority order clicked food should fill them (first non-full pan wins).
public class PanManager : MonoBehaviour
{
    public static PanManager Instance { get; private set; }

    [SerializeField] private Pan[] pans;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public bool TryPlaceFood(FoodObject food)
    {
        foreach (Pan pan in pans)
        {
            if (pan == null || pan.IsFull) continue;
            if (pan.TryAddFood(food)) return true;
        }

        Debug.Log("All pans are full.");
        return false;
    }
}
