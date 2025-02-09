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

        private const int TOTAL_PAWNS_FOR_PLAYER = 8;
        private const int UNIT_PER_GRID = 2;

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

            for (int i = 0; i < TOTAL_PAWNS_FOR_PLAYER; i++)
            {
                Pawn pawnX = new Pawn(PawnType.X);
                Pawn pawnO = new Pawn(PawnType.O);

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
                    Vector3 worldPosition = new Vector3(i * UNIT_PER_GRID, j * UNIT_PER_GRID, 1);
                    GameObject gameObject = Instantiate(gameObjectDebug, worldPosition, Quaternion.identity, parentGridDebug.transform);
                    gameObject.GetComponent<GridDebugObject>().SetTextMeshPro((worldPosition.x / UNIT_PER_GRID).ToString() + " " + (worldPosition.y / UNIT_PER_GRID).ToString());
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

            GameObject parentPawn = new GameObject("PawnContainer");

            foreach (Pawn pawn in player1Pawns)
            {
                Vector3 worldPosition = new Vector3(pawn.GetGridPosition().x * UNIT_PER_GRID, pawn.GetGridPosition().y * UNIT_PER_GRID, 0);
                GameObject gameObject = Instantiate(player1PawnGO, worldPosition, Quaternion.identity, parentPawn.transform);
            }

            foreach (Pawn pawn in player2Pawns)
            {
                Vector3 worldPosition = new Vector3(pawn.GetGridPosition().x * UNIT_PER_GRID, pawn.GetGridPosition().y * UNIT_PER_GRID, 0);
                GameObject gameObject = Instantiate(player2PawnGO, worldPosition, Quaternion.identity, parentPawn.transform);
            }
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
            Debug.Log($"Mouse Down at: {e.mouseWorldPosition}");
            Instantiate(pawnGOPrefab, e.mouseWorldPosition, Quaternion.identity);
        }

        private void InputManager_HandleMouseUp(object sender, MouseEventArgs e)
        {
            Debug.Log($"Mouse Up at: {e.mouseWorldPosition}");
        }

    }

}
