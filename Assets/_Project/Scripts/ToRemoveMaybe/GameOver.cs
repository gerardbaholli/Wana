using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private float gameOverDuration = 2f;
    [SerializeField] private TextMeshProUGUI gameOverTMP;

    [SerializeField] private Color32 player1Color = new Color32(0x4A, 0xC1, 0x9E, 0xFF);
    [SerializeField] private Color32 player2Color = new Color32(255, 255, 255, 255);

    private void Awake()
    {
        gameOverTMP.gameObject.SetActive(false);
    }

    private void Start()
    {
        Ball.OnAnyBallMoved += Ball_OnAnyBallMoved;
    }

    private void Ball_OnAnyBallMoved(object sender, EventArgs e)
    {
        Ball looserBall;
        bool isGameOver = RuleSystem.Instance.IsGameOver(out looserBall);

        if (isGameOver)
            StartCoroutine(ShowGameOver(looserBall));
    }

    private IEnumerator ShowGameOver(Ball looserBall)
    {
        gameOverTMP.gameObject.SetActive(true);

        looserBall.GetPart();

        if (looserBall.GetPart() != TurnSystem.Part.Player1)
        {
            gameOverTMP.text = "GREEN WINS";
            gameOverTMP.color = player1Color;
        }
        else
        {
            gameOverTMP.text = "WHITE WINS";
            gameOverTMP.color = player2Color;
        }

        yield return new WaitForSeconds(gameOverDuration);

        gameOverTMP.gameObject.SetActive(false);

        DestroyAllBalls();

        FindFirstObjectByType<SceneLoader>().LoadStartingScene();
    }

    private void DestroyAllBalls()
    {
        Ball[] ballList = FindObjectsByType<Ball>(FindObjectsSortMode.None);
        foreach (Ball ball in ballList)
        {
            Destroy(ball.gameObject);
        }
    }
}
