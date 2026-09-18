using UnityEngine;

// Attach to an empty GameObject. Holds references to the tray slots
// laid out along the bottom of the Canvas (e.g. inside a Horizontal Layout Group).
public class FoodTrayManager : MonoBehaviour
{
    public static FoodTrayManager Instance { get; private set; }

    [Header("Tray Slots (assign in Inspector, left to right)")]
    [SerializeField] private FoodTraySlot[] slots;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public bool AddFood(FoodData data)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsOccupied)
            {
                slot.SetFood(data);
                return true;
            }
        }

        Debug.Log("Food tray full — no empty slots left.");
        return false;
    }

    public void ClearTray()
    {
        foreach (var slot in slots)
        {
            slot.Clear();
        }
    }
}
