using UnityEngine;

// Attach to each food GameObject that FoodInteraction's mouse raycast can destroy.
[RequireComponent(typeof(BoxCollider))]
public class FoodObject : MonoBehaviour
{
    public FoodData data;

    // The scale this object was authored with, captured before anything
    // (basket/counter/pan placement) has a chance to change it. Lets other
    // scripts scale the object relative to its original size instead of
    // whatever size it happens to be at the moment.
    public Vector3 DefaultLocalScale { get; private set; }

    // True once this food item has been placed into a Pan. Interaction is
    // blocked while true, until IsCooked flips it back open (set by
    // PanCooking once the pan's timer finishes).
    public bool IsInPan { get; set; }
    public bool IsCooked { get; set; }

    // True once this food item has been placed into a Bowl. Interaction is
    // blocked while true, same as IsInPan does for pans.
    public bool IsInBowl { get; set; }

    // The pan this (cooked) dish is currently sitting in, set by
    // Pan.FinishCooking. Used to free the pan up once the dish is moved to a plate.
    public Pan SourcePan { get; set; }

    private void Awake()
    {
        DefaultLocalScale = transform.localScale;
    }
}
