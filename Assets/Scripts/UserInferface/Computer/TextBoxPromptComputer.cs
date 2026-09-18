using UnityEngine;

public class TextBoxPromptComputer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject uiObject;
    [SerializeField] private GameObject computerPanel;
    [SerializeField] private ComputerTabManager tabManager;

    [Header("Cameras")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera interactionCamera;

    [Header("Settings")]
    [SerializeField] private string playerTag = "Player";

    private bool isPlayerInRange;
    private bool isViewingInteraction;

    // Camera that computer windows opened later (Gmail, Game Over, ...) should
    // assign to their own worldCamera, since they're inactive when the loop
    // below runs and so never get one otherwise. See DesktopWindowButton.
    public static Camera ActiveCamera { get; private set; }

    private void Awake()
    {
        if (uiObject != null)
            uiObject.SetActive(false);

        if (computerPanel != null)
            computerPanel.SetActive(false);

        SetInteractionView(false);
    }

    private void Update()
    {
        if (isPlayerInRange && !isViewingInteraction && Input.GetKeyDown(KeyCode.E))
        {
            if (computerPanel != null)
                computerPanel.SetActive(true);

            if (tabManager != null)
                tabManager.ShowComputer();

            SetInteractionView(true);

            ComputerInteractionEvents.RaiseComputerInteracted(this);
        }
        else if (isViewingInteraction && Input.GetKeyDown(KeyCode.Escape))
        {
            SetInteractionView(false);

            if (computerPanel != null)
                computerPanel.SetActive(false);

            if (tabManager != null)
                tabManager.HideComputer();
        }
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

        // World Space canvases (Desktop, Gmail, Game Over, ...) have no Render
        // Camera assigned, so their GraphicRaycaster falls back to Camera.main.
        // That fallback breaks as soon as mainCamera is disabled above, so every
        // world-space canvas needs its worldCamera pointed at whichever camera
        // is actually active or clicks stop landing where they visually appear.
        Camera activeCamera = viewingInteraction ? interactionCamera : mainCamera;
        ActiveCamera = activeCamera;
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
    }
}
