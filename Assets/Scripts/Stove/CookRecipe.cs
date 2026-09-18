using System.Collections.Generic;
using UnityEngine;

// Defines which exact set of ingredients (by FoodData, counting duplicates —
// e.g. two Egg entries for scrambled eggs) turns into which cooked prefab.
[CreateAssetMenu(fileName = "NewCookRecipe", menuName = "Food/Cook Recipe")]
public class CookRecipe : ScriptableObject
{
    [Tooltip("The ingredients this recipe needs, one entry per item (duplicates allowed, e.g. Egg, Egg).")]
    [SerializeField] public FoodData[] ingredients;

    [Tooltip("Prefab spawned in the pan once these ingredients finish cooking.")]
    [SerializeField] private GameObject cookedFoodPrefab;

    public GameObject CookedFoodPrefab => cookedFoodPrefab;

    // Order-independent match: true only if candidateIngredients contains
    // exactly these FoodData entries, same counts, nothing extra.
    public bool Matches(IReadOnlyList<FoodData> candidateIngredients)
    {
        if (candidateIngredients.Count != ingredients.Length) return false;

        List<FoodData> remaining = new List<FoodData>(ingredients);
        foreach (FoodData candidate in candidateIngredients)
        {
            if (!remaining.Remove(candidate)) return false;
        }

        return true;
    }
}
