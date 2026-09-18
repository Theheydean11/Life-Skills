using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectiveDropDownButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Objective UI")]
    [SerializeField] private GameObject objectivePanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ToggleObjectivePanel();
        }
    }

    private void ToggleObjectivePanel()
    {
        if (objectivePanel != null)
        {
            bool isActive = objectivePanel.activeSelf;
            objectivePanel.SetActive(!isActive);
        }
    }
}
