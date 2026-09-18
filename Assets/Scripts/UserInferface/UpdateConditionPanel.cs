using UnityEngine;
using TMPro; // Required namespace for TextMeshPro elements
public class UpdateConditionPanel : MonoBehaviour
{
    [Header("Target UI Fields")]
    [Tooltip("The TextMeshPro element used for the main title display.")]
    [SerializeField] private TextMeshProUGUI titleTextElement;

    [Tooltip("The TextMeshPro element used for the body description or message.")]
    [SerializeField] private TextMeshProUGUI messageTextElement;

    [Header("New Content Variables")]
    [Tooltip("The new header text that will appear when clicked.")]
    [SerializeField] private string newTitle = "Updated Title";

    [Tooltip("The new description text that will appear when clicked.")]
    [TextArea(3, 5)] // Creates a larger textbox in the Unity Inspector
    [SerializeField] private string newMessage = "Your custom message text goes here.";

    /// <summary>
    /// Public method to trigger the text changes. 
    /// This will be explicitly called via the Unity UI Button event list.
    /// </summary>
    public void ChangeUiTextContent()
    {
        // Update the title field if it is assigned
        if (titleTextElement != null)
        {
            titleTextElement.text = newTitle;
        }
        else
        {
            Debug.LogWarning("Title Text Element is missing from the script reference slots.");
        }

        // Update the message field if it is assigned
        if (messageTextElement != null)
        {
            messageTextElement.text = newMessage;
        }
        else
        {
            Debug.LogWarning("Message Text Element is missing from the script reference slots.");
        }
    }
}