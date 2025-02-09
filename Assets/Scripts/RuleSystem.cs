using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleSystem : MonoBehaviour
{

    public static RuleSystem Instance { get; private set; }

    [SerializeField] private Transform highlightPrefab;

    private int width;
    private int height;
    private GridObject[,] gridObjectArray;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one RuleSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        width = LevelGrid.Instance.GetWidth();
        height = LevelGrid.Instance.GetHeight();
        gridObjectArray = LevelGrid.Instance.GetGridObjectArray();
    }

    public void HighlightValidMovePosition(GridPosition ballGridPosition)
    {
        List<GridPosition> possibleMovePositionList = GetPossibleMovementList(ballGridPosition);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridPosition gridPosition = new GridPosition(x, y);

                if (possibleMovePositionList.Contains(gridPosition))
                {
                    Transform highlightTransform = GameObject.Instantiate(highlightPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                    GridValidObject gridValidObject = highlightTransform.GetComponent<GridValidObject>();
                    gridValidObject.SetGridObject(LevelGrid.Instance.GetGridObject(gridPosition));
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
            bool hasAnyPawn = gridObject.HasAnyPawn();
            bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

            if (hasAnyPawn || !isValidGridPosition)
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
            bool hasAnyPawn = gridObject.HasAnyPawn();
            bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

            if (hasAnyPawn || !isValidGridPosition)
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
            bool hasAnyPawn = gridObject.HasAnyPawn();
            bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

            if (hasAnyPawn || !isValidGridPosition)
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
            bool hasAnyPawn = gridObject.HasAnyPawn();
            bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

            if (hasAnyPawn || !isValidGridPosition)
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
                bool hasAnyPawn = gridObject.HasAnyPawn();
                bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

                if (hasAnyPawn)
                    return;

                if (!isValidGridPosition)
                {
                    int temp = --tempBallGridPosition.x;
                    tempBallGridPosition.x = tempBallGridPosition.y;
                    tempBallGridPosition.y = temp;

                    gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                    if (gridObject.HasAnyPawn())
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
                bool hasAnyPawn = gridObject.HasAnyPawn();
                bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

                if (hasAnyPawn)
                    return;

                if (!isValidGridPosition)
                {
                    tempBallGridPosition.x = tempBallGridPosition.x - ballRing;
                    tempBallGridPosition.y = ++tempBallGridPosition.y - ballRing;

                    gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                    if (gridObject.HasAnyPawn())
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
                bool hasAnyPawn = gridObject.HasAnyPawn();
                bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

                if (hasAnyPawn)
                    return;

                if (!isValidGridPosition)
                {
                    int temp = ++tempBallGridPosition.x;
                    tempBallGridPosition.x = tempBallGridPosition.y;
                    tempBallGridPosition.y = temp;

                    gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                    if (gridObject.HasAnyPawn())
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
                bool hasAnyPawn = gridObject.HasAnyPawn();
                bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

                if (hasAnyPawn)
                    return;

                if (!isValidGridPosition)
                {
                    tempBallGridPosition.x = tempBallGridPosition.x + ballRing;
                    tempBallGridPosition.y = --tempBallGridPosition.y + ballRing;

                    gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                    if (gridObject.HasAnyPawn())
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
                bool hasAnyPawn = gridObject.HasAnyPawn();
                bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

                if (hasAnyPawn)
                    return;

                if (!isValidGridPosition)
                {
                    tempBallGridPosition.x = ++tempBallGridPosition.x - ballRing;
                    tempBallGridPosition.y = tempBallGridPosition.y - ballRing;

                    possibleCircularMovementList.Add(tempBallGridPosition);

                    gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                    if (gridObject.HasAnyPawn())
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
                bool hasAnyPawn = gridObject.HasAnyPawn();
                bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

                if (hasAnyPawn)
                    return;

                if (!isValidGridPosition)
                {
                    int temp = tempBallGridPosition.x;
                    tempBallGridPosition.x = ++tempBallGridPosition.y;
                    tempBallGridPosition.y = temp;

                    gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                    if (gridObject.HasAnyPawn())
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
                bool hasAnyPawn = gridObject.HasAnyPawn();
                bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

                if (hasAnyPawn)
                    return;

                if (!isValidGridPosition)
                {
                    tempBallGridPosition.x = --tempBallGridPosition.x + ballRing;
                    tempBallGridPosition.y = tempBallGridPosition.y + ballRing;

                    gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                    if (gridObject.HasAnyPawn())
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
                bool hasAnyPawn = gridObject.HasAnyPawn();
                bool isValidGridPosition = IsValidGridPosition(gridObject.GetGridPosition());

                if (hasAnyPawn)
                    return;

                if (!isValidGridPosition)
                {
                    int temp = tempBallGridPosition.x;
                    tempBallGridPosition.x = --tempBallGridPosition.y;
                    tempBallGridPosition.y = temp;

                    gridObject = gridObjectArray[tempBallGridPosition.x, tempBallGridPosition.y];
                    if (gridObject.HasAnyPawn())
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
            if (gridPosition.x == 2) { return 1; }
            if (gridPosition.x == 1) { return 2; }
            if (gridPosition.x == 0) { return 3; }
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

    public bool IsGameOver(out Ball ball)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridObject gridObject = gridObjectArray[x, y];

                if (!gridObject.HasAnyPawn())
                    continue;

                bool hasBallUp = HasAnyPawnUp(gridObject);
                bool hasBallDown = HasAnyPawnDown(gridObject);
                bool hasBallLeft = HasAnyPawnLeft(gridObject);
                bool hasBallRight = HasAnyPawnRight(gridObject);

                if (hasBallUp && hasBallDown && hasBallLeft && hasBallRight)
                {
                    ball = gridObject.GetBall();
                    return true;
                }
            }
        }

        ball = null;
        return false;
    }

    private bool HasAnyPawnUp(GridObject gridObject)
    {
        GridPosition gridPosition = gridObject.GetGridPosition();

        if ((3 <= gridPosition.x && gridPosition.x <= 5) ||
            (gridPosition.y == 3 || gridPosition.y == 4))
        {
            int upY = gridPosition.y + 1;

            if (gridPosition.y == 8)
                upY = 0;

            return gridObjectArray[gridPosition.x, upY].HasAnyPawn();
        }
        else
        {
            int gridPositionRing = GetGridPositionRing(gridPosition);
            int gridPositionQuadrant = GetGridPositionQuadrant(gridPosition);

            GridPosition upGridPosition = new GridPosition();

            if (gridPositionQuadrant == 4)
            {
                upGridPosition.x = gridPosition.x + gridPositionRing;
                upGridPosition.y = gridPosition.y + gridPositionRing;
            }
            else if (gridPositionQuadrant == 6)
            {
                upGridPosition.x = gridPosition.y;
                upGridPosition.y = gridPosition.x;
            }

            return gridObjectArray[upGridPosition.x, upGridPosition.y].HasAnyPawn();
        }
    }

    private bool HasAnyPawnDown(GridObject gridObject)
    {
        GridPosition gridPosition = gridObject.GetGridPosition();

        if ((3 <= gridPosition.x && gridPosition.x <= 5) ||
            (gridPosition.y == 4 || gridPosition.y == 5))
        {
            int downY = gridPosition.y - 1;

            if (gridPosition.y == 0)
                downY = 8;

            return gridObjectArray[gridPosition.x, downY].HasAnyPawn();
        }
        else
        {
            int gridPositionRing = GetGridPositionRing(gridPosition);
            int gridPositionQuadrant = GetGridPositionQuadrant(gridPosition);

            GridPosition downGridPosition = new GridPosition();

            if (gridPositionQuadrant == 6)
            {
                downGridPosition.x = gridPosition.x - gridPositionRing;
                downGridPosition.y = gridPosition.y - gridPositionRing;
            }
            else if (gridPositionQuadrant == 4)
            {
                downGridPosition.x = gridPosition.y;
                downGridPosition.y = gridPosition.x;
            }

            return gridObjectArray[downGridPosition.x, downGridPosition.y].HasAnyPawn();
        }
    }

    private bool HasAnyPawnLeft(GridObject gridObject)
    {
        GridPosition gridPosition = gridObject.GetGridPosition();

        if ((3 <= gridPosition.y && gridPosition.y <= 5) ||
            (gridPosition.x == 4 || gridPosition.x == 5))
        {
            int leftX = gridPosition.x - 1;

            if (gridPosition.x == 0)
                leftX = 8;

            return gridObjectArray[leftX, gridPosition.y].HasAnyPawn();
        }
        else
        {
            int gridPositionRing = GetGridPositionRing(gridPosition);
            int gridPositionQuadrant = GetGridPositionQuadrant(gridPosition);

            GridPosition leftGridPosition = new GridPosition();

            if (gridPositionQuadrant == 8)
            {
                leftGridPosition.x = gridPosition.x - gridPositionRing;
                leftGridPosition.y = gridPosition.y - gridPositionRing;
            }
            else if (gridPositionQuadrant == 2)
            {
                leftGridPosition.x = gridPosition.y;
                leftGridPosition.y = gridPosition.x;
            }

            return gridObjectArray[leftGridPosition.x, leftGridPosition.y].HasAnyPawn();
        }
    }

    private bool HasAnyPawnRight(GridObject gridObject)
    {
        GridPosition gridPosition = gridObject.GetGridPosition();

        if ((3 <= gridPosition.y && gridPosition.y <= 5) ||
            (gridPosition.x == 3 || gridPosition.x == 4))
        {
            int rightX = gridPosition.x + 1;

            if (gridPosition.x == 8)
                rightX = 0;

            return gridObjectArray[rightX, gridPosition.y].HasAnyPawn();
        }
        else
        {
            int gridPositionRing = GetGridPositionRing(gridPosition);
            int gridPositionQuadrant = GetGridPositionQuadrant(gridPosition);

            GridPosition rightGridPosition = new GridPosition();

            if (gridPositionQuadrant == 2)
            {
                rightGridPosition.x = gridPosition.x + gridPositionRing;
                rightGridPosition.y = gridPosition.y + gridPositionRing;
            }
            else if (gridPositionQuadrant == 8)
            {
                rightGridPosition.x = gridPosition.y;
                rightGridPosition.y = gridPosition.x;
            }

            return gridObjectArray[rightGridPosition.x, rightGridPosition.y].HasAnyPawn();
        }
    }

    private void PrintGridPositionList(List<GridPosition> gridPositionList)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            Debug.Log(gridPosition.ToString());
        }
    }

}
