

namespace Wana
{

    public class GridObject
    {

        private GridSystem gridSystem;
        private GridPosition gridPosition;
        private Pawn pawn;

        public GridObject(GridSystem gridSystem, GridPosition gridPosition)
        {
            this.gridSystem = gridSystem;
            this.gridPosition = gridPosition;
        }

        public override string ToString()
        {
            return gridPosition.ToString();
        }

        public void AddPawn(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public void RemovePawn()
        {
            this.pawn = null;
        }

        public bool HasAnyPawn()
        {
            return pawn != null;
        }

        public Pawn GetPawn()
        {
            return pawn;
        }

        public GridPosition GetGridPosition()
        {
            return gridPosition;
        }

    }

}