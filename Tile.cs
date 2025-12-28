using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    public Vector2Int coord;
    public bool walkable = true;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ResetColor();
    }

    public void SetWalkable(bool state)
    {
        walkable = state;
        sr.color = state ? Color.white : Color.black;
    }

    public void SetAsStart() => sr.color = Color.green;
    public void SetAsGoal() => sr.color = Color.red;
    public void SetAsPath() => sr.color = Color.yellow;
    public void ResetColor()
    {
        if (walkable) sr.color = Color.white;
        else sr.color = Color.black;
    }
}