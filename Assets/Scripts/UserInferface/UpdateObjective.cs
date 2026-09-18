using UnityEngine;
using TMPro;

// Attach to the ObjectivePanel. Shows whichever objective step the player
// has most recently reached, advancing automatically as other scripts
// report progress:
//   - TextBoxPromptComputer already raises ComputerInteractionEvents when
//     the player interacts with the computer; that alone advances to
//     GoToGmail, no extra wiring needed.
//   - Wire the Gmail desktop icon's OnClick to MarkGmailOpened().
//   - Wire the legitimate "Package Button" (Package.com panel) and the
//     scam "Pickup Button" (Scam Page 2) OnClick to MarkEmailDecisionMade()
//     - whichever the player picks, they're done with the email and should
//     head to the box.
//   - BoxInteraction raises ObjectiveEvents.RaiseStepReached(Done) directly
//     once the player interacts with the box.
// Steps only ever move forward - an out-of-order or repeated trigger is
// ignored so the panel can't jump backwards.
public class UpdateObjective : MonoBehaviour
{
    [Header("Target UI Field")]
    [SerializeField] private TextMeshProUGUI objectiveTextElement;

    [Header("Objective Text, in order")]
    [SerializeField] private string goToComputerText = "Go to the computer and interact with it.";
    [SerializeField] private string goToGmailText = "Go to gmail.";
    [SerializeField] private string findMomEmailText = "Now find your mom email.";
    [SerializeField] private string goToBoxText = "Go outside the door and interact with the box.";
    [SerializeField] private string doneText = "Nice work - you spotted the scam!";

    private ObjectiveStep currentStep = ObjectiveStep.GoToComputer;

    private void OnEnable()
    {
        ObjectiveEvents.OnStepReached += HandleStepReached;
        ComputerInteractionEvents.OnComputerInteracted += HandleComputerInteracted;
    }

    private void OnDisable()
    {
        ObjectiveEvents.OnStepReached -= HandleStepReached;
        ComputerInteractionEvents.OnComputerInteracted -= HandleComputerInteracted;
    }

    private void Start()
    {
        ShowStep(currentStep);
    }

    // Wire to the Gmail desktop icon's OnClick.
    public void MarkGmailOpened()
    {
        HandleStepReached(ObjectiveStep.FindMomEmail);
    }

    // Wire to the "Package Button" (legit) and scam "Pickup Button" OnClick.
    public void MarkEmailDecisionMade()
    {
        HandleStepReached(ObjectiveStep.GoToBox);
    }

    private void HandleComputerInteracted(TextBoxPromptComputer source)
    {
        HandleStepReached(ObjectiveStep.GoToGmail);
    }

    private void HandleStepReached(ObjectiveStep step)
    {
        if (step <= currentStep) return;

        currentStep = step;
        ShowStep(currentStep);
    }

    private void ShowStep(ObjectiveStep step)
    {
        if (objectiveTextElement == null) return;

        objectiveTextElement.text = step switch
        {
            ObjectiveStep.GoToComputer => goToComputerText,
            ObjectiveStep.GoToGmail => goToGmailText,
            ObjectiveStep.FindMomEmail => findMomEmailText,
            ObjectiveStep.GoToBox => goToBoxText,
            ObjectiveStep.Done => doneText,
            _ => objectiveTextElement.text
        };
    }
}
