using UnityEngine;

/// <summary>
/// Attach this to the Book Carpet (alongside TextBoxPromptBook, on the same
/// trigger collider). It disables the Fridge Carpet, Stove Carpet and
/// Cabinet Carpet interaction scripts until the player has interacted with
/// the Book Carpet AND clicked one of the recipe TMPs (Pancake TMP, Scramble
/// Egg TMP or Salad TMP) in the Recipe Panel.
/// </summary>
public class BookCarpetUnlockGate : MonoBehaviour
{
    [Header("Locked until the player opens the book and picks a recipe")]
    [SerializeField] private TextBoxPromptFridge fridgeInteraction;
    [SerializeField] private TextBoxPromptStove stoveInteraction;
    [SerializeField] private TextBoxPromptCabinet cabinetInteraction;

    [Header("Settings")]
    [SerializeField] private string playerTag = "Player";

    private bool isPlayerInRange;
    private bool hasInteractedWithBook;
    private bool hasSelectedRecipe;
    private bool hasUnlocked;

    private void Awake()
    {
        SetLockedInteractionsEnabled(false);
    }

    private void OnEnable()
    {
        RecipeBookEvents.OnRecipeSelected += HandleRecipeSelected;
    }

    private void OnDisable()
    {
        RecipeBookEvents.OnRecipeSelected -= HandleRecipeSelected;
    }

    private void Update()
    {
        if (!hasInteractedWithBook && isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            hasInteractedWithBook = true;
            TryUnlock();
        }
    }

    private void HandleRecipeSelected()
    {
        hasSelectedRecipe = true;
        TryUnlock();
    }

    private void TryUnlock()
    {
        if (hasUnlocked || !hasInteractedWithBook || !hasSelectedRecipe) return;

        hasUnlocked = true;
        SetLockedInteractionsEnabled(true);
    }

    private void SetLockedInteractionsEnabled(bool isEnabled)
    {
        if (fridgeInteraction != null)
            fridgeInteraction.enabled = isEnabled;

        if (stoveInteraction != null)
            stoveInteraction.enabled = isEnabled;

        if (cabinetInteraction != null)
            cabinetInteraction.enabled = isEnabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
            isPlayerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
            isPlayerInRange = false;
    }
}
