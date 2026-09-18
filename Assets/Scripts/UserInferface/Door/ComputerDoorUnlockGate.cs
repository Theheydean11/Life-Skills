using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach anywhere in the Computer scene. Disables the door's
/// TextBoxPromptDoor until the player has interacted with every computer
/// listed below (pressed E on each TextBoxPromptComputer at least once),
/// forcing the computer(s) to be used before the door can be opened.
/// Mirrors BookCarpetUnlockGate in the Kitchen scene.
/// </summary>
public class ComputerDoorUnlockGate : MonoBehaviour
{
    [Header("Must be interacted with before the door unlocks")]
    [SerializeField] private TextBoxPromptComputer[] computerInteractions;

    [Header("Locked until every computer above has been interacted with")]
    [SerializeField] private TextBoxPromptDoor doorInteraction;

    private readonly HashSet<TextBoxPromptComputer> interactedComputers = new HashSet<TextBoxPromptComputer>();

    private void Awake()
    {
        if (doorInteraction != null)
            doorInteraction.enabled = false;
    }

    private void OnEnable()
    {
        ComputerInteractionEvents.OnComputerInteracted += HandleComputerInteracted;
    }

    private void OnDisable()
    {
        ComputerInteractionEvents.OnComputerInteracted -= HandleComputerInteracted;
    }

    private void HandleComputerInteracted(TextBoxPromptComputer source)
    {
        if (computerInteractions == null || doorInteraction == null || doorInteraction.enabled) return;

        bool isTracked = false;
        foreach (TextBoxPromptComputer computer in computerInteractions)
        {
            if (computer == source)
            {
                isTracked = true;
                break;
            }
        }

        if (!isTracked) return;

        interactedComputers.Add(source);

        if (interactedComputers.Count >= computerInteractions.Length)
            doorInteraction.enabled = true;
    }
}
