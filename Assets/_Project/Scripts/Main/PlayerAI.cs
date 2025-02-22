#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wana
{
    public class PlayerAI
    {

        private readonly Board board;
        private readonly System.Random random = new();

        public PlayerAI(Board board)
        {
            this.board = board;
        }

        public BoardAction MakeRandomMovement()
        {
            List<Pawn> pawns = new List<Pawn>();
            List<GridPosition> freeGridPositions = new List<GridPosition>();

            Pawn?[,] pawnsMatrix = board.pawnsMatrix;

            for (int i = 0; i < Board.BOARD_SIZE; i++)
            {
                for (int j = 0; j < Board.BOARD_SIZE; j++)
                {
                    if (pawnsMatrix[i, j] is Pawn pawn && pawn.GetPawnType() == PawnType.O)
                    {
                        pawns.Add(pawn);
                    }
                    if (pawnsMatrix[i, j] is null)
                    {
                        freeGridPositions.Add(new GridPosition(i, j));
                    }
                }
            }

            if (pawns.Count == 0 || freeGridPositions.Count == 0)
            {
                throw new InvalidOperationException("Nessuna mossa valida disponibile.");
            }

            Pawn pawnToMove = pawns[random.Next(pawns.Count)];
            GridPosition targetPosition = freeGridPositions[random.Next(freeGridPositions.Count)];

            return new BoardAction(board, pawnToMove, targetPosition);
        }

        public void TempMakeRandomMovement()
        {
            List<Pawn> pawns = new List<Pawn>();
            List<GridPosition> freeGridPositions = new List<GridPosition>();

            Pawn?[,] pawnsMatrix = board.pawnsMatrix;

            for (int i = 0; i < Board.BOARD_SIZE; i++)
            {
                for (int j = 0; j < Board.BOARD_SIZE; j++)
                {
                    GridPosition gridPosition = new GridPosition(i, j);
                    if (pawnsMatrix[i, j] is Pawn pawn && pawn.GetPawnType() == PawnType.O)
                    {
                        pawns.Add(pawn);
                    }
                    if (pawnsMatrix[i, j] is null && RuleChecker.CheckBoardActionValidity(board, pawnsMatrix[i, j], gridPosition))
                    {
                        freeGridPositions.Add(gridPosition);
                    }
                }
            }

            if (pawns.Count == 0 || freeGridPositions.Count == 0)
            {
                throw new InvalidOperationException("Nessuna mossa valida disponibile.");
            }

            Pawn pawnToMove = pawns[random.Next(pawns.Count)];
            GridPosition targetPosition = freeGridPositions[random.Next(freeGridPositions.Count)];

            board.MakeAction(pawnToMove, targetPosition);
        }


    }

}