using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{

    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private List<Ball> ballList;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        ballList = new List<Ball>();
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
        ballList.Add(ball);
    }

    //public void RemoveBall(Ball ball)
    //{
    //    ballList.Remove(ball);
    //}

    //public List<Unit> GetUnitList()
    //{
    //    return unitList;
    //}

    //public bool HasAnyUnit()
    //{
    //    return unitList.Count > 0;
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