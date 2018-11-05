using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTest : MonoBehaviour {

 //   public float m_Radius;
 //   public Transform m_Center;
 //   float m_fLenght;
 //   Vector3 m_vForward;
 //   Vector3 m_vPos;
	//// Use this for initialization
	//void Start () {
		
	//}
	//// Update is called once per frame
	//void Update () {
 //       m_fLenght = (CrossPoint(transform) - transform.position).magnitude;

 //       DrawTools.DrawRectangleSolid(transform, m_vPos, m_fLenght, 2);
 //       m_vForward = transform.forward;
	//}
 //   public float Interscetion(Transform t)
 //   {
 //       float PowRadius = m_Radius * m_Radius;
 //       float disX = t.position.x - m_Center.position.x;
 //       float disZ = t.position.z - m_Center.position.z;
 //       //float a = t.forward.x * t.forward.x + t.forward.z * t.forward.z;
 //       float b = disX * t.forward.x + disZ * t.forward.z;
 //       float c = (disX * disX + disZ * disZ) - PowRadius;
 //       float length = (-b + Mathf.Sqrt(b * b - 4 * c)) * .5f;
 //       return length;
 //   }
 //   public Vector3 CrossPoint(Transform t)
 //   {
 //       float PowRadius = m_Radius * m_Radius;
 //       float m = t.forward.z / t.forward.x;
 //       float p = t.position.z - m * t.position.x;
 //       float a = 1 + m * m;
 //       float b = m * (p - m_Center.position.z) - m_Center.position.x;
 //       float c = m_Center.position.x * m_Center.position.x + (p - m_Center.position.z) * (p - m_Center.position.z) - PowRadius;
 //       float x = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / 2 * a;
 //       float z = m * x + p;
 //       Vector3 pos = new Vector3(x, t.position.y, z);
 //       return pos;
 //   }


}
