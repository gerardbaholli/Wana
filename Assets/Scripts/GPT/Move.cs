namespace Wana
{
    public class Move
    {
        public GridPosition StartPosition { get; private set; }
        public GridPosition EndPosition { get; private set; }

        public Move(GridPosition start, GridPosition end)
        {
            StartPosition = start;
            EndPosition = end;
        }

        /// <summary>
        /// Verifica se una mossa è valida considerando che i quadranti raggiungibili
        /// si trovano solo nel centro della board 9x9.
        /// </summary>
        public bool IsValid()
        {
            // La posizione di partenza e di arrivo devono essere nel range della board
            if (!IsPositionInRange(StartPosition) || !IsPositionInRange(EndPosition))
            {
                return false;
            }

            // La posizione di arrivo deve trovarsi nei quadranti centrali (escludendo angoli)
            return IsInReachableQuadrant(EndPosition);
        }

        private bool IsPositionInRange(GridPosition position)
        {
            return position.x >= 0 && position.x < 9 && position.y >= 0 && position.y < 9;
        }

        private bool IsInReachableQuadrant(GridPosition position)
        {
            // Rimuove gli angoli esterni della 9x9 dividendo la board in quadranti 3x3.
            return (position.x >= 3 && position.x <= 5) && (position.y >= 3 && position.y <= 5);
        }
    }
}
