using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    [Header("Grid Settings")]
    [SerializeField] private int width = 9;
    [SerializeField] private int height = 9;
    [SerializeField] private float cellSize = 2f;

    [Header("Debug Options")]
    [SerializeField] bool isDebugActive = false;
    [SerializeField] private Transform gridDebugObjectPrefab;


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
        }
    }

    public void AddBallAtGridPosition(GridPosition gridPosition, Ball ball)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddBall(ball);
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

    /* This is the same thing:
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return gridSystem.GetGridPosition(worldPosition);
    }
    */

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

    public float GetCellSize() => gridSystem.GetCellSize();

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