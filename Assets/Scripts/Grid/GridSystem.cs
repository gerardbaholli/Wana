using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using System.Collections;
using static UnityEditor.Progress;

public class GridSystem
{

    private int width;
    private int height;
    private float cellSize;
    private GridObject[,] gridObjectArray;

    public GridSystem(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new GridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridPosition gridPosition = new GridPosition(x, y);
                gridObjectArray[x, y] = new GridObject(this, gridPosition);
            }
        }
    }

    public Vector2 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector2(gridPosition.x, gridPosition.y) * cellSize;
    }

    public GridPosition GetGridPosition(Vector2 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.y / cellSize)
            );
    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.y];
    }

    public GridObject[,] GetGridObjectArray()
    {
        return gridObjectArray;
    }

    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridPosition gridPosition = new GridPosition(x, y);
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public void CreateValidPositionDebugObjects(Transform validDebugPrefab, Transform invalidDebugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridPosition gridPosition = new GridPosition(x, y);
                if (IsValidGridPosition(gridPosition))
                {
                    Transform debugTransform = GameObject.Instantiate(validDebugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                    GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                    gridDebugObject.SetGridObject(GetGridObject(gridPosition));
                }
                else
                {
                    Transform debugTransform = GameObject.Instantiate(invalidDebugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                    GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                    gridDebugObject.SetGridObject(GetGridObject(gridPosition));
                }
            }
        }
    }

    public void HighlightValidMovePosition(Transform highlightPrefab, GridPosition ballGridPosition)
    {
        List<GridPosition> possibleMovePositionList = GetPossibleMovementList(ballGridPosition);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridPosition gridPosition = new GridPosition(x, y);

                if (possibleMovePositionList.Contains(gridPosition))
                {
                    Transform highlightTransform = GameObject.Instantiate(highlightPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                    GridValidObject gridValidObject = highlightTransform.GetComponent<GridValidObject>();
                    gridValidObject.SetGridObject(GetGridObject(gridPosition));
                }
            }
        }
    }

    private List<GridPosition> GetPossibleMovementList(GridPosition ballGridPosition)
    {
        List<GridPosition> possibleLinearMovementList;
        List<GridPosition> possibleCircularMovementList;
        List<GridPosition> allPossibleMovementList = new List<GridPosition>();

        possibleLinearMovementList = GetLinearMovement(ballGridPosition);
        possibleCircularMovementList = GetCircularMovement(ballGridPosition);

        // Combines the two list into allPossibleMovePositionList
        allPossibleMovementList.AddRange(possibleLinearMovementList);
        foreach (GridPosition gridPosition in possibleCircularMovementList)
        {
            if (!allPossibleMovementList.Contains(gridPosition))
                allPossibleMovementList.Add(gridPosition);
        }

        return allPossibleMovementList;
    }

    private List<GridPosition> GetLinearMovement(GridPosition ballGridPosition)
    {
        List<GridPosition> possibleLinearMovementList = new List<GridPosition>();

        GetUpDirection(ballGridPosition, possibleLinearMovementList);
        GetDownDirection(ballGridPosition, possibleLinearMovementList);
        GetLeftDirection(ballGridPosition, possibleLinearMovementList);
        GetRightDirection(ballGridPosition, possibleLinearMovementList);

        return possibleLinearMovementList;
    }

    private void GetRightDirection(GridPosition ballGridPosition, List<GridPosition> possibleLinearMovementList)
    {
        //Check right
        int rightIndex = ballGridPosition.x;

        for (int i = 0; i < 9; i++)
        {
            rightIndex++;

            if (rightIndex >= width)
            {
                rightIndex = rightIndex - width;
            }

            GridObject gridObject = gridObjectArray[rightIndex, ballGridPosition.y];
            bool hasAnyBall = gridObject.HasAnyBall();
            bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

            if (hasAnyBall || !isValidGridPosition)
                return;

            GridPosition possibleMovePosition = new GridPosition(rightIndex, ballGridPosition.y);
            possibleLinearMovementList.Add(possibleMovePosition);
        }
    }

    private void GetLeftDirection(GridPosition ballGridPosition, List<GridPosition> possibleLinearMovementList)
    {
        int leftIndex = ballGridPosition.x;

        for (int i = 0; i < 9; i++)
        {
            leftIndex--;

            if (leftIndex < 0)
            {
                leftIndex = leftIndex + width;
            }

            GridObject gridObject = gridObjectArray[leftIndex, ballGridPosition.y];
            bool hasAnyBall = gridObject.HasAnyBall();
            bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

            if (hasAnyBall || !isValidGridPosition)
                return;

            GridPosition possibleMovePosition = new GridPosition(leftIndex, ballGridPosition.y);
            possibleLinearMovementList.Add(possibleMovePosition);
        }
    }

    private void GetDownDirection(GridPosition ballGridPosition, List<GridPosition> possibleLinearMovementList)
    {
        //Check down
        int downIndex = ballGridPosition.y;

        for (int i = 0; i < 9; i++)
        {
            downIndex--;

            if (downIndex < 0)
            {
                downIndex = downIndex + height;
            }

            GridObject gridObject = gridObjectArray[ballGridPosition.x, downIndex];
            bool hasAnyBall = gridObject.HasAnyBall();
            bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

            if (hasAnyBall || !isValidGridPosition)
                return;

            GridPosition possibleMovePosition = new GridPosition(ballGridPosition.x, downIndex);
            possibleLinearMovementList.Add(possibleMovePosition);

        }
    }

    private void GetUpDirection(GridPosition ballGridPosition, List<GridPosition> possibleLinearMovementList)
    {
        int upIndex = ballGridPosition.y;

        for (int i = 0; i < 9; i++)
        {
            upIndex++;

            if (upIndex >= height)
            {
                upIndex = upIndex - height;
            }

            GridObject gridObject = gridObjectArray[ballGridPosition.x, upIndex];
            bool hasAnyBall = gridObject.HasAnyBall();
            bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

            if (hasAnyBall || !isValidGridPosition)
                return;

            GridPosition possibleMovePosition = new GridPosition(ballGridPosition.x, upIndex);
            possibleLinearMovementList.Add(possibleMovePosition);
        }
    }

    private List<GridPosition> GetCircularMovement(GridPosition ballGridPosition)
    {
        List<GridPosition> possibleCircularMovementList = new List<GridPosition>();

        GetClockwiseDirection(ballGridPosition, possibleCircularMovementList);
        GetCounterclockwiseDirection(ballGridPosition, possibleCircularMovementList);

        return possibleCircularMovementList;
    }

    private void GetClockwiseDirection(GridPosition ballGridPosition, List<GridPosition> possibleCircularMovementList)
    {
        GridPosition tempBallGridPosition = new GridPosition(ballGridPosition.x, ballGridPosition.y);

        int ballQuadrant = GetGridPositionQuadrant(tempBallGridPosition);
        int ballRing = GetGridPositionRing(tempBallGridPosition);

        for (int i = 0; i < 12; i++)
        {

            if (ballQuadrant == 8)
            {
                tempBallGridPosition.x++;

                GridObject gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                bool hasAnyBall = gridObject.HasAnyBall();
                bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

                if (hasAnyBall)
                    return;

                if (!isValidGridPosition)
                {
                    int temp = --tempBallGridPosition.x;
                    tempBallGridPosition.x = tempBallGridPosition.y;
                    tempBallGridPosition.y = temp;

                    gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                    if (gridObject.HasAnyBall())
                        return;

                    possibleCircularMovementList.Add(tempBallGridPosition);

                    ballQuadrant = GetGridPositionQuadrant(tempBallGridPosition);
                    continue;
                }

                GridPosition possibleMovePosition = new GridPosition(tempBallGridPosition.x, tempBallGridPosition.y);
                possibleCircularMovementList.Add(possibleMovePosition);
            }

            if (ballQuadrant == 6)
            {
                tempBallGridPosition.y--;

                GridObject gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                bool hasAnyBall = gridObject.HasAnyBall();
                bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

                if (hasAnyBall)
                    return;

                if (!isValidGridPosition)
                {
                    tempBallGridPosition.x = tempBallGridPosition.x - ballRing;
                    tempBallGridPosition.y = ++tempBallGridPosition.y - ballRing;

                    gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                    if (gridObject.HasAnyBall())
                        return;

                    possibleCircularMovementList.Add(tempBallGridPosition);

                    ballQuadrant = GetGridPositionQuadrant(tempBallGridPosition);
                    continue;
                }

                GridPosition possibleMovePosition = new GridPosition(tempBallGridPosition.x, tempBallGridPosition.y);
                possibleCircularMovementList.Add(possibleMovePosition);
            }

            if (ballQuadrant == 2)
            {
                tempBallGridPosition.x--;

                GridObject gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                bool hasAnyBall = gridObject.HasAnyBall();
                bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

                if (hasAnyBall)
                    return;

                if (!isValidGridPosition)
                {
                    int temp = ++tempBallGridPosition.x;
                    tempBallGridPosition.x = tempBallGridPosition.y;
                    tempBallGridPosition.y = temp;

                    gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                    if (gridObject.HasAnyBall())
                        return;

                    possibleCircularMovementList.Add(tempBallGridPosition);

                    ballQuadrant = GetGridPositionQuadrant(tempBallGridPosition);
                    continue;
                }

                GridPosition possibleMovePosition = new GridPosition(tempBallGridPosition.x, tempBallGridPosition.y);
                possibleCircularMovementList.Add(possibleMovePosition);
            }

            if (ballQuadrant == 4)
            {
                tempBallGridPosition.y++;

                GridObject gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                bool hasAnyBall = gridObject.HasAnyBall();
                bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

                if (hasAnyBall)
                    return;

                if (!isValidGridPosition)
                {
                    tempBallGridPosition.x = tempBallGridPosition.x + ballRing;
                    tempBallGridPosition.y = --tempBallGridPosition.y + ballRing;

                    gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                    if (gridObject.HasAnyBall())
                        return;

                    possibleCircularMovementList.Add(tempBallGridPosition);

                    ballQuadrant = GetGridPositionQuadrant(tempBallGridPosition);
                    continue;
                }

                GridPosition possibleMovePosition = new GridPosition(tempBallGridPosition.x, tempBallGridPosition.y);
                possibleCircularMovementList.Add(possibleMovePosition);
            }

        }
    }

    private void GetCounterclockwiseDirection(GridPosition ballGridPosition, List<GridPosition> possibleCircularMovementList)
    {
        GridPosition tempBallGridPosition = new GridPosition(ballGridPosition.x, ballGridPosition.y);

        int ballQuadrant = GetGridPositionQuadrant(tempBallGridPosition);
        int ballRing = GetGridPositionRing(tempBallGridPosition);

        for (int i = 0; i < 12; i++)
        {

            if (ballQuadrant == 8)
            {
                tempBallGridPosition.x--;

                GridObject gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                bool hasAnyBall = gridObject.HasAnyBall();
                bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

                if (hasAnyBall)
                    return;

                if (!isValidGridPosition)
                {
                    tempBallGridPosition.x = ++tempBallGridPosition.x - ballRing;
                    tempBallGridPosition.y = tempBallGridPosition.y - ballRing;

                    possibleCircularMovementList.Add(tempBallGridPosition);

                    gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                    if (gridObject.HasAnyBall())
                        return;

                    ballQuadrant = GetGridPositionQuadrant(tempBallGridPosition);
                    continue;
                }

                GridPosition possibleMovePosition = new GridPosition(tempBallGridPosition.x, tempBallGridPosition.y);
                possibleCircularMovementList.Add(possibleMovePosition);
            }

            if (ballQuadrant == 4)
            {
                tempBallGridPosition.y--;

                GridObject gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                bool hasAnyBall = gridObject.HasAnyBall();
                bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

                if (hasAnyBall)
                    return;

                if (!isValidGridPosition)
                {
                    int temp = tempBallGridPosition.x;
                    tempBallGridPosition.x = ++tempBallGridPosition.y;
                    tempBallGridPosition.y = temp;

                    gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                    if (gridObject.HasAnyBall())
                        return;

                    possibleCircularMovementList.Add(tempBallGridPosition);

                    ballQuadrant = GetGridPositionQuadrant(tempBallGridPosition);
                    continue;
                }

                GridPosition possibleMovePosition = new GridPosition(tempBallGridPosition.x, tempBallGridPosition.y);
                possibleCircularMovementList.Add(possibleMovePosition);
            }

            if (ballQuadrant == 2)
            {
                tempBallGridPosition.x++;

                GridObject gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                bool hasAnyBall = gridObject.HasAnyBall();
                bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

                if (hasAnyBall)
                    return;

                if (!isValidGridPosition)
                {
                    tempBallGridPosition.x = --tempBallGridPosition.x + ballRing;
                    tempBallGridPosition.y = tempBallGridPosition.y + ballRing;

                    gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                    if (gridObject.HasAnyBall())
                        return;

                    possibleCircularMovementList.Add(tempBallGridPosition);

                    ballQuadrant = GetGridPositionQuadrant(tempBallGridPosition);
                    continue;
                }

                GridPosition possibleMovePosition = new GridPosition(tempBallGridPosition.x, tempBallGridPosition.y);
                possibleCircularMovementList.Add(possibleMovePosition);
            }

            if (ballQuadrant == 6)
            {
                tempBallGridPosition.y++;

                GridObject gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                bool hasAnyBall = gridObject.HasAnyBall();
                bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

                if (hasAnyBall)
                    return;

                if (!isValidGridPosition)
                {
                    int temp = tempBallGridPosition.x;
                    tempBallGridPosition.x = --tempBallGridPosition.y;
                    tempBallGridPosition.y = temp;

                    gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                    if (gridObject.HasAnyBall())
                        return;

                    possibleCircularMovementList.Add(tempBallGridPosition);

                    ballQuadrant = GetGridPositionQuadrant(tempBallGridPosition);
                    continue;
                }

                GridPosition possibleMovePosition = new GridPosition(tempBallGridPosition.x, tempBallGridPosition.y);
                possibleCircularMovementList.Add(possibleMovePosition);
            }

        }
    }

    /*
     *  321|...|123     0 if null
     */
    private int GetGridPositionRing(GridPosition gridPosition)
    {
        int gridPositionQuadrant = GetGridPositionQuadrant(gridPosition);

        if (gridPositionQuadrant == 8)
        {
            if (gridPosition.y == 8) { return 3; }
            if (gridPosition.y == 7) { return 2; }
            if (gridPosition.y == 6) { return 1; }
        }
        else if (gridPositionQuadrant == 6)
        {
            if (gridPosition.x == 6) { return 1; }
            if (gridPosition.x == 7) { return 2; }
            if (gridPosition.x == 8) { return 3; }
        }
        else if (gridPositionQuadrant == 2)
        {
            if (gridPosition.y == 2) { return 1; }
            if (gridPosition.y == 1) { return 2; }
            if (gridPosition.y == 0) { return 3; }
        }
        else if (gridPositionQuadrant == 4)
        {
            if (gridPosition.x == 2) { return 3; }
            if (gridPosition.x == 1) { return 2; }
            if (gridPosition.x == 0) { return 1; }
        }

        return 0;
    }

    /*
     *  7|8|9
     *  4|5|6
     *  1|2|3           0 if null
     */
    private int GetGridPositionQuadrant(GridPosition gridPosition)
    {
        if ((3 <= gridPosition.x && gridPosition.x <= 5) && (3 <= gridPosition.y && gridPosition.y <= 5))
        {
            // Q5
            return 5;
        }

        if ((3 <= gridPosition.x && gridPosition.x <= 5) && (0 <= gridPosition.y && gridPosition.y <= 2))
        {
            // Q2
            return 2;
        }

        if ((0 <= gridPosition.x && gridPosition.x <= 2) && (3 <= gridPosition.y && gridPosition.y <= 5))
        {
            // Q4
            return 4;
        }

        if ((6 <= gridPosition.x && gridPosition.x <= 8) && (3 <= gridPosition.y && gridPosition.y <= 5))
        {
            // Q6
            return 6;
        }

        if ((3 <= gridPosition.x && gridPosition.x <= 5) && (6 <= gridPosition.y && gridPosition.y <= 8))
        {
            // Q8
            return 8;
        }

        if ((0 <= gridPosition.x && gridPosition.x <= 2) && (0 <= gridPosition.y && gridPosition.y <= 2))
        {
            // Q1
            return 1;
        }

        if ((6 <= gridPosition.x && gridPosition.x <= 8) && (0 <= gridPosition.y && gridPosition.y <= 2))
        {
            // Q3
            return 3;
        }

        if ((0 <= gridPosition.x && gridPosition.x <= 2) && (6 <= gridPosition.y && gridPosition.y <= 8))
        {
            // Q7
            return 7;
        }

        if ((6 <= gridPosition.x && gridPosition.x <= 8) && (6 <= gridPosition.y && gridPosition.y <= 8))
        {
            // Q9
            return 9;
        }

        return 0;
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        if ((0 <= gridPosition.x && gridPosition.x <= 2) && (0 <= gridPosition.y && gridPosition.y <= 2))
        {
            //Debug.Log("Invalid position: Q1");
            return false;
        }

        if ((6 <= gridPosition.x && gridPosition.x <= 8) && (0 <= gridPosition.y && gridPosition.y <= 2))
        {
            //Debug.Log("Invalid position: Q3");
            return false;
        }

        if ((0 <= gridPosition.x && gridPosition.x <= 2) && (6 <= gridPosition.y && gridPosition.y <= 8))
        {
            //Debug.Log("Invalid position: Q7");
            return false;
        }

        if ((6 <= gridPosition.x && gridPosition.x <= 8) && (6 <= gridPosition.y && gridPosition.y <= 8))
        {
            //Debug.Log("Invalid position: Q9");
            return false;
        }

        //Debug.Log("Valid position: [" + gridPosition.x + ", " + gridPosition.y + "]");
        return true;
    }

    public bool IsValidMovement(GridPosition startingPosition, GridPosition endPosition)
    {
        if (!IsValidGridPosition(endPosition))
            return false;

        List<GridPosition> allPossibleMovePositionList = new List<GridPosition>();

        allPossibleMovePositionList = GetPossibleMovementList(startingPosition);

        if (allPossibleMovePositionList.Contains(endPosition))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

}
