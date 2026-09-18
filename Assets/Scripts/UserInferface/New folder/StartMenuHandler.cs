using UnityEngine;

public class StartMenuHandler : MonoBehaviour
{
    [Header("Menu References")]
    [SerializeField] private GameObject startMenuCanvas;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject ExitMenuCanvas;

    [Header("Gameplay UI")]
    [Tooltip("The main gameplay UI Canvas (Screen Space - Overlay, no camera assigned). Shown at every gameplay camera (main/stove/fridge/etc.) except while the start menu is up.")]
    [SerializeField] private GameObject uiCanvas;

    [Header("Player References")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Camera")]
    [Tooltip("The player's main view camera. Esc only opens the menu while this is the active camera (not while viewing a stove/fridge/etc. interaction).")]
    [SerializeField] private Camera mainCamera;

    [Header("Exit Confirmation")]
    [Tooltip("Seconds allowed between the two Esc presses required to open the start menu.")]
    [SerializeField] private float doublePressWindow = 1f;

    private float lastEscapeTime = -Mathf.Infinity;

    void Start()
    {
        // Player shouldn't be able to move while the start menu is up
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (ExitMenuCanvas != null)
            ExitMenuCanvas.SetActive(false);

        if (uiCanvas != null)
            uiCanvas.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void Update()
    {
        // Only react to Esc while actually playing (start menu hidden) and
        // looking through the main camera (not a stove/fridge/etc. interaction
        // view). Requires two presses within doublePressWindow so a single Esc
        // that closes an interaction view (which hands control back to the
        // main camera that same frame) can't also open the start menu.
        if (Input.GetKeyDown(KeyCode.Escape)
            && startMenuCanvas != null && !startMenuCanvas.activeSelf
            && mainCamera != null && mainCamera.enabled)
        {
            if (Time.time - lastEscapeTime <= doublePressWindow)
            {
                lastEscapeTime = -Mathf.Infinity;
                OnExitButtonPressed();
            }
            else
            {
                lastEscapeTime = Time.time;
            }
        }
    }

    // Hooked up to the "Start" button's OnClick event
    public void OnStartButtonPressed()
    {
        if (startMenuCanvas != null)
            startMenuCanvas.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (ExitMenuCanvas != null)
            ExitMenuCanvas.SetActive(true);

        if (uiCanvas != null)
            uiCanvas.SetActive(true);

        // Gameplay is mouse-driven (click-to-teleport, food/computer UI), so
        // the cursor must stay visible and unlocked instead of being captured.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Hooked up to the "Settings" button's OnClick event
    public void OnSettingsButtonPressed()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    // Hooked up to a "Back"/"Close" button on the settings panel, if present
    public void OnSettingsClosePressed()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }
    // Hooked up to the exit panel's button, and to pressing Esc while playing
    public void OnExitButtonPressed()
    {
        if (startMenuCanvas != null)
            startMenuCanvas.SetActive(true);

        if (ExitMenuCanvas != null)
            ExitMenuCanvas.SetActive(false);

        if (uiCanvas != null)
            uiCanvas.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
