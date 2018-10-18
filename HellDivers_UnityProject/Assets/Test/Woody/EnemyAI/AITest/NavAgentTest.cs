using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentTest : MonoBehaviour {

    NavMeshAgent nma;
    Vector3 vTarget;
    bool bMove = false;
	// Use this for initialization
	void Start () {
        nma = GetComponent<NavMeshAgent>();

    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rh;
            if(Physics.Raycast(r, out rh, 1000.0f))
            {
                if(rh.collider.gameObject != this.gameObject)
                {
                    bMove = true;
                    vTarget = rh.point;
                }
            }
        }
        nma.SetDestination(vTarget);
        //if(bMove)
        //{
        //    nma.SetDestination(vTarget);
        //    OffMeshLinkData omld =  nma.currentOffMeshLinkData;
        //    if(omld.activated)
        //    {
        //        this.transform.position = omld.endPos;
        //        nma.CompleteOffMeshLink();
        //    }
        //}
    }
}
