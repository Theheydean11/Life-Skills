using UnityEngine;

public class TextBoxPromptFridge : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject uiObject;
    [SerializeField] private GameObject foodNamePanel;

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

        foodNamePanel.SetActive(false);
        SetInteractionView(false);
    }

    private void Update()
    {
        if (isPlayerInRange && !isViewingInteraction && Input.GetKeyDown(KeyCode.E))
        {
            SetInteractionView(true);
            foodNamePanel.SetActive(true);
        }
        else if (isViewingInteraction && Input.GetKeyDown(KeyCode.Escape))
        {
            SetInteractionView(false);
            foodNamePanel.SetActive(false);
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

        if (FoodInteraction.Instance != null)
        {
            FoodInteraction.Instance.SetContext(viewingInteraction
                ? FoodInteraction.FoodContext.Fridge
                : FoodInteraction.FoodContext.None,
                interactionCamera);
        }
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
    }
}