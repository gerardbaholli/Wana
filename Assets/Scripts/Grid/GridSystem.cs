using UnityEngine;
using TMPro;

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

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        if ((0 <= gridPosition.x && gridPosition.x <= 2) && (0 <= gridPosition.y && gridPosition.y <= 2))
        {
            Debug.Log("Invalid position: Q1");
            return false;
        }

        if ((6 <= gridPosition.x && gridPosition.x <= 8) && (0 <= gridPosition.y && gridPosition.y <= 2))
        {
            Debug.Log("Invalid position: Q3");
            return false;
        }

        if ((0 <= gridPosition.x && gridPosition.x <= 2) && (6 <= gridPosition.y && gridPosition.y <= 8))
        {
            Debug.Log("Invalid position: Q7");
            return false;
        }

        if ((6 <= gridPosition.x && gridPosition.x <= 8) && (6 <= gridPosition.y && gridPosition.y <= 8))
        {
            Debug.Log("Invalid position: Q9");
            return false;
        }

        //Debug.Log("Valid position: [" + gridPosition.x + ", " + gridPosition.y + "]");
        return true;
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