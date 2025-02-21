using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wana
{

    public class Game : MonoBehaviour
    {
        [SerializeField] private GameObject gameObjectDebug;
        [SerializeField] private GameObject player1PawnGO;
        [SerializeField] private GameObject player2PawnGO;
        [SerializeField] private PawnGO pawnGOPrefab;


        public event EventHandler OnMoveCompleted;


        private const int TOTAL_PAWNS_FOR_PLAYER = 8;

        private Board board;
        private List<Player> players;

        private List<Pawn> player1Pawns;
        private List<Pawn> player2Pawns;

        private void Start()
        {
            InitGridDebug();
            InitPawns();
            InitBoard();

            players = new List<Player>
            {
                new Player(0, "Player1", player1Pawns),
                new Player(1, "Player2", player2Pawns)
            };

            Pawn[,] pawnsMatrix = new Pawn[9, 9];

            // Popola la matrice con le pedine del player 1
            foreach (Pawn pawn in player1Pawns)
            {
                GridPosition pos = pawn.GetGridPosition();
                pawnsMatrix[pos.x, pos.y] = pawn;
            }

            // Popola la matrice con le pedine del player 2
            foreach (Pawn pawn in player2Pawns)
            {
                GridPosition pos = pawn.GetGridPosition();
                pawnsMatrix[pos.x, pos.y] = pawn;
            }

            board = new Board(pawnsMatrix);
            board.PrintBoard();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("Pressed A");
                board.EvaluateBoardForPlayer(players[0]);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("Pressed S");
                GridPosition gridPosition = new GridPosition(0, 0);
                Vector2 worldPosition = new Vector2(gridPosition.x, gridPosition.y);
                PawnGO pawnGO = Instantiate(pawnGOPrefab, worldPosition, Quaternion.identity);
                Pawn pawn = players[0].GetPawns()[0];
                pawnGO.SetPawn(pawn);
                pawnGO.SetLabel("1,1");
            }
        }

        private void InitPawns()
        {
            player1Pawns = new List<Pawn>();
            player2Pawns = new List<Pawn>();

            Color player1PawnsColor = new Color(0.5f, 0.84f, 0.85f, 1f);
            Color player2PawnsColor = new Color(0.85f, 0.5f, 0.75f, 1f);

            GameObject parentPawn = new GameObject("PawnContainer");

            for (int i = 0; i < TOTAL_PAWNS_FOR_PLAYER; i++)
            {
                PawnGO pawnGOX = Instantiate(pawnGOPrefab, new Vector2(), Quaternion.identity, parentPawn.transform);
                PawnGO pawnGOO = Instantiate(pawnGOPrefab, new Vector2(), Quaternion.identity, parentPawn.transform);
                pawnGOX.SetColor(player1PawnsColor);
                pawnGOO.SetColor(player2PawnsColor);
                Pawn pawnX = new Pawn(PawnType.X, pawnGOX);
                Pawn pawnO = new Pawn(PawnType.O, pawnGOO);

                player1Pawns.Add(pawnX);
                player2Pawns.Add(pawnO);
            }
        }

        private void InitGridDebug()
        {

            GameObject parentGridDebug = new GameObject("GridDebugContainer");

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Vector3 worldPosition = new Vector3(i * Board.CELL_SIZE, j * Board.CELL_SIZE, 1);
                    GameObject gameObject = Instantiate(gameObjectDebug, worldPosition, Quaternion.identity, parentGridDebug.transform);
                    gameObject.GetComponent<GridDebugObject>().SetTextMeshPro((worldPosition.x / Board.CELL_SIZE).ToString() + " " + (worldPosition.y / Board.CELL_SIZE).ToString());
                }
            }

        }

        private void InitBoard()
        {
            player1Pawns[0].SetGridPosition(new GridPosition(3, 0));
            player1Pawns[2].SetGridPosition(new GridPosition(5, 0));
            player1Pawns[3].SetGridPosition(new GridPosition(3, 1));
            player1Pawns[4].SetGridPosition(new GridPosition(5, 1));
            player1Pawns[1].SetGridPosition(new GridPosition(3, 2));
            player1Pawns[5].SetGridPosition(new GridPosition(5, 2));
            player1Pawns[6].SetGridPosition(new GridPosition(3, 3));
            player1Pawns[7].SetGridPosition(new GridPosition(5, 3));

            player2Pawns[0].SetGridPosition(new GridPosition(3, 5));
            player2Pawns[1].SetGridPosition(new GridPosition(5, 5));
            player2Pawns[2].SetGridPosition(new GridPosition(3, 6));
            player2Pawns[3].SetGridPosition(new GridPosition(5, 6));
            player2Pawns[4].SetGridPosition(new GridPosition(3, 7));
            player2Pawns[5].SetGridPosition(new GridPosition(5, 7));
            player2Pawns[6].SetGridPosition(new GridPosition(3, 8));
            player2Pawns[7].SetGridPosition(new GridPosition(5, 8));

            // foreach (Pawn pawn1 in player1Pawns)
            // {
            //     pawn1.UpdatePawnGOPosition();
            // }

            // foreach (Pawn pawn2 in player2Pawns)
            // {
            //     pawn2.UpdatePawnGOPosition();
            // }

            // GameObject parentPawn = new GameObject("PawnContainer");

            // foreach (Pawn pawn in player1Pawns)
            // {
            //     Vector3 worldPosition = new Vector3(pawn.GetGridPosition().x * Board.CELL_SIZE, pawn.GetGridPosition().y * Board.CELL_SIZE, 0);
            //     GameObject gameObject = Instantiate(player1PawnGO, worldPosition, Quaternion.identity, parentPawn.transform);
            // }

            // foreach (Pawn pawn in player2Pawns)
            // {
            //     Vector3 worldPosition = new Vector3(pawn.GetGridPosition().x * Board.CELL_SIZE, pawn.GetGridPosition().y * Board.CELL_SIZE, 0);
            //     GameObject gameObject = Instantiate(player2PawnGO, worldPosition, Quaternion.identity, parentPawn.transform);
            // }
        }


        private void OnEnable()
        {
            InputManager.Instance.OnMouseButtonDown += InputManager_OnMouseButtonDown;
            InputManager.Instance.OnMouseButtonUp += InputManager_HandleMouseUp;
        }

        private void OnDisable()
        {
            InputManager.Instance.OnMouseButtonDown -= InputManager_OnMouseButtonDown;
            InputManager.Instance.OnMouseButtonUp -= InputManager_HandleMouseUp;
        }

        private void InputManager_OnMouseButtonDown(object sender, MouseEventArgs e)
        {
            // Debug.Log($"Mouse Down at: {e.mouseWorldPosition}");
            // PawnGO pawnGO = Instantiate(pawnGOPrefab, e.mouseWorldPosition, Quaternion.identity);
            // pawnGO.SetLabel(HandleInput(e.mouseWorldPosition));


            GridPosition? gridPosition = GridManager.Instance.GetGridPosition(e.mouseWorldPosition);
            if (gridPosition is not null)
            {
                // Debug.Log("Test " + board.GetPawn(gridPosition.Value));
                HandlePawnMovement(gridPosition.Value);
            }
        }

        private void InputManager_HandleMouseUp(object sender, MouseEventArgs e)
        {
            // Debug.Log($"Mouse Up at: {e.mouseWorldPosition}");
        }

        private string HandleInput(Vector2 worldPosition)
        {

            float cellSize = 2.0f;
            int gridX = Mathf.FloorToInt(worldPosition.x / cellSize);
            int gridY = Mathf.FloorToInt(worldPosition.y / cellSize);


            var test = GridManager.Instance.GetGridPosition(worldPosition);
            //Debug.Log($"Cella selezionata: ({test.x}, {test.y})");

            GridPosition? gridPosition = GridManager.Instance.GetGridPosition(worldPosition);

            if (gridPosition.HasValue)
            {
                GridObject gridObject = GridManager.Instance.GetGridObject(gridPosition.Value);
                Debug.Log(gridObject);
                return gridObject.GetGridPosition().ToString();
            }

            return "null";
        }

        private bool isPawnSelected = false;
        private Pawn pawnToMove;

        private void HandlePawnMovement(GridPosition gridPosition)
        {

            if (!isPawnSelected)
            {
                Pawn pawn = board.GetPawn(gridPosition);
                if (pawn is not null)
                {
                    Debug.Log("Pawn selected in position: " + gridPosition.ToString());
                    isPawnSelected = true;
                    pawnToMove = pawn;
                }
                else
                {
                    Debug.Log("No pawn in position: " + gridPosition.ToString());
                }
            }
            else
            {
                Pawn pawn = board.GetPawn(gridPosition);
                if (pawn is not null)
                {
                    Debug.Log("Position: " + gridPosition.ToString() + " is not empty.");
                }
                else
                {
                    Debug.Log("Position: " + gridPosition.ToString() + " is empty.");
                    Debug.Log("Pawn placed in: " + gridPosition.ToString());
                    board.MakeAction(pawnToMove, gridPosition);
                    pawnToMove = null;
                    isPawnSelected = false;
                    OnMoveCompleted?.Invoke(this, EventArgs.Empty);
                }
            }


        }

    }

}


// public enum PlayerType { Player1, Player2 }

// public static class InitialPositions
// {
//     public static readonly Dictionary<PlayerType, List<GridPosition>> Positions = new()
//     {
//         { PlayerType.Player1, new List<GridPosition> { new(3, 0), new(3, 1), new(3, 2), new(3, 3), new(5, 0), new(5, 1), new(5, 2), new(5, 3) } },
//         { PlayerType.Player2, new List<GridPosition> { new(3, 5), new(3, 6), new(3, 7), new(3, 8), new(5, 5), new(5, 6), new(5, 7), new(5, 8) } }
//     };

//     public static List<GridPosition> GetPositions(PlayerType player)
//     {
//         return Positions.ContainsKey(player) ? Positions[player] : new List<GridPosition>();
//     }
// }
