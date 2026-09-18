using UnityEngine;

// Attach to a computer desktop button (e.g. the Gmail icon). Wire the
// Button's OnClick() event to OpenWindow() in the Inspector. Hides whichever
// window(s) are currently showing before showing the new one, so only one
// computer window is ever visible/clickable at a time.
public class DesktopWindowButton : MonoBehaviour
{
    [Header("Hidden when this button is clicked")]
    [SerializeField] private GameObject[] windowsToClose;

    [Header("Shown when this button is clicked")]
    [SerializeField] private GameObject windowToOpen;

    public void OpenWindow()
    {
        if (windowsToClose != null)
        {
            foreach (GameObject window in windowsToClose)
            {
                if (window != null)
                    window.SetActive(false);
            }
        }

        if (windowToOpen != null)
        {
            AssignWorldCamera(windowToOpen);
            windowToOpen.SetActive(true);
        }
    }

    // windowToOpen is inactive up to this point, so TextBoxPromptComputer's
    // canvas/camera assignment loop (which only sees active canvases) never
    // reaches it. Assign the interaction camera here instead, right before
    // activating it, so its GraphicRaycaster has a camera to click against.
    private static void AssignWorldCamera(GameObject window)
    {
        Camera activeCamera = TextBoxPromptComputer.ActiveCamera;
        if (activeCamera == null) return;

        foreach (Canvas canvas in window.GetComponentsInChildren<Canvas>(true))
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
                canvas.worldCamera = activeCamera;
        }
    }
}
