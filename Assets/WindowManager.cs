using System.Collections.Generic;
using UnityEngine;

// Tracks a back/forward stack of open windows so closing one returns to the previous.
public class WindowManager : MonoBehaviour
{
    public static WindowManager Instance { get; private set; }

    private readonly Stack<GameObject> windowStack = new Stack<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Pushes a window onto the stack, hiding whichever window was on top (if any).
    public void OpenWindow(GameObject window)
    {
        Debug.Log("Hi");
        if (windowStack.Count > 0)
        {
            windowStack.Peek().SetActive(false);
        }

        window.SetActive(true);
        windowStack.Push(window);
    }

    // Closes the given window and reveals the previous one on the stack.
    public void CloseWindow(GameObject window)
    {
        if (windowStack.Count == 0 || windowStack.Peek() != window)
        {
            return;
        }

        windowStack.Pop().SetActive(false);

        if (windowStack.Count > 0)
        {
            windowStack.Peek().SetActive(true);
        }
    }

    // Closes every window back down to the first one opened.
    public void CloseAll()
    {
        while (windowStack.Count > 1)
        {
            windowStack.Pop().SetActive(false);
        }

        if (windowStack.Count > 0)
        {
            windowStack.Peek().SetActive(true);
        }
    }
}
