#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wana
{
    public class Board : ICloneable
    {

        public const int BOARD_SIZE = 9;
        public const int CELL_SIZE = 2;

        public Pawn?[,] pawnsMatrix;

        public Board(Pawn?[,] pawnsMatrix)
        {
            this.pawnsMatrix = new Pawn?[BOARD_SIZE, BOARD_SIZE];

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    this.pawnsMatrix[i, j] = pawnsMatrix[i, j];
                }
            }

        }

        public void MakeAction(Pawn pawn, GridPosition gridPositionToMoveOn)
        {
            // Board clonedBoard = (Board) this.Clone();
            // BoardAction boardAction = new BoardAction(clonedBoard, pawn, gridPositionToMoveOn);


            GridPosition pawnToMoveGridPosition = pawn.GetGridPosition();
            pawnsMatrix[pawnToMoveGridPosition.x, pawnToMoveGridPosition.y] = null;
            pawnsMatrix[gridPositionToMoveOn.x, gridPositionToMoveOn.y] = pawn;
            pawn.SetGridPosition(gridPositionToMoveOn);

        }

        public Pawn? GetPawn(GridPosition? pawnPosition)
        {
            if (pawnPosition is null)
            {
                return null;
            }
            GridPosition pos = pawnPosition.Value;
            return pawnsMatrix[pos.x, pos.y];
        }

        public void PrintBoard()
        {

            string printedBoard = "";

            for (int y = BOARD_SIZE - 1; y >= 0; y--)
            {
                for (int x = 0; x < BOARD_SIZE; x++)
                {
                    if (pawnsMatrix[x, y] is not null)
                    {
                        if (pawnsMatrix[x, y]?.GetPawnType() == PawnType.O)
                        {
                            printedBoard += "0";
                        }
                        else
                        {
                            printedBoard += "X";
                        }
                    }
                    else
                    {
                        printedBoard += " - ";
                    }
                }
                printedBoard += "\n";
            }

            Debug.Log(printedBoard);

        }

        public int EvaluateBoardForPlayer(Player player)
        {

            Debug.Log("Evaluating board for " + player.GetName());
            if (player.GetId() == 0)
            {
                List<Pawn> pawnList = player.GetPawns();

                int? count = GetPawnNeighborCount(player.GetPawn(new GridPosition(4, 3)));
                Debug.Log(count);
            }
            return 0;
        }

        public int? GetPawnNeighborCount(Pawn? pawn)
        {
            if (pawn is null)
            {
                return null;
            }

            GridPosition pos = pawn.GetGridPosition();
            int[] directionsX = { -1, 1, 0, 0 };
            int[] directionsY = { 0, 0, -1, 1 };
            int count = 0;

            for (int i = 0; i < directionsX.Length; i++)
            {
                int neighborX = (pos.x + directionsX[i] + BOARD_SIZE) % BOARD_SIZE;
                int neighborY = (pos.y + directionsY[i] + BOARD_SIZE) % BOARD_SIZE;

                if (pawnsMatrix[neighborX, neighborY] != null)
                {
                    count++;
                }
            }

            return count;
        }

        public object Clone()
        {
            return new Board((Pawn?[,]) pawnsMatrix.Clone());
        }
    }
}
