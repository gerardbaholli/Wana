using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] TextMeshPro textMeshPro;

    private object gridObject;
    private string text;

    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }

    public virtual void SetTextMeshPro(string text)
    {
        this.text = text;
    }

    protected virtual void Update()
    {
        // textMeshPro.text = gridObject.ToString();
        textMeshPro.text = text;
    }

}
