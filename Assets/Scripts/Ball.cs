﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
//using UnityEngine.iOS;

public class Ball : MonoBehaviour
{

    [SerializeField] private TurnSystem.Part part;
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private Sprite highlightBallSprite;

    private Sprite ballSprite;
    private SpriteRenderer spriteRenderer;

    private Vector3 mOffset;
    private float mZCoord;

    public static event EventHandler OnAnyBallMoved;
    public static event EventHandler OnAnyBallSelected;
    public static event EventHandler OnAnyBallClicked;

    public static void ResetStaticData()
    {
        OnAnyBallMoved = null;
        OnAnyBallSelected = null;
        OnAnyBallClicked = null;
    }

    private GridPosition startPosition;


    private void Start()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        ballSprite = spriteRenderer.sprite;

        startPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
        LevelGrid.Instance.AddBallAtGridPosition(this, startPosition);
    }

    private void OnMouseDown()
    {
        OnAnyBallClicked?.Invoke(this, EventArgs.Empty);

        if (!IsPlayerTurn())
            return;

        OnAnyBallSelected?.Invoke(this, EventArgs.Empty);
        SelectBall();
    }

    public void MakeMove(GridPosition endPosition)
    {
        bool isValidMovement = RuleSystem.Instance.IsValidMovement(startPosition, endPosition);
        bool hasAnyBall = LevelGrid.Instance.HasBallAtGridPosition(endPosition);
        bool isSamePosition = (startPosition == endPosition);

        if (isValidMovement && !isSamePosition && !hasAnyBall)
        {
            MoveOnGridPosition(endPosition, moveDuration);
            LevelGrid.Instance.AddBallAtGridPosition(this, endPosition);
            LevelGrid.Instance.RemoveBallAtGridPosition(startPosition);
            DeselectBall();
            startPosition = endPosition;
            OnAnyBallMoved?.Invoke(this, EventArgs.Empty);
            NextTurn();
        }
        else
        {
            DeselectBall();
        }
    }

    private void MoveOnGridPosition(GridPosition gridPositionToMove, float moveDuration)
    {
        Vector2 worldPositionToMove = LevelGrid.Instance.GetWorldPosition(gridPositionToMove);

        spriteRenderer.sortingOrder = 20;

        Vector3 halfwayPosition = ((Vector2)this.transform.position + worldPositionToMove) / 2f;
        Vector3 targetScale = transform.localScale * 1.5f;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(halfwayPosition, moveDuration));
        sequence.Join(transform.DOScale(targetScale, 0.3f).SetEase(Ease.OutQuad));
        sequence.Append(transform.DOMove(worldPositionToMove, moveDuration));
        sequence.Join(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InQuad));

        StartCoroutine(WaitForTweenCompletion(sequence));
    }

    private IEnumerator WaitForTweenCompletion(Tween tween)
    {
        yield return tween.WaitForCompletion();
        spriteRenderer.sortingOrder = 10;
    }

    // To remove
    private Vector2 GetNearestPosition(Vector2 worldPosition)
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
        return LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    public void NextTurn()
    {
        TurnSystem.Instance.NextTurn();
    }

    private bool IsPlayerTurn()
    {
        return TurnSystem.Instance.GetPlayerTurn() == part;
    }

    public void SelectBall()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && highlightBallSprite != null)
        {
            spriteRenderer.sprite = highlightBallSprite;
            RuleSystem.Instance.HighlightValidMovePosition(startPosition);
        }

    }

    public void DeselectBall()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && ballSprite != null)
        {
            spriteRenderer.sprite = ballSprite;
            LevelGrid.Instance.RemoveValidMovePosition();
        }
    }

    public TurnSystem.Part GetPart()
    {
        return part;
    }

}
