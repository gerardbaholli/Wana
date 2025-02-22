#nullable enable
using UnityEngine;

namespace Wana
{
    public class BoardAction
    {
        public Board Board { get; private set; }
        public Pawn PawnToMove { get; private set; }
        public GridPosition GridPositionToMoveOn { get; private set; }

        public BoardAction(Board board, Pawn pawnToMove, GridPosition gridPositionToMoveOn)
        {
            Board = board;
            PawnToMove = pawnToMove;
            GridPositionToMoveOn = gridPositionToMoveOn;
        }

        public bool Execute()
        {
            // if (!IsValid())
            // {
            //     Debug.Log("Invalid move");
            //     return false;
            // }

            // Pawn? pawn = Board.pawnsMatrix[From.x, From.y];
            // Board.pawnsMatrix[From.x, From.y] = null;
            // Board.pawnsMatrix[To.x, To.y] = pawn;
            
            // Debug.Log($"Moved pawn from ({From.x}, {From.y}) to ({To.x}, {To.y})");
            return true;
        }

        public bool IsValid()
        {
            // if (Board.pawnsMatrix[From.x, From.y] == null)
            // {
            //     return false;
            // }
            // if (Board.pawnsMatrix[To.x, To.y] != null)
            // {
            //     return false;
            // }
            return true;
        }
        
    }

}
