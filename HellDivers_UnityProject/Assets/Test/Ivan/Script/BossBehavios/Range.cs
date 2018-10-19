using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour {

    public float m_Radius = 10;
    

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, m_Radius);
    }

}
