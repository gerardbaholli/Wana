using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    [SerializeField] private Transform highlightPrefab;

    private int width = 9;
    private int height = 9;
    private float cellSize = 2f;

    private GridSystem gridSystem;
    private Ball selectedBall;
    private Ball[] ballList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one LevelGrid! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gridSystem = new GridSystem(width, height, cellSize);
    }

    private void Start()
    {
        ballList = FindObjectsByType<Ball>(FindObjectsSortMode.None);

        Ball.OnAnyBallSelected += Ball_OnAnyBallSelected;
        GridValidObject.OnValidPositionSelected += GridValidObject_OnValidPositionSelected;
    }

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

    public float GetCellSize() => gridSystem.GetCellSize();

    public GridPosition GetGridPosition(Vector2 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public GridObject GetGridObject(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition);

    public GridObject[,] GetGridObjectArray() => gridSystem.GetGridObjectArray();

    public void AddBallAtGridPosition(Ball ball, GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);

        if (!gridObject.HasAnyPawn())
        {
            gridObject.AddBall(ball);
        }
    }

    public void RemoveBallAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);

        if (gridObject.HasAnyPawn())
        {
            gridObject.RemoveBall();
        }
    }

    public bool HasBallAtGridPosition(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition).HasAnyPawn();

    public void RemoveValidMovePosition()
    {
        GridValidObject[] gridValidObjects = FindObjectsByType<GridValidObject>(FindObjectsSortMode.None);

        foreach (GridValidObject gridValidObject in gridValidObjects)
        {
            Destroy(gridValidObject.gameObject);
        }
    }

    private void GridValidObject_OnValidPositionSelected(object sender, EventArgs e)
    {
        GridValidObject gridValidObject = sender as GridValidObject;
        Vector2 worldPosition = gridValidObject.transform.position;
        GridPosition endPosition = GetGridPosition(worldPosition);

        selectedBall.MakeMove(endPosition);
    }

    private void Ball_OnAnyBallSelected(object sender, EventArgs e)
    {
        selectedBall = sender as Ball;

        for (int i = 0; i < ballList.Length; i++)
        {
            if (ballList[i] != selectedBall)
            {
                ballList[i].DeselectBall();
            };
        }
    }

}
