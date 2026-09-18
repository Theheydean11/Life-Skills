using UnityEngine;

// Place on any GameObject in the Kitchen and Computer scenes. Keeps the
// cursor visible and unlocked for the click-driven gameplay (teleporting,
// food/computer UI), since Unity's cursor lock state otherwise carries over
// between scenes.
public class CursorManager : MonoBehaviour
{
    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
