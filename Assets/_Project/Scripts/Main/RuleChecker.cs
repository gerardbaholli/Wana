

namespace Wana
{
    public class RuleChecker
    {

        public static bool CheckBoardActionValidity(Board board, Pawn pawn, GridPosition positionToMoveOn)
        {
            return IsValidGridPosition(positionToMoveOn);
        }

        private static bool IsValidGridPosition(GridPosition gridPosition)
        {
            if ((0 <= gridPosition.x && gridPosition.x <= 2) && (0 <= gridPosition.y && gridPosition.y <= 2))
            {
                //Debug.Log("Invalid position: Q1");
                return false;
            }

            if ((6 <= gridPosition.x && gridPosition.x <= 8) && (0 <= gridPosition.y && gridPosition.y <= 2))
            {
                //Debug.Log("Invalid position: Q3");
                return false;
            }

            if ((0 <= gridPosition.x && gridPosition.x <= 2) && (6 <= gridPosition.y && gridPosition.y <= 8))
            {
                //Debug.Log("Invalid position: Q7");
                return false;
            }

            if ((6 <= gridPosition.x && gridPosition.x <= 8) && (6 <= gridPosition.y && gridPosition.y <= 8))
            {
                //Debug.Log("Invalid position: Q9");
                return false;
            }

            //Debug.Log("Valid position: [" + gridPosition.x + ", " + gridPosition.y + "]");
            return true;
        }

    }

}

