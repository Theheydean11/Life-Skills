using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FridgeMove : MonoBehaviour
{
    [Header("Door")]
    [SerializeField] private Transform doorTransform;
    [SerializeField] private float openAngle = -90f; // degrees around the hinge's Y axis
    [SerializeField] private float rotateSpeed = 120f; // degrees per second

    private Quaternion closedLocalRotation;
    private Quaternion openLocalRotation;
    private bool isOpen;

    private void Awake()
    {
        if (doorTransform == null)
            doorTransform = transform;

        closedLocalRotation = doorTransform.localRotation;
        openLocalRotation = closedLocalRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    private void Update()
    {
        Quaternion target = isOpen ? openLocalRotation : closedLocalRotation;
        doorTransform.localRotation = Quaternion.RotateTowards(doorTransform.localRotation, target, rotateSpeed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        isOpen = !isOpen; // clicking again while open swings it back closed
    }
}
