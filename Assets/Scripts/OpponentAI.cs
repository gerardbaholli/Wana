using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpponentAI : MonoBehaviour
{

    private enum State
    {
        WaitingForOpponentsTurn,
        TakingTurn
    }

    private State state;

    [SerializeField] private float timer = 2f;

    private void Awake()
    {
        state = State.WaitingForOpponentsTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {

        if (!IsOpponentTurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForOpponentsTurn:
                Debug.Log("Opponent AI --> WaitingForOpponentsTurn");
                break;
            case State.TakingTurn:
                Debug.Log("Opponent AI --> TakingTurn");
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    state = State.WaitingForOpponentsTurn;
                    TurnSystem.Instance.NextTurn();
                }
                break;
        }

    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!IsOpponentTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
            return;
        }

    }

    private bool IsOpponentTurn()
    {
        return TurnSystem.Instance.IsPlayerTurn(TurnSystem.Part.Player2);
    }

}
