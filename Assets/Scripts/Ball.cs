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

    private GridPosition startingPosition;

    private void Start()
    {
        startingPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
    }

    private void OnMouseDown()
    {
        //startingPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);

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
        bool isValidMovement = LevelGrid.Instance.IsValidGridPosition(endPosition);

        if (isValidMovement)
        {
            MoveOnNearestGridPosition(moveDuration);
            startingPosition = endPosition;
            NextTurn();
        }
        else
        {
            MoveOnGridPosition(startingPosition, moveDuration);
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
