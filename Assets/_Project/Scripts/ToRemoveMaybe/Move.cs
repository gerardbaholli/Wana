using UnityEngine;

public class Move : MonoBehaviour
{

    private readonly GridPosition fromGridPosition;
    private readonly GridPosition toGridPosition;

    public Move(GridPosition fromGridPosition, GridPosition toGridPosition) {
        this.fromGridPosition = fromGridPosition;
        this.toGridPosition = toGridPosition;
    }

}
