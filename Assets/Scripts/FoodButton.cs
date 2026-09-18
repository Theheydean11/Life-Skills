using UnityEngine;

// Attach to each clickable food GameObject in the scene. Requires a Collider.
// Assign the FoodData asset for this object in the Inspector.
public class FoodButton : MonoBehaviour
{
    [SerializeField] private FoodData data;

    private void OnMouseDown()
    {
        if (FoodTrayManager.Instance.AddFood(data))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnMouseEnter()
    {
        HoverNamePanel.Instance.Show(data);
    }

    private void OnMouseExit()
    {
        HoverNamePanel.Instance.Hide();
    }
}
