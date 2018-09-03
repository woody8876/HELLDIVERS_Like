using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class SupplyRequester : MonoBehaviour
{
    public SupplyRequesterData Data { get { return m_Data; } }

    [SerializeField] private GameObject m_Display;
    [SerializeField] private SupplyRequesterData m_Data;
    private Rigidbody m_Rigidbody;

    public void Init(SupplyRequesterData data)
    {
        data.CopyTo(ref m_Data);
    }

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

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 vDir = this.transform.forward;
            vDir.y += 1;
            m_Rigidbody.isKinematic = false;
            Throw(vDir, 500.0f);
        }
    }

    public void Throw(Vector3 direction, float size)
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
            StartCoroutine(StartActivate(m_Data.AcivateTime));
        }
    }

    private IEnumerator StartActivate(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Instantiate(m_Data.Item, this.transform.position, Quaternion.identity, null);
        this.gameObject.SetActive(false);
    }

    // Set with default value
    // Only called in editor mode
    private void Reset()
    {
        m_Data = null;
    }
}