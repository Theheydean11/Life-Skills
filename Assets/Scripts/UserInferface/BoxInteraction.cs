using UnityEngine;
using UnityEngine.Serialization;

// Attach to the box the player interacts with near the door. Which outcome
// panel it reveals depends on what the player clicked earlier on the
// computer: wire the scam "Pickup Button" OnClick to call MarkLoseOutcome()
// here (alongside its existing UpdateConditionPanel.ChangeUiTextContent()
// call), and wire the legitimate "Package Button" OnClick (Package.com
// panel) to call MarkWinOutcome() (alongside its own ChangeUiTextContent()
// call). Whichever was marked most recently is what shows when the player
// presses E on the box. Defaults to the lose panel if neither was called.
public class BoxInteraction : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The Transform of the player (e.g., Main Camera or Player Object).")]
    [SerializeField] private Transform playerTransform;

    [Tooltip("The UI element hosted on/above this object.")]
    [SerializeField] private GameObject worldUiElement;

    [Tooltip("Shown on pressing E if the player fell for the scam (Pickup Button).")]
    [FormerlySerializedAs("targetCanvasObject")]
    [SerializeField] private GameObject loseConditionPanel;

    [Tooltip("Shown on pressing E if the player avoided the scam (Package Button).")]
    [SerializeField] private GameObject winConditionPanel;

    [Header("Settings")]
    [Tooltip("Distance threshold to trigger UI visibility.")]
    [SerializeField] private float interactionDistance = 2f;

    [Tooltip("The key used to trigger the canvas activation.")]
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    private bool isPlayerClose = false;
    private bool hasWon = false;

    private void Awake()
    {
        // Automatically fetch main camera as player if left empty
        if (playerTransform == null && Camera.main != null)
        {
            playerTransform = Camera.main.transform;
        }

        // Ensure elements start in correct states
        if (worldUiElement != null) worldUiElement.SetActive(false);
        if (loseConditionPanel != null) loseConditionPanel.SetActive(false);
        if (winConditionPanel != null) winConditionPanel.SetActive(false);
    }

    // Call from the scam "Pickup Button" OnClick.
    public void MarkLoseOutcome()
    {
        hasWon = false;
    }

    // Call from the legitimate "Package Button" OnClick (Package.com panel).
    public void MarkWinOutcome()
    {
        hasWon = true;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // Calculate absolute distance between this object and the player
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // Check if player is within the 2f unit range
        if (distance <= interactionDistance)
        {
            if (!isPlayerClose)
            {
                isPlayerClose = true;
                ToggleWorldUI(true);
            }

            // Listen for the interaction key while near the object
            if (Input.GetKeyDown(interactionKey))
            {
                ActivateTargetCanvas();
            }
        }
        else
        {
            if (isPlayerClose)
            {
                isPlayerClose = false;
                ToggleWorldUI(false);
            }
        }
    }

    private void ToggleWorldUI(bool isVisible)
    {
        if (worldUiElement != null)
        {
            worldUiElement.SetActive(isVisible);
        }
    }

    private void ActivateTargetCanvas()
    {
        GameObject panelToShow = hasWon ? winConditionPanel : loseConditionPanel;

        if (panelToShow != null)
        {
            // Activate the matching outcome panel
            panelToShow.SetActive(true);

            // Optional: Hide the hovering prompt UI once opened
            ToggleWorldUI(false);
        }

        ObjectiveEvents.RaiseStepReached(ObjectiveStep.Done);
    }
}
