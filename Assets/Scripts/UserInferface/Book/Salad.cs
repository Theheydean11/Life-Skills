using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; // Required for UI event interfaces

public class Salad : MonoBehaviour, IPointerClickHandler
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
            objectiveTMP.SetText("1: Take out the bowls from where you store them, and place it on the tables to use. 2: Do that for the intergients as well, get it from the fridge. 3: “Rinse in Cold Water – keeping the lettuce cold will keep it crisp.” 4: Creating Croutons: Cut the baguette in half lengthwise and slice into 1/4″ thick pieces and place on a baking sheet. Combine 3 Tbsp extra virgin olive oil with minced garlic. Toss the croutons with garlic-infused oil and 2 Tbsp finely grated Parmesan. Spread them evenly onto the baking sheet and bake until crisp and golden at the edges. If you want to store these for later, bake until they are fully dried out.” 5:  Creating the Dressing “Whisk together minced garlic, Dijon, Worcestershire, lemon juice, and red wine vinegar. Whisk constantly while adding oil to emulsify the dressing for a smooth and creamy (not oily) consistency. Season with 1/2 tsp salt and 1/8 tsp black pepper, or to taste.” 6: “In a large mixing bowl, add your prepared romaine lettuce, top with croutons and parmesan. Just before serving, drizzle with your dressing, and toss gently to coat the lettuce and croutons.” 6:“Serve and Enjoy!”7: Clean up the bowls, do the dishes reorganize things after eating Caesar Salad.");
            ingredientsTMP.SetText("Lettuce,Cheese,Bread,Oil,Garlic and cloves");

            RecipeBookEvents.RaiseRecipeSelected();
        }
    }
}


