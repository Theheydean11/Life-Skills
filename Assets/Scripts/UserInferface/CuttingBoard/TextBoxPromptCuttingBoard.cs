using UnityEngine;

public class TextBoxPromptCuttingBoard : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject uiObject;
    [SerializeField] private GameObject cuttingBoardPanel;

    [Header("Cameras")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera interactionCamera;

    [Header("Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool hideOnExit = true;

    [Header("Ingredient Placement")]
    [Tooltip("Dumps the fridge basket's food into Ingridient Placement (2) when this cutting board's interaction view opens.")]
    [SerializeField] private bool placeBasketFood = false;

    private bool isPlayerInRange;
    private bool isViewingInteraction;

    private void Awake()
    {
        if (uiObject != null)
            uiObject.SetActive(false);

        if (cuttingBoardPanel != null)
            cuttingBoardPanel.SetActive(false);

        SetInteractionView(false);
    }

    private void Update()
    {
        if (isPlayerInRange && !isViewingInteraction && Input.GetKeyDown(KeyCode.E))
        {
            SetInteractionView(true);

            if (cuttingBoardPanel != null)
                cuttingBoardPanel.SetActive(true);
        }
        else if (isViewingInteraction && Input.GetKeyDown(KeyCode.Escape))
        {
            SetInteractionView(false);

            if (cuttingBoardPanel != null)
                cuttingBoardPanel.SetActive(false);
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
                ? FoodInteraction.FoodContext.Cabinet
                : FoodInteraction.FoodContext.None,
                interactionCamera);

            if (viewingInteraction && placeBasketFood)
                FoodInteraction.Instance.MoveBasketFoodToCounter2();
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

        if (isViewingInteraction)
            SetInteractionView(false);

        if (cuttingBoardPanel != null)
            cuttingBoardPanel.SetActive(false);
    }
}
