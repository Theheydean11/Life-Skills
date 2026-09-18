using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Attach to each slot GameObject in the bottom tray. Needs a child Image for the icon.
public class FoodTraySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;

    private FoodData currentData;

    public bool IsOccupied { get; private set; }

    private void Awake()
    {
        // Clear();
    }

    public void SetFood(FoodData data)
    {
        currentData = data;
        iconImage.sprite = data.icon;
        iconImage.enabled = true;
        IsOccupied = true;
    }

    public void Clear()
    {
        currentData = null;
        iconImage.sprite = null;
        iconImage.enabled = false;
        IsOccupied = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsOccupied) HoverNamePanel.Instance.Show(currentData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverNamePanel.Instance.Hide();
    }
}
