using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 20;
    public int height = 12;
    public GameObject tilePrefab;
    [HideInInspector] public Tile[,] tiles;

    [Header("Start / Goal (initial)")]
    public Vector2Int start = new Vector2Int(0, 0);
    public Vector2Int goal = new Vector2Int(19, 11);

    void Start()
    {
        GenerateGrid();
        CenterCamera();
        SetStart(start);
        SetGoal(goal);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int cell = GetMouseGridPos();
            if (cell.x < 0) return;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                SetStart(cell);
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                SetGoal(cell);
            else
                ToggleObstacle(cell);
        }
    }

    void GenerateGrid()
    {
        tiles = new Tile[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                GameObject go = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity, this.transform);
                go.name = $"Tile {x},{y}";
                Tile t = go.GetComponent<Tile>();
                t.coord = new Vector2Int(x, y);
                t.SetWalkable(true);
                tiles[x, y] = t;
            }
    }

    void CenterCamera()
    {
        if (Camera.main == null) return;
        Camera.main.transform.position = new Vector3(width / 2f - 0.5f, height / 2f - 0.5f, -10f);
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = Mathf.Max(width, height) / 2f + 1f;
    }

    public Vector2Int GetMouseGridPos()
    {
        Vector3 w = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int x = Mathf.RoundToInt(w.x);
        int y = Mathf.RoundToInt(w.y);
        if (x < 0 || x >= width || y < 0 || y >= height) return new Vector2Int(-1, -1);
        return new Vector2Int(x, y);
    }

    public bool InBounds(Vector2Int p) => p.x >= 0 && p.x < width && p.y >= 0 && p.y < height;
    public bool IsWalkable(Vector2Int p) => InBounds(p) && tiles[p.x, p.y].walkable;

    public void ToggleObstacle(Vector2Int p)
    {
        if (!InBounds(p)) return;
        if (p == start || p == goal) return;
        tiles[p.x, p.y].SetWalkable(!tiles[p.x, p.y].walkable);
    }

    public void SetStart(Vector2Int p)
    {
        if (!InBounds(p)) return;
        if (InBounds(start)) tiles[start.x, start.y].ResetColor();
        start = p;
        tiles[p.x, p.y].SetWalkable(true);
        tiles[p.x, p.y].SetAsStart();
    }

    public void SetGoal(Vector2Int p)
    {
        if (!InBounds(p)) return;
        if (InBounds(goal)) tiles[goal.x, goal.y].ResetColor();
        goal = p;
        tiles[p.x, p.y].SetWalkable(true);
        tiles[p.x, p.y].SetAsGoal();
    }

    public Vector3 GridToWorld(Vector2Int p) => new Vector3(p.x, p.y, 0);
    public Tile GetTile(Vector2Int p) => InBounds(p) ? tiles[p.x, p.y] : null;

    public void ResetVisuals()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                tiles[x, y].ResetColor();

        if (InBounds(start)) tiles[start.x, start.y].SetAsStart();
        if (InBounds(goal)) tiles[goal.x, goal.y].SetAsGoal();
    }

    public void ClearObstacles()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (!tiles[x, y].walkable) tiles[x, y].SetWalkable(true);

        ResetVisuals();
    }
}