
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

        public Pawn (PawnType pawnType) {
            this.pawnType = pawnType;
        }

        public PawnType GetPawnType() {
            return pawnType;
        }

        public GridPosition GetGridPosition() {
            return gridPosition;
        }

        public void SetGridPosition(GridPosition gridPosition) {
            this.gridPosition = gridPosition;
        }

    }

}
