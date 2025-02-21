using UnityEngine;

namespace Wana
{

    public enum PawnType {
        X,
        O,
    }

    public class Pawn
    {

        private PawnType pawnType;
        private GridPosition gridPosition;
        private PawnGO pawnGO;

        public Pawn (PawnType pawnType, PawnGO pawnGO) {
            this.pawnType = pawnType;
            this.pawnGO = pawnGO;
        }

        public PawnType GetPawnType() {
            return pawnType;
        }

        public GridPosition GetGridPosition() {
            return gridPosition;
        }

        public PawnGO GetPawnGO() {
            return pawnGO;
        }

        public void SetGridPosition(GridPosition gridPosition) {
            this.gridPosition = gridPosition;
            UpdatePawnGOPosition();
        }

        private void UpdatePawnGOPosition() {
            pawnGO.transform.position = new Vector3(gridPosition.x * Board.CELL_SIZE, gridPosition.y * Board.CELL_SIZE, 0f);
        }

    }

}
