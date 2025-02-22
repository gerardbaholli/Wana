using UnityEngine;

namespace Wana
{
    public class GridSystem
    {

        private int width;
        private int height;
        private float cellSize;
        private float cellSpacing;

        private GridObject[,] gridObjectArray;

        public GridSystem(int width, int height, float cellSize, float cellSpacing)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.cellSpacing = cellSpacing;

            gridObjectArray = new GridObject[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GridPosition gridPosition = new GridPosition(x, y);
                    gridObjectArray[x, y] = new GridObject(this, gridPosition);
                }
            }

            this.cellSpacing = cellSpacing;
        }

        public Vector2 GetWorldPosition(GridPosition gridPosition)
        {
            return new Vector2(gridPosition.x, gridPosition.y) * cellSize;
        }

        public GridPosition? GetGridPosition(Vector2 worldPosition)
        {
            float halfCellSizeWithSpacing = (cellSize / 2) - cellSpacing;

            GridPosition gridPosition = new GridPosition(
                Mathf.RoundToInt(worldPosition.x / cellSize),
                Mathf.RoundToInt(worldPosition.y / cellSize)
            );
            Vector2 gridWorldPosition = GetWorldPosition(gridPosition);

            bool isInsideCell =
                Mathf.Abs(gridWorldPosition.x - worldPosition.x) <= halfCellSizeWithSpacing &&
                Mathf.Abs(gridWorldPosition.y - worldPosition.y) <= halfCellSizeWithSpacing;

            return isInsideCell ? gridPosition : null;
        }


        public GridObject GetGridObject(GridPosition? gridPosition)
        {
            if (!gridPosition.HasValue)
            {
                Debug.LogWarning("GridPosition is has not a value.");
                return null;
            }

            if (
                gridPosition.Value.x < 0 || gridPosition.Value.x >= gridObjectArray.GetLength(0) ||
                gridPosition.Value.y < 0 || gridPosition.Value.y >= gridObjectArray.GetLength(1)
                )
            {
                Debug.LogWarning("GridPosition outside index limits.");
                return null;
            }

            return gridObjectArray[gridPosition.Value.x, gridPosition.Value.y];
        }

        public GridObject[,] GetGridObjectArray()
        {
            return gridObjectArray;
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

        public Vector2 GetGridCenterPosition()
        {
            float centerX = width / 2 * cellSize;
            float centerY = height / 2 * cellSize;
            return new Vector2(centerX, centerY);
        }

    }
    
}