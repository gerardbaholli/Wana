using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridValidObject : MonoBehaviour
{
    private object gridObject;

    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }

}
