using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    //public event EventHandler OnTurnChanged;

    public enum Part
    {
        Player1,
        Player2
    }

    private int turnNumber = 1;
    private Part playerTurn = Part.Player1;

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

        Debug.Log("NextTurn()" + playerTurn);

        //OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public Part GetPlayerTurn()
    {
        return playerTurn;
    }

}
