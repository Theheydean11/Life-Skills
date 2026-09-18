using System;

/// <summary>
/// Shared signal raised whenever the player opens a computer's interaction
/// view (presses E on a TextBoxPromptComputer trigger). Carries the specific
/// TextBoxPromptComputer instance so listeners can tell computers apart,
/// mirroring RecipeBookEvents in the Kitchen scene.
/// </summary>
public static class ComputerInteractionEvents
{
    public static event Action<TextBoxPromptComputer> OnComputerInteracted;

    public static void RaiseComputerInteracted(TextBoxPromptComputer source)
    {
        OnComputerInteracted?.Invoke(source);
    }
}
