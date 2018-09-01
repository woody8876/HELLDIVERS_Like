using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class SupplyRequester : MonoBehaviour
{
    [SerializeField] private GameObject m_Display;
    [SerializeField] private SupplyRequesterData m_Data;
    private Rigidbody m_Rigidbody;

    public void Init(SupplyRequesterData data)
    {
        m_Data = data;
    }

    // Use this for initialization
    private void Start()
    {
        m_Rigidbody = this.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Terrain")
        {
            m_Rigidbody.isKinematic = true;
            this.transform.up = Vector3.up;
            StartCoroutine(StartActivate(m_Data.AcivateTime));
        }
    }

    private IEnumerator StartActivate(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Instantiate(m_Data.Item, this.transform.position, Quaternion.identity, null);
        DestroyImmediate(this.gameObject);
    }

    // Set with default value
    // Only called in editor mode
    private void Reset()
    {
        m_Data = null;
    }
}

public enum RequestCode
{
    Up,
    Down,
    Left,
    Right
}