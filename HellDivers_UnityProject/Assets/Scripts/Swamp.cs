using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swamp : MonoBehaviour {

    public Transform SpawnPoint;
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(ReBorn(other));
    }

    IEnumerator ReBorn(Collider other)
    {
        yield return new WaitForSeconds(1);
        other.transform.position = SpawnPoint.position;
    }

}
