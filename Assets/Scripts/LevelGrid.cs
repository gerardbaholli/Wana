using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    [Header("Debug Options")]
    [SerializeField] bool isDebugActive = false;
    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private Transform validPositionDebugObject;
    [SerializeField] private Transform invalidPositionDebugObject;
    [SerializeField] private Transform highlightPrefab;

    private int width = 9;
    private int height = 9;
    private float cellSize = 2f;

    private GridSystem gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Here we create the GridSystem
        gridSystem = new GridSystem(width, height, cellSize);

        if (isDebugActive)
        {
            gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
            //gridSystem.CreateValidPositionDebugObjects(validPositionDebugObject);
        }

        // to remove
        //gridSystem.CreateValidPositionDebugObjects(validPositionDebugObject, invalidPositionDebugObject);
        //gridSystem.HighlightValidMovePosition(highlightPrefab);

    }

    private void Start()
    {
        Ball.OnAnyBallMoved += Ball_OnAnyBallMoved;
    }

    private void Ball_OnAnyBallMoved(object sender, EventArgs e)
    {
        if (IsGameOver())
        {
            Debug.Log("GAME OVER");
        }
    }

    private bool IsGameOver()
    {
        GridObject[,] gridObject = gridSystem.GetGridObjectArray();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!gridObject[x, y].HasAnyBall())
                    continue;

                int upY = y + 1;
                int downY = y - 1;
                int leftX = x - 1;
                int rightX = x + 1;

                if (upY >= height)
                    upY = upY - height;

                if (downY < 0)
                    downY = downY + height;

                if (leftX < 0)
                    leftX = leftX + width;

                if (rightX >= width)
                    rightX = rightX - width;

                //Debug.Log(upY + " " + downY + " " + leftX + " " + rightX);

                bool hasBallUp = gridObject[x, upY].HasAnyBall();
                bool hasBallDown = gridObject[x, downY].HasAnyBall();
                bool hasBallLeft = gridObject[leftX, y].HasAnyBall();
                bool hasBallRight = gridObject[rightX, y].HasAnyBall();

                if (hasBallUp && hasBallDown && hasBallLeft && hasBallRight)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void AddBallAtGridPosition(Ball ball, GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);

        if (!gridObject.HasAnyBall())
        {
            gridObject.AddBall(ball);
        }
    }

    public void RemoveBallAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);

        if (gridObject.HasAnyBall())
        {
            gridObject.RemoveBall();
        }
    }

    //public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    //{
    //    GridObject gridObject = gridSystem.GetGridObject(gridPosition);
    //    return gridObject.GetUnitList();
    //}

    //public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    //{
    //    GridObject gridObject = gridSystem.GetGridObject(gridPosition);
    //    gridObject.RemoveUnit(unit);
    //}

    //public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    //{
    //    RemoveUnitAtGridPosition(fromGridPosition, unit);
    //    AddUnitAtGridPosition(toGridPosition, unit);
    //}

    public GridPosition GetGridPosition(Vector2 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    //public int GetGridPositionQuadrant(GridPosition gridPosition) => gridSystem.GetGridPositionQuadrant(gridPosition);

    /* This is the same thing:
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return gridSystem.GetGridPosition(worldPosition);
    }
    */

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public bool IsValidMovement(GridPosition startPosition, GridPosition endPosition) => gridSystem.IsValidMovement(startPosition, endPosition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

    public float GetCellSize() => gridSystem.GetCellSize();

    public void HighlightValidMovePosition(GridPosition ballGridPosition) => gridSystem.HighlightValidMovePosition(highlightPrefab, ballGridPosition);

    public void RemoveValidMovePosition()
    {
        GridValidObject[] gridValidObjects = FindObjectsOfType<GridValidObject>();

        //Debug.Log(gridValidObjects.Length);

        foreach (GridValidObject gridValidObject in gridValidObjects)
        {
            Destroy(gridValidObject.gameObject);
        }
    }

    //public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    //{
    //    GridObject gridObject = gridSystem.GetGridObject(gridPosition);
    //    return gridObject.HasAnyUnit();
    //}

    //public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    //{
    //    GridObject gridObject = gridSystem.GetGridObject(gridPosition);
    //    return gridObject.GetUnit();
    //}

}