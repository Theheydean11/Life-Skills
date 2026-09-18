using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Attach to the keypad panel GameObject (the same one opencloseDoor1 shows
// and hides by distance). Wire each digit button's OnClick() to
// ButtonPressed(int), passing that button's own digit, and the Enter
// button's OnClick() to Enter().
public class KeypadScript : MonoBehaviour
{
    [Header("Door")]
    [SerializeField] private opencloseDoor1 door;

    [Header("Access code (four digits, e.g. 1234)")]
    public int accessCode = 1234;

    [Header("Display")]
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private Image displayImage;
    [SerializeField] private Color idleColor = Color.white;
    [SerializeField] private Color correctColor = new Color(0.2f, 0.85f, 0.4f);
    [SerializeField] private Color wrongColor = new Color(0.9f, 0.25f, 0.25f);
    [SerializeField] private float resultHoldSeconds = 1f;

    private const int CodeLength = 4;
    private readonly List<int> enteredDigits = new List<int>();
    private bool isShowingResult; // true while the red/green flash holds, ignores further input

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (displayImage != null)
            displayImage.color = idleColor;

        Display();
    }

    // Every time a digit button is pressed, its own text value is passed in here.
    public void ButtonPressed(int number)
    {
        if (isShowingResult) return;

        // Can't display more than four digits.
        if (enteredDigits.Count >= CodeLength) return;

        enteredDigits.Add(number);
        Display();
    }

    // Shows every digit that's been pressed so far, up to four.
    private void Display()
    {
        if (displayText == null) return;

        string text = "";
        foreach (int digit in enteredDigits)
            text += digit.ToString();

        displayText.text = text;
    }

    // Wire the Enter button's OnClick() to this. Checks the entered digits
    // against accessCode: fewer than four digits, or a mismatch, flashes the
    // display red and clears; a match flashes it green for a second, then
    // hides the keypad panel and unlocks the door.
    public void Enter()
    {
        if (isShowingResult) return;

        if (enteredDigits.Count < CodeLength || DigitsToCode() != accessCode)
        {
            StartCoroutine(ShowResult(wrongColor, wasCorrect: false));
            return;
        }

        StartCoroutine(ShowResult(correctColor, wasCorrect: true));
    }

    private int DigitsToCode()
    {
        int code = 0;
        foreach (int digit in enteredDigits)
            code = code * 10 + digit;
        return code;
    }

    private IEnumerator ShowResult(Color flashColor, bool wasCorrect)
    {
        isShowingResult = true;

        if (displayImage != null)
            displayImage.color = flashColor;

        yield return new WaitForSeconds(resultHoldSeconds);

        enteredDigits.Clear();
        Display();

        if (wasCorrect)
        {
            if (door != null)
            {
                door.Unlock();

                if (door.keypadPanel != null)
                    door.keypadPanel.SetActive(false);
            }
        }
        else if (displayImage != null)
        {
            displayImage.color = idleColor;
        }

        isShowingResult = false;
    }
}
