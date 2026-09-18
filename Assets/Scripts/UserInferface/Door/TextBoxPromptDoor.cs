using UnityEngine;

public class TextBoxPromptDoor : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject uiObject;
    [SerializeField] private GameObject doorPanel;

    [Header("Door")]
    [Tooltip("Optional. When set, the keypad panel (doorPanel) only shows while this door is still locked, even while viewing the door camera.")]
    [SerializeField] private opencloseDoor1 door;

    [Header("Cameras")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera interactionCamera;

    [Header("Settings")]
    [SerializeField] private string playerTag = "Player";

    private bool isPlayerInRange;
    private bool isViewingInteraction;

    // Lets KeypadScript gate its own E-to-open/close handling to only while
    // the player is actually looking through this door's camera.
    public bool IsViewingInteraction => isViewingInteraction;

    private void Awake()
    {
        if (uiObject != null)
            uiObject.SetActive(false);

        if (doorPanel != null)
            doorPanel.SetActive(false);

        SetInteractionView(false);
    }

    private void Update()
    {
        if (isPlayerInRange && !isViewingInteraction && Input.GetKeyDown(KeyCode.E))
        {
            SetInteractionView(true);
        }
        else if (isViewingInteraction && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitInteractionView();
        }
    }

    // Same exit path as pressing Escape. Called by KeypadScript once the
    // correct code is entered, so the camera returns to the main view before
    // the door itself opens.
    public void ExitInteractionView()
    {
        if (!isViewingInteraction) return;

        SetInteractionView(false);
    }

    private void SetInteractionView(bool viewingInteraction)
    {
        isViewingInteraction = viewingInteraction;

        if (mainCamera != null)
        {
            mainCamera.enabled = !viewingInteraction;

            AudioListener mainListener = mainCamera.GetComponent<AudioListener>();
            if (mainListener != null)
                mainListener.enabled = !viewingInteraction;
        }

        if (interactionCamera != null)
        {
            interactionCamera.enabled = viewingInteraction;

            AudioListener interactionListener = interactionCamera.GetComponent<AudioListener>();
            if (interactionListener != null)
                interactionListener.enabled = viewingInteraction;
        }

        // World Space canvases (the door keypad, etc.) have no Render Camera
        // assigned, so their GraphicRaycaster falls back to Camera.main. That
        // fallback breaks as soon as mainCamera is disabled above, so every
        // world-space canvas needs its worldCamera pointed at whichever camera
        // is actually active or clicks stop landing where they visually appear.
        Camera activeCamera = viewingInteraction ? interactionCamera : mainCamera;
        if (activeCamera != null)
        {
            foreach (Canvas canvas in FindObjectsByType<Canvas>(FindObjectsSortMode.None))
            {
                if (canvas.renderMode == RenderMode.WorldSpace)
                    canvas.worldCamera = activeCamera;
            }
        }

        if (uiObject != null)
            uiObject.SetActive(!viewingInteraction);

        // Keypad panel: only ever shown while actually looking through the
        // door camera, and only if the door still needs unlocking.
        if (doorPanel != null)
            doorPanel.SetActive(viewingInteraction && (door == null || door.isLocked));

        if (DoorHandleInteraction.Instance != null)
            DoorHandleInteraction.Instance.SetActive(viewingInteraction, interactionCamera);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        isPlayerInRange = true;

        if (uiObject != null && !isViewingInteraction)
            uiObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        isPlayerInRange = false;

        if (uiObject != null && !isViewingInteraction)
            uiObject.SetActive(false);
    }
}
