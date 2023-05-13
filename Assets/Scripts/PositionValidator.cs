using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionValidator : MonoBehaviour
{

    [SerializeField] bool debugOption = false;
    [SerializeField] Ball ball;

    private void Start()
    {
        IsValidPosition(ball.transform.position);
    }


    public bool IsValidPosition(Vector2 pos)
    {
        //Debug.Log(pos.x + " " + pos.y);

        if ((0 <= pos.x && pos.x <= 2) && (0 <= pos.y && pos.y <= 2))
        {
            if (debugOption)
                Debug.Log("Invalid position: Q1");
            return false;
        }

        if ((6 <= pos.x && pos.x <= 8) && (0 <= pos.y && pos.y <= 2))
        {
            if (debugOption)
                Debug.Log("Invalid position: Q3");
            return false;
        }

        if ((0 <= pos.x && pos.x <= 2) && (6 <= pos.y && pos.y <= 8))
        {
            if (debugOption)
                Debug.Log("Invalid position: Q7");
            return false;
        }

        if ((6 <= pos.x && pos.x <= 8) && (6 <= pos.y && pos.y <= 8))
        {
            if (debugOption)
                Debug.Log("Invalid position: Q9");
            return false;
        }

        if (debugOption)
            Debug.Log("Valid position: " + pos.x + " " + pos.y);
        return true;
    }


}
