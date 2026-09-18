using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; // Required for UI event interfaces

public class Pancake : MonoBehaviour, IPointerClickHandler
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
            objectiveTMP.SetText("“Heat a lightly oiled griddle or pan over medium-high heat. Pour or scoop the batter onto the griddle, using approximately 1/4 cup for each pancake; cook until bubbles form and the edges are dry, about 2 to 3 minutes.”");
            ingredientsTMP.SetText("Flour, Baking Powder, Sugar,Salt,Milk,Butter, egg");

            RecipeBookEvents.RaiseRecipeSelected();
        }
    }
}
