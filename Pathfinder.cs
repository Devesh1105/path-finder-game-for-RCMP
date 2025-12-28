using UnityEngine;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject agentPrefab;

    private GameObject currentAgent;

    public enum Algorithm { BFS, Dijkstra, AStar, Grassfire }

    public List<Vector2Int> FindPath(Algorithm algo)
    {
        switch (algo)
        {
            case Algorithm.BFS: return BFS();
            case Algorithm.Dijkstra: return Dijkstra();
            case Algorithm.AStar: return AStar();
            case Algorithm.Grassfire: return Grassfire();
            default: return BFS();
        }
    }

    public void SpawnAgent(List<Vector2Int> path)
    {
        if (currentAgent != null) Destroy(currentAgent);
        currentAgent = Instantiate(agentPrefab, gridManager.GridToWorld(gridManager.start), Quaternion.identity);

        StartCoroutine(MoveAgent(path));
    }

    private System.Collections.IEnumerator MoveAgent(List<Vector2Int> path)
    {
        foreach (var step in path)
        {
            Vector3 target = gridManager.GridToWorld(step);
            while ((currentAgent.transform.position - target).sqrMagnitude > 0.01f)
            {
                currentAgent.transform.position = Vector3.MoveTowards(currentAgent.transform.position, target, 5f * Time.deltaTime);
                yield return null;
            }
        }
    }

    private List<Vector2Int> BFS() => Search((from, to) => 1);
    private List<Vector2Int> Dijkstra() => Search((from, to) => 1);
    private List<Vector2Int> AStar() => Search((from, to) => 1, true);
    private List<Vector2Int> Grassfire() => BFS();

    private List<Vector2Int> Search(System.Func<Vector2Int, Vector2Int, int> costFunc, bool useHeuristic = false)
    {
        var start = gridManager.start;
        var goal = gridManager.goal;

        var frontier = new PriorityQueue<Vector2Int>();
        frontier.Enqueue(start, 0);

        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var costSoFar = new Dictionary<Vector2Int, int>();
        costSoFar[start] = 0;

        Vector2Int[] dirs = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            if (current == goal) break;

            foreach (var d in dirs)
            {
                var next = current + d;
                if (!gridManager.IsWalkable(next)) continue;

                int newCost = costSoFar[current] + costFunc(current, next);
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    int priority = newCost;
                    if (useHeuristic) priority += Mathf.Abs(goal.x - next.x) + Mathf.Abs(goal.y - next.y);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }

        return ReconstructPath(cameFrom, start, goal);
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int start, Vector2Int goal)
    {
        var path = new List<Vector2Int>();
        var current = goal;
        while (current != start)
        {
            if (!cameFrom.ContainsKey(current)) return path;
            path.Add(current);
            current = cameFrom[current];
        }
        path.Reverse();
        return path;
    }
}

public class PriorityQueue<T>
{
    private List<(T item, int priority)> elements = new List<(T, int)>();

    public int Count => elements.Count;

    public void Enqueue(T item, int priority)
    {
        elements.Add((item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;
        for (int i = 0; i < elements.Count; i++)
            if (elements[i].priority < elements[bestIndex].priority)
                bestIndex = i;

        var bestItem = elements[bestIndex].item;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
}