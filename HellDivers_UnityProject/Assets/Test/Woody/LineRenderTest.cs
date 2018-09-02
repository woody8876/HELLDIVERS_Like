using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderTest : MonoBehaviour {
    
    private GameObject LineRenderGameObject;
    private LineRenderer lineRenderer;
    private int lineLength = 2;
    // Use this for initialization
    void Start () {
        LineRenderGameObject = GameObject.Find("Line");
        lineRenderer = (LineRenderer)LineRenderGameObject.GetComponent("LineRenderer");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(1))
        {
            lineRenderer.enabled = true;
            lineRenderer.useWorldSpace = false;
            lineRenderer.SetWidth(0.1f, 0.1f);
            lineRenderer.SetVertexCount(lineLength);
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rh;
            if (Physics.Raycast(ray, out rh))
            {
                lineRenderer.SetPosition(0, Vector3.forward * 100f);//方向*距离，典型的Location坐标系用法
            }
        }else if (Input.GetMouseButtonUp(1))
        {
            lineRenderer.enabled = false;
        }

    }
}
