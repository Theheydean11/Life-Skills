using UnityEngine;

// Attach to a stove knob GameObject. Requires a Collider so OnMouseDown fires.
// Clicking the knob toggles isOn and activates/deactivates the linked particle
// effect (e.g. flame or steam) to match.
[RequireComponent(typeof(Collider))]
public class StoveKnob : MonoBehaviour
{
    [Header("State")]
    [Tooltip("True while the knob is turned on. Drives the particle effect below.")]
    [SerializeField] private bool isOn;

    [Tooltip("True while the knob is locked off, e.g. because its pan's food already finished cooking. Locked knobs ignore clicks.")]
    [SerializeField] private bool isLocked;

    [Header("Effect")]
    [Tooltip("Particle system to play while the knob is on (e.g. flame/steam).")]
    [SerializeField] private ParticleSystem particleEffect;

    public bool IsOn => isOn;
    public ParticleSystem ParticleEffect => particleEffect;

    private void Awake()
    {
        ApplyState();
    }

    private void OnMouseDown()
    {
        if (isLocked) return;

        isOn = !isOn;
        ApplyState();
    }

    // Called by PanCooking once its pan's food finishes cooking.
    public void TurnOff()
    {
        isOn = false;
        ApplyState();
    }

    // Prevents the knob being turned back on, e.g. while a finished dish is
    // still sitting in its pan. Call Unlock once the dish is plated.
    public void Lock() => isLocked = true;
    public void Unlock() => isLocked = false;

    private void ApplyState()
    {
        if (particleEffect == null)
            return;

        if (isOn)
            particleEffect.Play();
        else
            particleEffect.Stop();
    }
}
