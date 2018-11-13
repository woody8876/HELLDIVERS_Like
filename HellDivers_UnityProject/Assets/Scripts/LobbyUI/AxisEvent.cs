using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AxisEvent : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (timer < 0)
        {
            currentAxis = new AxisEventData(EventSystem.current);
            currentButton = EventSystem.current.currentSelectedGameObject;

            if (Input.GetAxis("1PVertical") > 0 || Input.GetAxis("2PVertical") > 0) // move up
            {
                Debug.Log("Up");
                currentAxis.moveDir = MoveDirection.Up;
                ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
                timer = timeBetweenInputs;
            }
            else if (Input.GetAxis("1PVertical") < 0 || Input.GetAxis("2PVertical") < 0) // move down
            {
                Debug.Log("Down");
                currentAxis.moveDir = MoveDirection.Down;
                ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
                timer = timeBetweenInputs;
            }
            else if (Input.GetAxis("1PVertical") > 0 || Input.GetAxis("2PVertical") > 0) // move right
            {
                Debug.Log("Right");
                currentAxis.moveDir = MoveDirection.Right;
                ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
                timer = timeBetweenInputs;
            }
            else if (Input.GetAxis("1PHorizontal") < 0 || Input.GetAxis("2PVertical") < 0) // move left
            {
                Debug.Log("Left");
                currentAxis.moveDir = MoveDirection.Left;
                ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
                timer = timeBetweenInputs;
            }
        }
        timer -= Time.fixedDeltaTime;
    }

    private AxisEventData currentAxis;
    private GameObject currentButton;
    private float timer;
    private float timeBetweenInputs = 0.15f;
}
