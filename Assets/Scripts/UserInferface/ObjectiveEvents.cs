using System;

// The tutorial step the Objective Panel should currently be displaying.
// Values are in the order the player is expected to reach them.
public enum ObjectiveStep
{
    GoToComputer,
    GoToGmail,
    FindMomEmail,
    GoToBox,
    Done
}

// Shared signal raised whenever the player reaches a new objective step,
// mirroring RecipeBookEvents and ComputerInteractionEvents. UpdateObjective
// listens for this to keep the Objective Panel's text in sync.
public static class ObjectiveEvents
{
    public static event Action<ObjectiveStep> OnStepReached;

    public static void RaiseStepReached(ObjectiveStep step)
    {
        OnStepReached?.Invoke(step);
    }
}
