using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    public AIData data = new AIData();
    public Transform Target;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        data.m_vTarget = Target.transform.position;
		if (!SteeringBehaviours.CollisionAvoid(data))
        {
            SteeringBehaviours.Seek(data);
        }
        SteeringBehaviours.Move(data);
	}
}
