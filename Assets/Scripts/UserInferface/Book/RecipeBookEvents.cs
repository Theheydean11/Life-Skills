using System;

/// <summary>
/// Shared signal raised whenever the player clicks a recipe TMP
/// (Pancake TMP, Scramble Egg TMP or Salad TMP) in the Recipe Panel.
/// </summary>
public static class RecipeBookEvents
{
    public static event Action OnRecipeSelected;

    public static void RaiseRecipeSelected()
    {
        OnRecipeSelected?.Invoke();
    }
}
