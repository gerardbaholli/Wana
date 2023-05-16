using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Ball : MonoBehaviour
{

    [SerializeField] TurnSystem.Part part;
    [SerializeField] float moveDuration = 0.5f;


    private Vector3 mOffset;
    private float mZCoord;

    public static event EventHandler OnAnyBallMoved;
    private GridPosition startPosition;

    private void Start()
    {
        startPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
        LevelGrid.Instance.AddBallAtGridPosition(this, startPosition);
    }

    private void OnMouseDown()
    {
        if (!IsPlayerTurn())
            return;

        LevelGrid.Instance.HighlightValidMovePosition(startPosition);

        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // Store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();

    }

    private void OnMouseDrag()
    {
        if (!IsPlayerTurn())
            return;

        this.transform.position = GetMouseAsWorldPoint() + mOffset;
    }

    private void OnMouseUp()
    {
        if (!IsPlayerTurn())
            return;

        Vector2 nearestPosition = GetNearestPosition(this.transform.position);
        GridPosition endPosition = LevelGrid.Instance.GetGridPosition(nearestPosition);
        //bool isValidMovement = LevelGrid.Instance.IsValidGridPosition(endPosition);
        bool isValidMovement = LevelGrid.Instance.IsValidMovement(startPosition, endPosition);
        bool isSamePosition = (startPosition == endPosition);

        if (isValidMovement && !isSamePosition)
        {
            MoveOnNearestGridPosition(moveDuration);
            LevelGrid.Instance.AddBallAtGridPosition(this, endPosition);
            LevelGrid.Instance.RemoveBallAtGridPosition(startPosition);
            LevelGrid.Instance.RemoveValidMovePosition();
            startPosition = endPosition;
            OnAnyBallMoved?.Invoke(this, EventArgs.Empty);
            NextTurn();
        }
        else
        {
            MoveOnGridPosition(startPosition, moveDuration);
            LevelGrid.Instance.RemoveValidMovePosition();
        }

    }

    private void MoveOnNearestGridPosition(float moveDuration)
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
        Vector2 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        Vector2 worldPositionToMove = GetNearestPosition(this.transform.position);

        transform.DOMove(worldPositionToMove, moveDuration);
    }

    private void MoveOnGridPosition(GridPosition gridPositionToMove, float moveDuration)
    {
        Vector2 worldPositionToMove = LevelGrid.Instance.GetWorldPosition(gridPositionToMove);

        transform.DOMove(worldPositionToMove, moveDuration);
    }

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

}
