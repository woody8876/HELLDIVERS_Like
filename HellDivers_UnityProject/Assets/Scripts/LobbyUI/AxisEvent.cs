using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AxisEvent : MonoBehaviour {

	// Update is called once per frame
	void FixedUpdate () {
        if (!PlayerManager.Instance.Players.ContainsKey(1) && m_controller == null) { return; }
        if (PlayerManager.Instance.Players.ContainsKey(1)) { m_controller = PlayerManager.Instance.Players[1].controllerSetting; }
        if (timer < 0)
        {
            InputSetting(m_controller);
        }
        timer -= Time.fixedDeltaTime;
    }

    private void InputSetting(ControllerSetting controller)
    {
        currentAxis = new AxisEventData(EventSystem.current);
        currentButton = EventSystem.current.currentSelectedGameObject;

        if (Input.GetAxis(controller.Vertical) > 0)
        {
            currentAxis.moveDir = MoveDirection.Up;
            ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
            timer = timeBetweenInputs;
        }
        else if (Input.GetAxis(controller.Vertical) < 0)
        {
            currentAxis.moveDir = MoveDirection.Down;
            ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
            timer = timeBetweenInputs;
        }
        else if (Input.GetAxis(controller.Horizontal) > 0)
        {
            currentAxis.moveDir = MoveDirection.Right;
            ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
            timer = timeBetweenInputs;
        }
        else if (Input.GetAxis(controller.Horizontal) < 0)
        {
            currentAxis.moveDir = MoveDirection.Left;
            ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
            timer = timeBetweenInputs;
        }

    }

    public void SetController(ControllerSetting controller) { m_controller = controller; }

    private ControllerSetting m_controller;
    private AxisEventData currentAxis;
    private GameObject currentButton;
    private float timer;
    private float timeBetweenInputs = 0.15f;
}
