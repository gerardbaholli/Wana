public class GridObject
{

    private GridPosition gridPosition;
    private Ball ball;

    public GridObject(GridPosition gridPosition)
    {

        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public void AddBall(Ball ball)
    {
        this.ball = ball;
    }

    public void RemoveBall()
    {
        this.ball = null;
    }

    public bool HasAnyPawn()
    {
        return ball != null;
    }

    public Ball GetBall()
    {
        return ball;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

}