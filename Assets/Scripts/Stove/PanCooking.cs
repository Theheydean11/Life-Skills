using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Attach to a pan GameObject alongside Pan and PanMovement. Cooking starts
// automatically once the pan is full and the nearest stove knob's burner is
// lit, and locks the pan (via PanMovement.IsLocked) and its food
// (FoodObject.IsInPan) in place until it finishes. Once a batch finishes,
// the timer will not start again for that pan until its cooked dish is
// removed (moved onto a plate).
[RequireComponent(typeof(Pan))]
public class PanCooking : MonoBehaviour
{
    [Header("Cooking")]
    [Tooltip("Seconds of lit flame required to finish cooking a full pan.")]
    [SerializeField] private float cookTime = 10f;

    [Header("Recipes")]
    [Tooltip("Every dish this pan can produce. The first recipe whose ingredient set matches what's in the pan wins.")]
    [SerializeField] private CookRecipe[] recipes;

    [Header("UI")]
    [Tooltip("TextMeshPro child that shows the remaining cook time while the pan is full and its burner is lit.")]
    [SerializeField] private TMP_Text cookTimeText;

    private Pan pan;
    private PanMovement panMovement;
    private StoveKnob[] allKnobs;
    private Transform cookTimeCanvas;
    private bool isCooking;
    private bool hasCookedDish;
    private float cookedTime;
    private StoveKnob lockedKnob;

    private void Awake()
    {
        pan = GetComponent<Pan>();
        panMovement = GetComponent<PanMovement>();
        allKnobs = Object.FindObjectsByType<StoveKnob>(FindObjectsSortMode.None);

        pan.CookedDishRemoved += HandleCookedDishRemoved;

        if (cookTimeText != null)
        {
            Canvas canvas = cookTimeText.GetComponentInParent<Canvas>();
            if (canvas != null)
                cookTimeCanvas = canvas.transform;
        }

        SetCookTimeText(null);
    }

    private void OnDestroy()
    {
        if (pan != null)
            pan.CookedDishRemoved -= HandleCookedDishRemoved;
    }

    private void Update()
    {
        StoveKnob knob = NearestKnob();

        if (isCooking)
        {
            ProgressCooking(knob);
            return;
        }

        // hasCookedDish blocks a finished batch from restarting the timer
        // every frame just because the pan is still full and the burner is
        // still lit — it only clears once the dish is moved onto a plate.
        if (!hasCookedDish && pan.IsFull && knob != null && knob.IsOn)
            StartCooking();
    }

    // Only one gameplay camera (Main, Fridge, Stove View, ...) is ever
    // enabled at a time, so this keeps the countdown facing whichever one is
    // currently active instead of a single fixed rotation that only reads
    // correctly from one viewpoint.
    private void LateUpdate()
    {
        if (cookTimeCanvas == null) return;

        Camera activeCamera = null;
        float bestDepth = float.NegativeInfinity;

        foreach (Camera camera in Camera.allCameras)
        {
            if (camera.depth < bestDepth) continue;
            bestDepth = camera.depth;
            activeCamera = camera;
        }

        if (activeCamera == null) return;

        cookTimeCanvas.rotation = Quaternion.LookRotation(
            cookTimeCanvas.position - activeCamera.transform.position);
    }

    // Picks whichever knob's flame/particle object sits physically closest to
    // this pan (not the knob dial itself, which may sit elsewhere on the
    // stove), so the burner used follows the pan if it's dragged to a
    // different spot.
    private StoveKnob NearestKnob()
    {
        StoveKnob nearest = null;
        float nearestSqrDistance = float.MaxValue;

        foreach (StoveKnob candidate in allKnobs)
        {
            if (candidate == null || candidate.ParticleEffect == null) continue;

            float sqrDistance = (candidate.ParticleEffect.transform.position - transform.position).sqrMagnitude;
            if (sqrDistance >= nearestSqrDistance) continue;

            nearest = candidate;
            nearestSqrDistance = sqrDistance;
        }

        return nearest;
    }

    private void StartCooking()
    {
        isCooking = true;
        cookedTime = 0f;

        if (panMovement != null)
            panMovement.IsLocked = true;
    }

    private void ProgressCooking(StoveKnob knob)
    {
        // The flame has to stay lit to make progress; turning it off just
        // pauses cooking rather than resetting it, and hides the countdown.
        if (knob == null || !knob.IsOn)
        {
            SetCookTimeText(00);
            return;
        }

        cookedTime += Time.deltaTime;
        SetCookTimeText(cookTime - cookedTime);

        if (cookedTime >= cookTime)
            FinishCooking();
    }

    private void SetCookTimeText(float? secondsRemaining)
    {
        if (cookTimeText == null) return;

        cookTimeText.text = secondsRemaining.HasValue
            ? Mathf.CeilToInt(Mathf.Max(0f, secondsRemaining.Value)) + "s"
            : "";
    }

    private void FinishCooking()
    {
        isCooking = false;
        hasCookedDish = true;

        // Turn the burner off and lock it so it can't be relit while the
        // finished dish is still sitting in the pan.
        lockedKnob = NearestKnob();
        if (lockedKnob != null)
        {
            lockedKnob.TurnOff();
            lockedKnob.Lock();
        }

        CookRecipe recipe = ResolveRecipe();
        if (recipe == null)
        {
            if (recipes == null || recipes.Length == 0)
            {
                Debug.LogWarning($"{name}: this pan's Recipes list is empty — add a CookRecipe asset to it.", this);
            }
            else
            {
                List<string> ingredientNames = new List<string>();
                foreach (FoodObject food in pan.FoodItems)
                    ingredientNames.Add(food.data != null ? food.data.name : "(no FoodData)");

                Debug.LogWarning($"{name}: no recipe matches these ingredients [{string.Join(", ", ingredientNames)}] — double-check each recipe's Ingredients array uses the same FoodData assets and count.", this);
            }
        }
        else if (recipe.CookedFoodPrefab == null)
            Debug.LogWarning($"{name}: matched recipe '{recipe.name}' but its Cooked Food Prefab field is empty — nothing will be spawned.", this);

        pan.FinishCooking(recipe != null ? recipe.CookedFoodPrefab : null);

        if (panMovement != null)
            panMovement.IsLocked = false;

        SetCookTimeText(null);
    }

    // Matches the FoodData of everything currently in the pan (order doesn't
    // matter, duplicates do — two eggs only matches a recipe that also lists
    // two eggs) against the configured recipes.
    private CookRecipe ResolveRecipe()
    {
        List<FoodData> ingredientData = new List<FoodData>(pan.FoodItems.Count);
        foreach (FoodObject food in pan.FoodItems)
            ingredientData.Add(food.data);

        foreach (CookRecipe recipe in recipes)
        {
            if (recipe != null && recipe.Matches(ingredientData))
                return recipe;
        }

        return null;
    }

    // Called once the cooked dish has been moved onto a plate: unlocks the
    // burner and re-arms the timer so this pan can cook another batch.
    private void HandleCookedDishRemoved()
    {
        hasCookedDish = false;

        if (lockedKnob != null)
        {
            lockedKnob.Unlock();
            lockedKnob = null;
        }
    }
}
