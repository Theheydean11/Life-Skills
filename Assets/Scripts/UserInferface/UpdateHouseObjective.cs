using UnityEngine;
using TMPro;

// Attach to the House scene's ObjectivePanel ("ObjectivePanel (house)").
// Shows whichever objective step the player has most recently reached,
// advancing automatically whenever something calls
// HouseObjectiveEvents.RaiseStepReached(...).
//
// Placeholder: the house objective steps/text below are not decided yet.
// Once the house tutorial flow is defined:
//   - Rename the HouseObjectiveStep enum values in HouseObjectiveEvents.cs
//     to match the real steps, in order.
//   - Fill in the matching text fields below.
//   - Wire whatever interactions mark progress (button OnClick, interaction
//     events, etc.) to call HouseObjectiveEvents.RaiseStepReached(...).
// Steps only ever move forward - an out-of-order or repeated trigger is
// ignored so the panel can't jump backwards.
public class UpdateHouseObjective : MonoBehaviour
{
    [Header("Target UI Field")]
    [SerializeField] private TextMeshProUGUI objectiveTextElement;

    [Header("Objective Text, in order (fill in once house objectives are decided)")]
    [SerializeField] private string step1Text = "";
    [SerializeField] private string step2Text = "";
    [SerializeField] private string step3Text = "";
    [SerializeField] private string step4Text = "";
    [SerializeField] private string doneText = "";

    private HouseObjectiveStep currentStep = HouseObjectiveStep.Step1;

    private void OnEnable()
    {
        HouseObjectiveEvents.OnStepReached += HandleStepReached;
    }

    private void OnDisable()
    {
        HouseObjectiveEvents.OnStepReached -= HandleStepReached;
    }

    private void Start()
    {
        ShowStep(currentStep);
    }

    private void HandleStepReached(HouseObjectiveStep step)
    {
        if (step <= currentStep) return;

        currentStep = step;
        ShowStep(currentStep);
    }

    private void ShowStep(HouseObjectiveStep step)
    {
        if (objectiveTextElement == null) return;

        objectiveTextElement.text = step switch
        {
            HouseObjectiveStep.Step1 => step1Text,
            HouseObjectiveStep.Step2 => step2Text,
            HouseObjectiveStep.Step3 => step3Text,
            HouseObjectiveStep.Step4 => step4Text,
            HouseObjectiveStep.Done => doneText,
            _ => objectiveTextElement.text
        };
    }
}
