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

    //public override string ToString()
    //{
    //    string unitString = "";
    //    foreach (Unit unit in unitList)
    //    {
    //        unitString += unit + "\n";
    //    }
    //    return gridPosition.ToString() + "\n" + unitString;
    //}

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

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    //public List<Unit> GetUnitList()
    //{
    //    return unitList;
    //}

    //public Unit GetUnit()
    //{
    //    if (HasAnyUnit())
    //    {
    //        return unitList[0];
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

}