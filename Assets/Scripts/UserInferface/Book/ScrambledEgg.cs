using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; // Required for UI event interfaces

public class ScrambledEgg : MonoBehaviour, IPointerClickHandler
{
    [Header("ObjectiveUI")]
    [SerializeField] private TextMeshProUGUI objectiveTMP;
    [SerializeField] private TextMeshProUGUI ingredientsTMP;

    // This method triggers automatically when the UI element is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log($"Player clicked on the TMP UI object: {gameObject.name}");
            objectiveTMP.SetText("  Steps: 1: Take out the bowls from where you store them, and place it on the tables to use2: Do that for the intergients as well, get it from the fridge. 3: “beat the eggs. Place them in a medium bowl, and whisk until the yolk and whites are thoroughly combined. 4: gently preheat the pan. Brush a small nonstick skillet with olive oil, or melt a little butter inside it. Warm the skillet over medium heat. 5: Finally, cook. Pour in the egg mixture, and let it cook for 1-2 minutes, undisturbed. Then, pull a rubber spatula across the bottom of the pan to form large, soft curds of scrambled eggs. Continue cooking over medium-low heat, folding and stirring the eggs every 30 seconds. As you work, make sure to scrape your spatula along the bottom and sides of the pan to continue to form curds and to prevent any part of the eggs from drying out. For a soft, creamy scramble, stop when the eggs are mostly set, but a little liquid egg remains.” (5 minutes total). Remove the pan from the heat, and season to taste with salt and pepper.6: Serve and Enjoy!");
            ingredientsTMP.SetText("Eggs Milk, waterOlive oil or butter Salt Pepper");

            RecipeBookEvents.RaiseRecipeSelected();
        }
    }
}