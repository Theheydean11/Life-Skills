using UnityEngine;

public class TextBoxPromptStove : MonoBehaviour
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

    [Header("Basket")]
    [SerializeField] private Transform basket;
    [SerializeField] private Transform basketDropLocation;

    [Header("Station")]
    [Tooltip("Enable on a second station (e.g. CabinetDset (2) / Ingridient Placement (2)) so basket food is dumped into its own counter slots instead of the first station's.")]
    [SerializeField] private bool isSecondaryStation = false;

    private bool isPlayerInRange;
    private bool isViewingInteraction;
    private bool hasMovedBasket;

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

        if (viewingInteraction && !hasMovedBasket)
            MoveBasketToStove();

        if (FoodInteraction.Instance != null)
        {
            FoodInteraction.Instance.SetContext(viewingInteraction
                ? FoodInteraction.FoodContext.Stove
                : FoodInteraction.FoodContext.None,
                interactionCamera);

            if (viewingInteraction)
            {
                if (isSecondaryStation)
                    FoodInteraction.Instance.MoveBasketFoodToCounter2();
                else
                    FoodInteraction.Instance.MoveBasketFoodToCounter();
            }
        }
    }

    private void MoveBasketToStove()
    {
        if (basket == null || basketDropLocation == null) return;

        basket.SetParent(basketDropLocation, false);
        basket.position = basketDropLocation.position;
        basket.rotation = basketDropLocation.rotation;
                                
        hasMovedBasket = true;
        Debug.Log("show ingredients");
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