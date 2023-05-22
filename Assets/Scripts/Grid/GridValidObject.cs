using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridValidObject : MonoBehaviour
{

    public static event EventHandler OnValidPositionSelected;

    private object gridObject;


    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }

    private void OnMouseDown()
    {
        OnValidPositionSelected?.Invoke(this, EventArgs.Empty);
    }

}
