using System;
using UnityEngine;
using Wana;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public event EventHandler<Turn> OnTurnChanged;

    private int turnNumber = 1;
    private Turn currentTurn = Turn.Player1;

    public enum Turn { Player1, Player2 }

    private Game game;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one TurnManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        game = FindFirstObjectByType<Game>();
        game.OnMoveCompleted += Game_OnMoveCompleted;
        if (currentTurn == Turn.Player2)
        {
            Invoke(nameof(AIPlayTurn), 1.0f);
        }
    }

    private void OnDisable()
    {
        game.OnMoveCompleted -= Game_OnMoveCompleted;
    }

    private void Game_OnMoveCompleted(object sender, EventArgs e)
    {
        NextTurn();
    }

    public void NextTurn()
    {
        turnNumber++;
        currentTurn = (currentTurn == Turn.Player1) ? Turn.Player2 : Turn.Player1;
        OnTurnChanged?.Invoke(this, currentTurn);

        if (currentTurn == Turn.Player2)
        {
            Invoke(nameof(AIPlayTurn), 1.0f); 
        }

        Debug.Log("Current turn: " + currentTurn);
    }

    public Turn GetCurrentTurn()
    {
        return currentTurn;
    }

    public bool IsPlayerTurn()
    {
        return currentTurn == Turn.Player1;
    }

    private void AIPlayTurn()
    {
        Debug.Log("AI sta giocando il suo turno...");
        // Simula l'azione dell'IA qui
        Invoke(nameof(NextTurn), 1.5f); // Dopo che l'IA ha giocato, passa al turno successivo
    }
}
