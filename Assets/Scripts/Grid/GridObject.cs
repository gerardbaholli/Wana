using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{

    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private Ball ball;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
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

    public bool HasAnyBall()
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
