using UnityEngine;

// Attach alongside TextBoxPromptComputer on the computer object and list every
// window/tab it can show (Canvas-Desktop, Canvas-Gmail, Win Canvas, Mom Email,
// Scam 1, Package.Com, ...) in tabWindows. Without this, each tab is only ever
// turned on by a DesktopWindowButton click and never turned off when the
// player leaves the computer, so whatever was left open collides with
// whatever is shown on the next visit.
//
// This remembers whichever tab was on screen the first time the player opened
// the computer and re-shows only that tab, with every other tab forced off,
// on every later visit for the rest of the playthrough.
public class ComputerTabManager : MonoBehaviour
{
    [Tooltip("Every window/tab GameObject the computer can show.")]
    [SerializeField] private GameObject[] tabWindows;

    private GameObject firstTab;
    private bool hasCapturedFirstTab;

    // Call when the player opens the computer (E press), before camera/raycast
    // setup runs, so the correct tab is already active for it to find.
    public void ShowComputer()
    {
        if (!hasCapturedFirstTab)
        {
            firstTab = FindActiveTab();
            hasCapturedFirstTab = true;
        }

        ShowOnly(firstTab);
    }

    // Call when the player leaves the computer (Esc press).
    public void HideComputer()
    {
        ShowOnly(null);
    }

    private GameObject FindActiveTab()
    {
        if (tabWindows == null) return null;

        foreach (GameObject tab in tabWindows)
        {
            if (tab != null && tab.activeSelf)
                return tab;
        }

        return tabWindows.Length > 0 ? tabWindows[0] : null;
    }

    private void ShowOnly(GameObject tabToShow)
    {
        if (tabWindows == null) return;

        foreach (GameObject tab in tabWindows)
        {
            if (tab != null)
                tab.SetActive(tab == tabToShow);
        }
    }
}
