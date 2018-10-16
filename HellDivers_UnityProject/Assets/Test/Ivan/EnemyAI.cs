using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public AIData data = new AIData();

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        data.m_vTarget = data.m_TargetObject.transform.position;

        transform.position += SteeringBehaviours.GroupBehavior(data, 2 , true) * data.m_fRot;
        transform.position += SteeringBehaviours.GroupBehavior(data, 5 , false) * data.m_fRot;
        //SteeringBehaviours.Seek(data);
        //SteeringBehaviours.Move(data);
        if (SteeringBehaviours.CollisionAvoided(data) == false)
        {
       //     SteeringBehaviours.EnemiesAvoided(data);
            SteeringBehaviours.Seek(data);
        }
        SteeringBehaviours.Move(data);
    }

    //private void OnDrawGizmos()
    //{
    //    if (data != null)
    //    {
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawWireSphere(this.transform.position, data.m_fProbeLength);
    //        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * this.data.m_fProbeLength);
    //        Gizmos.color = Color.yellow;
    //        Vector3 vLeftStart = this.transform.position - this.transform.right * data.m_fRadius;
    //        Vector3 vLeftEnd = vLeftStart + this.transform.forward * data.m_fProbeLength;
    //        Gizmos.DrawLine(vLeftStart, vLeftEnd);
    //        Vector3 vRightStart = this.transform.position + this.transform.right * data.m_fRadius;
    //        Vector3 vRightEnd = vRightStart + this.transform.forward * data.m_fProbeLength;
    //        Gizmos.DrawLine(vRightStart, vRightEnd);
    //        Gizmos.DrawLine(vLeftEnd, vRightEnd);

    //    }
    //}

}
