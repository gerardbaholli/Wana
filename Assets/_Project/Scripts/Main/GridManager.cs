using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wana
{
    public class GridManager : MonoBehaviour
    {

        public static GridManager Instance { get; private set; }

        [SerializeField] private GridSettingSO gridSettingSO;

        [SerializeField] private bool debug = false;
        [SerializeField] private GridDebugObject gridDebugObjectPrefab;

        private int width;
        private int height;
        private float cellSize;
        private float cellSpacing;

        private GridSystem gridSystem;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There's more than one GridManager! " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            Instance = this;

            width = gridSettingSO.gridSize;
            height = gridSettingSO.gridSize;
            cellSize = gridSettingSO.cellSize;
            cellSpacing = gridSettingSO.cellSpacing;

            gridSystem = new GridSystem(width, height, cellSize, cellSpacing);

            if (debug)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        GridPosition gridPosition = new GridPosition(x, y);
                        Vector2 worldPosition = gridSystem.GetWorldPosition(gridPosition);
                        GridDebugObject gridDebugObject = Instantiate(gridDebugObjectPrefab, worldPosition, Quaternion.identity);
                        gridDebugObject.SetGridObject(gridPosition);
                    }
                }
            }

        }

        public int GetWidth() => gridSystem.GetWidth();

        public int GetHeight() => gridSystem.GetHeight();

        public float GetCellSize() => gridSystem.GetCellSize();

        public GridPosition? GetGridPosition(Vector2 worldPosition) => gridSystem.GetGridPosition(worldPosition);

        public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

        public GridObject GetGridObject(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition);

        public GridObject[,] GetGridObjectArray() => gridSystem.GetGridObjectArray();

        // public Vector2 GetGridCenterPosition() => gridSystem.GetGridCenterPosition();

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            float cellSizeWithSpacing = gridSettingSO.cellSize - gridSettingSO.cellSpacing * 2;

            for (int x = 0; x < gridSettingSO.gridSize; x++)
            {
                for (int y = 0; y < gridSettingSO.gridSize; y++)
                {
                    GridPosition gridPosition = new GridPosition(x, y);
                    Vector2 worldPosition = new Vector2(gridPosition.x, gridPosition.y) * gridSettingSO.cellSize;
                    Gizmos.DrawWireCube(
                        new Vector2(worldPosition.x, worldPosition.y),
                        new Vector2(cellSizeWithSpacing, cellSizeWithSpacing)
                    );
                }
            }

        }

    }
}