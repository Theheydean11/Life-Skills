using System;

// The tutorial step the House scene's Objective Panel should currently be
// displaying. Placeholder names/count - rename these once the house
// objectives and their order are decided, then update UpdateHouseObjective
// to match.
public enum HouseObjectiveStep
{
    Step1,
    Step2,
    Step3,
    Step4,
    Done
}

// Shared signal raised whenever the player reaches a new House objective
// step, mirroring ObjectiveEvents for the Computer scene. UpdateHouseObjective
// listens for this to keep the House ObjectivePanel's text in sync.
public static class HouseObjectiveEvents
{
    public static event Action<HouseObjectiveStep> OnStepReached;

    public static void RaiseStepReached(HouseObjectiveStep step)
    {
        OnStepReached?.Invoke(step);
    }
}
