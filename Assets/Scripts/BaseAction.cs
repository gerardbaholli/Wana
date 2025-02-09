using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{

    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;


    protected Player player;
    protected bool isActive;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        player = GetComponent<Player>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointsCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;

        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();

        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public Player GetPlayer()
    {
        return player;
    }

    public OpponentAIAction GetBestOpponentAIAction()
    {
        List<OpponentAIAction> opponentAIActionList = new List<OpponentAIAction>();

        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

        foreach (GridPosition gridPosition in validActionGridPositionList)
        {
            OpponentAIAction enemyAIAction = GetOpponentAIAction(gridPosition);
            opponentAIActionList.Add(enemyAIAction);
        }

        if (opponentAIActionList.Count > 0)
        {
            opponentAIActionList.Sort((OpponentAIAction a, OpponentAIAction b) => b.actionValue - a.actionValue);
            return opponentAIActionList[0];
        } else
        {
            // No possible Enemy AI Actions
            return null;
        }

    }

    public abstract OpponentAIAction GetOpponentAIAction(GridPosition gridPosition);

}
