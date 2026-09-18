using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Attach to the left-side Panel GameObject in the Canvas.
// Panel should be anchored to the left-center of the screen.
public class HoverNamePanel : MonoBehaviour
{
    public static HoverNamePanel Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text nameText;   // TextMeshPro; swap for Text if not using TMP
    [SerializeField] private Image iconImage;     // Child Image under the panel that shows the food's icon

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        panel.SetActive(true);
    }

    public void Show(FoodData data)
    {
        if (data == null) return;

        nameText.text = data.foodName;
        iconImage.sprite = data.icon;
        iconImage.enabled = data.icon != null;
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(true);
    }
}
