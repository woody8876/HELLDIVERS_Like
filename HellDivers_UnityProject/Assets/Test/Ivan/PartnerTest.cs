using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerTest : MonoBehaviour {

    public Collider[] Colliders;
    public AIData data;
    private EnemyAI enemyAI;
    // Use this for initialization
    void Start () {
        enemyAI = GetComponent<EnemyAI>();
        data = new AIData();
	}
	
	// Update is called once per frame
	void Update () {
        Colliders = Physics.OverlapSphere(enemyAI.data.m_Go.transform.position, enemyAI.data.m_fRadius * 2, 1 << LayerMask.NameToLayer("Enemies"));
    }

    //private void OnDrawGizmos()
    //{
    //  //  Gizmos.DrawWireSphere(enemyAI.data.m_Go.transform.position, enemyAI.data.m_fRadius * 2);
    //    Gizmos.color = Color.red;
    //    //Gizmos.DrawWireSphere(enemyAI.data.m_Go.transform.position, enemyAI.data.m_fRadius * 5);
    //    Gizmos.DrawRay(enemyAI.data.m_Go.transform.position, enemyAI.groupVec * 10);
    //}
}
