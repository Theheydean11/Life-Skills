using UnityEngine;

[CreateAssetMenu(fileName = "NewFood", menuName = "Food/Food Data")]
public class FoodData : ScriptableObject
{
    public string foodName;
    public Sprite icon;
}
