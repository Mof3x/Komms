using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Playing,
    Failed,
    Won
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private int totalParts = 3;

    public GameState CurrentState { get; private set; } = GameState.Playing;
    public int CollectedParts { get; private set; }
    public int TotalParts => totalParts;
    public bool HasAllParts => CollectedParts >= totalParts;

    public event Action<int, int> PartsChanged;
    public event Action<GameState> StateChanged;
    public event Action<bool> SpottedChanged;

    private readonly HashSet<string> collectedPartIds = new HashSet<string>();
    private bool isSpotted;

    private void Awake()
    {
        Debug.Log("GameManager Awake on " + gameObject.name);

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("GameManager Awake on " + gameObject.name);
    }

    private void Start()
    {
        CurrentState = GameState.Playing;
        CollectedParts = 0;
        collectedPartIds.Clear();

        Debug.Log("GameManager Start: CollectedParts = " + CollectedParts + ", TotalParts = " + TotalParts);

        PartsChanged?.Invoke(CollectedParts, TotalParts);
        StateChanged?.Invoke(CurrentState);
    }

    public bool RegisterPart(string partId)
    {
        if (CurrentState != GameState.Playing)
            return false;

        if (string.IsNullOrWhiteSpace(partId))
            return false;

        if (collectedPartIds.Contains(partId))
            return false;

        collectedPartIds.Add(partId);
        CollectedParts++;

        PartsChanged?.Invoke(CollectedParts, TotalParts);
        return true;
    }

    public void WinGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        if (!HasAllParts)
        {
            Debug.Log("WinGame blocked: not all parts collected.");
            return;
        }

        CurrentState = GameState.Won;
        Debug.Log("WinGame: state set to Won");
        StateChanged?.Invoke(CurrentState);
    }

    public void FailGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        CurrentState = GameState.Failed;
        Debug.Log("FailGame: state set to Failed");
        StateChanged?.Invoke(CurrentState);
    }

    public void SetSpotted(bool spotted)
    {
        if (isSpotted == spotted)
            return;

        isSpotted = spotted;
        SpottedChanged?.Invoke(spotted);
    }
}