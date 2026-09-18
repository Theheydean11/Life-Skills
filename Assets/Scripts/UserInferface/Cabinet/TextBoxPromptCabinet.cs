using UnityEngine;

public class TextBoxPromptCabinet : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject uiObject;
    [SerializeField] private GameObject cabinetPanel;

    [Header("Cameras")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera interactionCamera;

    [Header("Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool hideOnExit = true;


    private bool isPlayerInRange;
    private bool isViewingInteraction;

    private void Awake()
    {
        if (uiObject != null)
            uiObject.SetActive(false);

        if (cabinetPanel != null)
            cabinetPanel.SetActive(false);

        SetInteractionView(false);
    }

    private void Update()
    {
        if (isPlayerInRange && !isViewingInteraction && Input.GetKeyDown(KeyCode.E))
        {
            SetInteractionView(true);

            if (cabinetPanel != null)
                cabinetPanel.SetActive(true);
        }
        else if (isViewingInteraction && Input.GetKeyDown(KeyCode.Escape))
        {
            SetInteractionView(false);

            if (cabinetPanel != null)
                cabinetPanel.SetActive(false);
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

        if (uiObject != null)
            uiObject.SetActive(isPlayerInRange && !viewingInteraction);
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

        if (uiObject != null && hideOnExit)
            uiObject.SetActive(false);

        if (isViewingInteraction)
            SetInteractionView(false);

        if (cabinetPanel != null)
            cabinetPanel.SetActive(false);
    }
}
