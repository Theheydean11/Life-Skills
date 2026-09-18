using UnityEngine;

// Attach to a trigger volume (Box Collider, Is Trigger) covering the house's
// interior floor. Tracks whether the player is currently inside, so
// KeypadScript knows whether to relock the front door when it's closed:
// stays unlocked when closed from inside, relocks when closed from outside.
public class HouseInteriorZone : MonoBehaviour
{
    public static HouseInteriorZone Instance { get; private set; }

    [SerializeField] private string playerTag = "Player";

    public bool IsPlayerInside { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
            IsPlayerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
            IsPlayerInside = false;
    }
}
