using System.Collections.Generic;

namespace Wana
{
    public class Player
    {
        private int id;
        private string name;
        private List<Pawn> pawns;

        public Player(int id, string name, List<Pawn> pawns)
        {
            this.id = id;
            this.name = name;
            this.pawns = pawns;
        }

        public int GetId()
        {
            return id;
        }

        public string GetName()
        {
            return name;
        }

        public List<Pawn> GetPawns()
        {
            return pawns;
        }

        public Pawn GetPawn(GridPosition gridPosition)
        {
            foreach (Pawn pawn in pawns)
            {
                if (gridPosition == pawn.GetGridPosition())
                {
                    return pawn;
                }
            }
            return null;
        }

    }

}
