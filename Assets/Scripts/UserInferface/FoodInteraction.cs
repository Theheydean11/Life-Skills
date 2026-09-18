using UnityEngine;
using TMPro;

public class FoodInteraction : MonoBehaviour
{
    // Which station the player is currently viewing. Food is only clickable
    // while this is Fridge (-> basket) or Stove (-> pan); everywhere else
    // clicks are ignored entirely.
    public enum FoodContext { None, Fridge, Stove, Cabinet }

    public static FoodInteraction Instance { get; private set; }

    [SerializeField] private Camera raycastCamera;
    [SerializeField] private float maxDistance = 200f;
    [SerializeField] private LayerMask foodLayer;

    public TMP_Text namePanelObject;

    [Header("Bottom Slot Panels")]
    [SerializeField] private TMP_Text[] bottomSlotTexts = new TMP_Text[5];

    [Header("Basket Slots")]
    [SerializeField] private Transform[] basketSlots = new Transform[5];

    [Header("Stove Counter Slots (where basket food is displayed at the stove)")]
    [SerializeField] private Transform[] counterSlots = new Transform[5];

    [Header("Ingridient Placement (2) Slots (a second station's counter, e.g. on CabinetDset)")]
    [SerializeField] private Transform[] counterSlots2 = new Transform[5];

