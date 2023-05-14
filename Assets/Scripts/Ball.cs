using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Ball : MonoBehaviour
{

    public enum Part
    {
        Player,
        Enemy
    }

    [SerializeField] Part part;
    [SerializeField] float moveDuration = 0.5f;

    private Vector3 mOffset;
    private float mZCoord;


    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // Store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + mOffset;
    }

    private void OnMouseUp()
    {
        MoveOnGridPosition(moveDuration);
    }

    private void MoveOnGridPosition(float moveDuration)
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
        Vector2 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        Vector2 worldPositionToMove = GetNearestPosition(this.transform.position);

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

}
