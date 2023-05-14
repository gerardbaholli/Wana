using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionValidator : MonoBehaviour
{
    public static PositionValidator Instance { get; private set; }

    [Header("Debug Options")]
    [SerializeField] bool debugLogActive = false;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public bool IsValidPosition(GridPosition gridPosition)
    {
        if ((0 <= gridPosition.x && gridPosition.x <= 2) && (0 <= gridPosition.y && gridPosition.y <= 2))
        {
            if (debugLogActive)
                Debug.Log("Invalid position: Q1");
            return false;
        }

        if ((6 <= gridPosition.x && gridPosition.x <= 8) && (0 <= gridPosition.y && gridPosition.y <= 2))
        {
            if (debugLogActive)
                Debug.Log("Invalid position: Q3");
            return false;
        }

        if ((0 <= gridPosition.x && gridPosition.x <= 2) && (6 <= gridPosition.y && gridPosition.y <= 8))
        {
            if (debugLogActive)
                Debug.Log("Invalid position: Q7");
            return false;
        }

        if ((6 <= gridPosition.x && gridPosition.x <= 8) && (6 <= gridPosition.y && gridPosition.y <= 8))
        {
            if (debugLogActive)
                Debug.Log("Invalid position: Q9");
            return false;
        }

        if (debugLogActive)
            Debug.Log("Valid position: " + gridPosition.x + " " + gridPosition.y);
        return true;
    }


}
