using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public Pathfinder pathfinder;

    public Button runRoundButton;
    public Button clearButton;
    public Button resetMatchButton;
    public Button[] guessButtons;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI timeText;

    private int score = 0;
    private int round = 0;
    private Pathfinder.Algorithm secretAlgo;
    private float timer;
    private bool roundRunning = false;

    void Update()
    {
        if (roundRunning)
        {
            timer += Time.deltaTime;
            timeText.text = $"Time: {timer:F1}s";
        }
    }

    public void RunRound()
    {
        round++;
        roundText.text = $"Round {round}";
        resultText.text = "";

        timer = 0;
        roundRunning = true;

        secretAlgo = (Pathfinder.Algorithm)Random.Range(0, 4);
        List<Vector2Int> path = pathfinder.FindPath(secretAlgo);
        pathfinder.SpawnAgent(path);
    }

    public void GuessBFS() => CheckGuess(Pathfinder.Algorithm.BFS);
    public void GuessDijkstra() => CheckGuess(Pathfinder.Algorithm.Dijkstra);
    public void GuessAStar() => CheckGuess(Pathfinder.Algorithm.AStar);
    public void GuessGrassfire() => CheckGuess(Pathfinder.Algorithm.Grassfire);

    private void CheckGuess(Pathfinder.Algorithm guess)
    {
        roundRunning = false;
        if (guess == secretAlgo)
        {
            score++;
            resultText.text = $"Correct! It was {secretAlgo}";
        }
        else
        {
            resultText.text = $"Wrong! It was {secretAlgo}";
        }
        scoreText.text = $"Score: {score}";
    }

    public void ClearBoard()
    {
        gridManager.ClearObstacles();
    }

    public void ResetMatch()
    {
        score = 0;
        round = 0;
        scoreText.text = "Score: 0";
        roundText.text = "Round 0";
        resultText.text = "";
        timeText.text = "Time: 0.0s";
    }
}