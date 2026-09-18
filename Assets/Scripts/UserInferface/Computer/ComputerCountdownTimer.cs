using UnityEngine;
using TMPro; // Required namespace for TextMeshPro elements

// Attach to the panel that holds the "10" countdown text and the button/panel
// that should appear once time runs out. Counts down once per second from
// startSeconds to 1, then permanently reveals revealTarget.
//
// Starts automatically whenever this GameObject becomes active (e.g. when the
// player opens this computer window). If it already finished on an earlier
// visit, re-enabling skips straight to the finished state instead of
// restarting the countdown, so revealTarget stays shown for the rest of the
// playthrough.
public class ComputerCountdownTimer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject revealTarget;

    [Header("Settings")]
    [SerializeField] private int startSeconds = 10;

    private float secondTimer;
    private int secondsLeft;
    private bool isRunning;
    private bool hasFinished;

    private void OnEnable()
    {
        if (hasFinished)
        {
            if (revealTarget != null)
                revealTarget.SetActive(true);
            return;
        }

        secondsLeft = startSeconds;
        secondTimer = 1f;
        isRunning = true;

        if (countdownText != null)
            countdownText.text = secondsLeft.ToString();

        if (revealTarget != null)
            revealTarget.SetActive(false);
    }

    private void Update()
    {
        if (!isRunning) return;

        secondTimer -= Time.deltaTime;
        if (secondTimer > 0f) return;

        secondTimer += 1f;
        secondsLeft--;

        if (secondsLeft <= 0)
        {
            isRunning = false;
            hasFinished = true;

            if (countdownText != null)
                countdownText.text = "0";

            if (revealTarget != null)
                revealTarget.SetActive(true);

            return;
        }

        if (countdownText != null)
            countdownText.text = secondsLeft.ToString();
    }
}
