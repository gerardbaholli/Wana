using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    
	[SerializeField] float anchorMoveDuration = 0.5f;
    
	private Vector3 mOffset;
	private float mZCoord;
    
	private void Start()
    {
	    //this.transform.position = AnchorPosition(this.transform.position);
	    transform.DOMove(AnchorPosition(this.transform.position), anchorMoveDuration);
    }
    
	private Vector2 AnchorPosition(Vector2 pos)
	{
		int newX;
		int newY;
		int x = (int) pos.x;
		int y = (int) pos.y;
		
		if (x + 0.5 >= pos.x)
			newX = x;
		else
			newX = x + 1;
			
		if (y + 0.5 >= pos.y)
			newY = y;
		else
			newY = y + 1;
			
		Debug.Log("[" + newX + ", " + newY + "]");
		
		return new Vector2(newX, newY);
	}


	public Vector2 GetPosition()
	{
		return AnchorPosition(this.transform.position);
    }

	
	private void OnMouseDown()
	{
		mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

		// Store offset = gameobject world pos - mouse world pos
		mOffset = gameObject.transform.position - GetMouseAsWorldPoint();

        
	}

	private Vector3 GetMouseAsWorldPoint()
	{
		// Pixel coordinates of mouse (x,y)
		Vector3 mousePoint = Input.mousePosition;

		// z coordinate of game object on screen
		mousePoint.z = mZCoord;

		// Convert it to world points
		return Camera.main.ScreenToWorldPoint(mousePoint);
	}

	private void OnMouseDrag()
	{
		transform.position = GetMouseAsWorldPoint() + mOffset;     
	}

	private void OnMouseUp()
	{
        transform.DOMove(AnchorPosition(this.transform.position), anchorMoveDuration);
    }
  
}
