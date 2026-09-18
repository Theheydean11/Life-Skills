using UnityEngine;

// Put this on each window/panel. Wire a button's onClick to Open() to navigate to
// this window, and this window's own X button to Close() to go back to the previous one.
public class WindowHandler : MonoBehaviour
{
    // Defaults to the object this script sits on, but can point elsewhere if needed.
    [SerializeField] private GameObject windowRoot;

    private void Awake()
    {
        if (windowRoot == null)
        {
            windowRoot = gameObject;
        }
    }

    // Call from a button elsewhere to navigate to this window.
    public void Open()
    { 
        Debug.Log("ok");
        WindowManager.Instance.OpenWindow(windowRoot);
    }

    // Call from this window's own X button to close it and return to the previous window.
    public void Close()
    {
        WindowManager.Instance.CloseWindow(windowRoot);
    }
}
