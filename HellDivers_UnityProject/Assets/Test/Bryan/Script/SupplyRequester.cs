using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class SupplyRequester : MonoBehaviour
{
    [SerializeField] private GameObject m_Display;
    [SerializeField] private GameObject m_Item;
    [SerializeField] private float m_ActTime;

    // Use this for initialization
    private void Start()
    {
        m_Rigidbody = this.GetComponent<Rigidbody>();
        m_Rigidbody.isKinematic = true;

        if (m_Display != null)
        {
            m_Display = Instantiate(m_Display, this.transform);
        }
    }

    public void SetData(SupplyRequesterData src)
    {
    }

    public void ThrowOut(Vector3 direction, float size)
    {
        Vector3 force = direction.normalized * size;
        m_Rigidbody.AddForce(force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Terrain")
        {
            m_Rigidbody.isKinematic = true;
            this.transform.up = Vector3.up;
            StartCoroutine(StartActivationProcess(m_ActTime));
        }
    }

    private IEnumerator StartActivationProcess(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        this.gameObject.SetActive(false);
    }

    private Rigidbody m_Rigidbody;
}