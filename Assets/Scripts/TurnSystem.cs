using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;

    private int turnNumber = 1;
    private Part playerTurn = Part.Player1;

    public enum Part { Player1, Player2 }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one TurnSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void NextTurn()
    {
        turnNumber++;

        if (playerTurn == Part.Player1)
        {
            playerTurn = Part.Player2;
        }
        else
        {
            playerTurn = Part.Player1;
        }

        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public Part GetPlayerTurn()
    {
        return playerTurn;
    }

    public bool IsPlayerTurn(Part part) {
        return playerTurn == part;
    }

}
