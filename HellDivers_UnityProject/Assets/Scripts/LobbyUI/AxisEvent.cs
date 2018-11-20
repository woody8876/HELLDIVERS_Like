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
        if (!PlayerManager.Instance.Players.ContainsKey(1) || !PlayerManager.Instance.Players[1].controllerSetting) { return; }
        if (timer < 0)
        {
            InputSetting(PlayerManager.Instance.Players[1].controllerSetting);
        }
        timer -= Time.fixedDeltaTime;
    }

    private void InputSetting(ControllerSetting m_controller)
    {
        currentAxis = new AxisEventData(EventSystem.current);
        currentButton = EventSystem.current.currentSelectedGameObject;

        if (Input.GetAxis(m_controller.Vertical) > 0)
        {
            currentAxis.moveDir = MoveDirection.Up;
            ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
            timer = timeBetweenInputs;
        }
        else if (Input.GetAxis(m_controller.Vertical) < 0)
        {
            currentAxis.moveDir = MoveDirection.Down;
            ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
            timer = timeBetweenInputs;
        }
        else if (Input.GetAxis(m_controller.Horizontal) > 0)
        {
            currentAxis.moveDir = MoveDirection.Right;
            ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
            timer = timeBetweenInputs;
        }
        else if (Input.GetAxis(m_controller.Horizontal) < 0)
        {
            currentAxis.moveDir = MoveDirection.Left;
            ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
            timer = timeBetweenInputs;
        }

    }
    private AxisEventData currentAxis;
    private GameObject currentButton;
    private float timer;
    private float timeBetweenInputs = 0.15f;
}
