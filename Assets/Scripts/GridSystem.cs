using UnityEngine;

public class GridSystem
{
    private int width;
    private int height;
    private float cellSize;
    private GridObject[,] gridObjectMatrix;

    public GridSystem(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectMatrix = new GridObject[width, height];






        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)







            {
                GridPosition gridPosition = new GridPosition(x, y);
                gridObjectMatrix[x, y] = new GridObject(gridPosition);



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
        return gridObjectMatrix[gridPosition.x, gridPosition.y];
    }

    public GridObject[,] GetGridObjectArray()
    {
        return gridObjectMatrix;
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

    /*
    public void CreateValidPositionDebugObjects(Transform validDebugPrefab, Transform invalidDebugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridPosition gridPosition = new GridPosition(x, y);
                if (RuleSystem.Instance.IsValidGridPosition(gridPosition))
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
    */

}