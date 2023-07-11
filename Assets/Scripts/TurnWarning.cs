using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnWarning : MonoBehaviour
{
    [SerializeField] private float warningTurnDuration = 2f;
    [SerializeField] private TextMeshProUGUI warningTurnTMP;

    [SerializeField] private Color32 player1Color = new Color32(0x4A, 0xC1, 0x9E, 0xFF);
    [SerializeField] private Color32 player2Color = new Color32(255, 255, 255, 255);

    private void Awake()
    {
        warningTurnTMP.gameObject.SetActive(false);
    }

    private void Start()
    {
        Ball.OnAnyBallClicked += Ball_OnAnyBallClicked;
    }

    private void Ball_OnAnyBallClicked(object sender, EventArgs e)
    {
        Ball ball = sender as Ball;
        TurnSystem.Part currentTurn = TurnSystem.Instance.GetPlayerTurn();

        if (currentTurn != ball.GetPart())
            StartCoroutine(ShowWarning());
    }

    private IEnumerator ShowWarning()
    {
        warningTurnTMP.gameObject.SetActive(true);
        if (TurnSystem.Instance.GetPlayerTurn() == TurnSystem.Part.Player1)
        {
            warningTurnTMP.text = "GREEN MOVES";
            warningTurnTMP.color = player1Color;
        }
        else
        {
            warningTurnTMP.text = "WHITE MOVES";
            warningTurnTMP.color = player2Color;
        }

        yield return new WaitForSeconds(warningTurnDuration);

        warningTurnTMP.gameObject.SetActive(false);
    }

}