    private FoodObject hoveredFood;
    private FoodContext currentContext = FoodContext.None;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (raycastCamera == null)
            raycastCamera = Camera.main;
    }

    private void Start(){
        
    }

    // Called by TextBoxPromptFridge / TextBoxPromptStove when the player
    // enters or exits each station's close-up interaction view. activeCamera
    // is that station's interaction camera — raycasts must originate from
    // whichever camera the player is actually looking through, since the
    // fridge and stove each use their own.
    public void SetContext(FoodContext context, Camera activeCamera = null)
    {
        currentContext = context;

        if (context != FoodContext.None && activeCamera != null)
            raycastCamera = activeCamera;
    }

    private void Update()
    {
        UpdateHover();

        // Not at the fridge or the stove — food can't be interacted with at all.
        if (currentContext == FoodContext.None) return;

        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);

        // Only hit colliders on the food layer, and only the closest one.
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, foodLayer))
        {
            FoodObject food = hit.collider.GetComponentInParent<FoodObject>();
            if (food != null)
            {
                // Locked in a pan while it cooks; clickable again once IsCooked.
                if (food.IsInPan && !food.IsCooked) return;

                // Already placed in the bowl; nothing more to do with it.
                if (food.IsInBowl) return;

                if (food == hoveredFood)
                {
                    hoveredFood = null;
                    // HoverNamePanel.Instance.Hide();
                }
            

                if (food.data == null) return;

                if (currentContext == FoodContext.Fridge){
                    PlaceFoodInBasket(food);

                }
                else if (currentContext == FoodContext.Stove){
                    if (food.IsCooked && food.IsInPan)
                        PlateFoodFromPan(food);
                    else
                        PlaceFoodInPan(food);
                }
                else if (currentContext == FoodContext.Cabinet){
                    PlaceFoodInBowl(food);
                }
            }
        }
        else
        {
            Debug.Log("No food object hit");
        }
    }

    // Fills the first empty bottom-panel slot with the clicked food's name and
    // moves the food object into the matching basket slot. Does nothing once
    // all three slots are full.
    private void PlaceFoodInBasket(FoodObject food)
    {
        for (int i = 0; i < bottomSlotTexts.Length; i++)
        {
            TMP_Text slot = bottomSlotTexts[i];
            if (slot == null || slot.text != "EMPTY") continue;

            slot.text = food.data.foodName;

            if (i < basketSlots.Length && basketSlots[i] != null)
            {
                food.transform.SetParent(basketSlots[i]);
                food.transform.localPosition = Vector3.zero;
                food.transform.localRotation = Quaternion.identity;

                Collider col = food.GetComponentInChildren<Collider>();
                if (col != null) col.enabled = false;

                SetFoodVisible(food, true);
            }

            return;
        }

        Debug.Log("Basket is full");
    }

    // Sends a counter-top food item into the first available pan.
    private void PlaceFoodInPan(FoodObject food)
    {
        if (PanManager.Instance == null)
        {
            Debug.LogWarning("No PanManager in scene — can't place food in a pan.");
            return;
        }

        PanManager.Instance.TryPlaceFood(food);
    }

    // Sends a clicked food item into the first available bowl (e.g. the
    // Straight Bowl's Place 1 / Place 2 / Place 3 slots inside CabinetDset).
    private void PlaceFoodInBowl(FoodObject food)
    {
        if (BowlManager.Instance == null)
        {
            Debug.LogWarning("No BowlManager in scene — can't place food in the bowl.");
            return;
        }

        BowlManager.Instance.TryPlaceFood(food);
    }

    // Moves a finished dish off the pan and onto the first empty plate.
    private void PlateFoodFromPan(FoodObject food)
    {
        if (PlateManager.Instance == null)
        {
            Debug.LogWarning("No PlateManager in scene — can't move cooked food to a plate.");
            return;
        }

        Pan sourcePan = food.SourcePan;
        if (!PlateManager.Instance.TryPlaceFood(food)) return;

        if (sourcePan != null)
            sourcePan.RemoveCookedDish();
    }

    // Called when the player enters the stove's close-up view. Moves every
    // food item out of the basket onto the counter (visible and clickable) —
    // the same object is reparented in place, nothing is cloned or
    // destroyed — and empties the bottom toolbar since the basket is no
    // longer the target.
    public void MoveBasketFoodToCounter() => MoveBasketFoodTo(counterSlots);

    // Same as MoveBasketFoodToCounter, but for a second station's counter
    // (e.g. Ingridient Placement (2) on the second CabinetDset). Call this
    // from that station's own close-up-view trigger instead.
    public void MoveBasketFoodToCounter2() => MoveBasketFoodTo(counterSlots2);

    private void MoveBasketFoodTo(Transform[] targetSlots)
    {
        for (int i = 0; i < basketSlots.Length; i++)
        {
            Transform basketSlot = basketSlots[i];
            if (basketSlot == null) continue;

            FoodObject food = basketSlot.GetComponentInChildren<FoodObject>();
            if (food != null)
            {
                if (i < targetSlots.Length && targetSlots[i] != null)
                {
                    food.transform.SetParent(targetSlots[i]);
                    food.transform.localPosition = Vector3.zero;
                    food.transform.localRotation = Quaternion.identity;
                }

                Collider col = food.GetComponentInChildren<Collider>();
                if (col != null) col.enabled = true;

                SetFoodVisible(food, true);

                food.transform.localScale = food.DefaultLocalScale;
            }

            if (i < bottomSlotTexts.Length && bottomSlotTexts[i] != null)
                bottomSlotTexts[i].text = "EMPTY";
        }
    }

    // Toggles the food's renderers without deactivating the GameObject
    // (which would break the GetComponentInChildren<FoodObject> lookup in
    // MoveBasketFoodToCounter, since that call doesn't include inactive
    // objects).
    private void SetFoodVisible(FoodObject food, bool visible)
    {
        foreach (Renderer renderer in food.GetComponentsInChildren<Renderer>(true))
            renderer.enabled = visible;
    }

    // Raycasts every frame (independent of clicks) to show the hovered food's
    // name on the TextMeshPro label in HoverNamePanel, hiding it once the ray leaves.
    private void UpdateHover()
    {
       
        Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);
        FoodObject food = null;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, foodLayer))
        {
            food = hit.collider.GetComponentInParent<FoodObject>();            
        }

        if (food == hoveredFood) return;

        hoveredFood = food;

        if (hoveredFood != null)
            {
                namePanelObject.text=food.data.foodName;

            // HoverNamePanel.Instance.Show(hoveredFood.data);
            }
        // else
            // HoverNamePanel.Instance.Hide();
    }
}
