using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stratagem : MonoBehaviour
{
    private StratagemInfo m_Info;
    private GameObject m_Go;
    private GameObject m_Item;
    private Rigidbody m_Rigidbody;
    private Animator m_Anima;
    private Transform m_LaunchPos;

    private float m_CurrentTime = 0.0f;

    private string _DEFAULT_DISPLAY = "st00";
    private string _DEFAULT_ITEM = "i00";
    private StratagemState m_State = StratagemState.Standby;
    public StratagemState State { get { return m_State; } }

    public enum StratagemState
    {
        Standby, Ready, Activating, Cooling
    }

    public void Init(StratagemInfo data, Transform launchPos)
    {
        m_Info = data;
    }

    // Use this for initialization
    private void Start()
    {
        m_Go = ResourceManager.m_Instance.LoadData(typeof(GameObject), "Startagems", _DEFAULT_DISPLAY, true) as GameObject;
        m_Go.transform.parent = this.transform;
        m_Go.SetActive(false);

        m_Item = ResourceManager.m_Instance.LoadData(typeof(GameObject), "Items", _DEFAULT_ITEM, true) as GameObject;
        m_Item.transform.parent = this.transform;
        m_Item.SetActive(false);

        m_Rigidbody = m_Go.GetComponent<Rigidbody>();
        m_Rigidbody.isKinematic = true;

        m_Anima = m_Go.GetComponent<Animator>();
    }

    private void Update()
    {
        if (m_State == StratagemState.Cooling)
        {
            if (m_CurrentTime >= m_Info.cooldown)
            {
                m_CurrentTime = 0.0f;
                m_State = StratagemState.Standby;
            }
            m_CurrentTime += Time.deltaTime;
        }
    }

    public void ThrowOut(Vector3 direction, float size)
    {
        this.transform.parent = null;
        m_Rigidbody.isKinematic = false;
        Vector3 force = direction.normalized * size;
        m_Rigidbody.AddForce(force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Terrain")
        {
            m_Rigidbody.isKinematic = true;
            this.transform.up = Vector3.up;
            if (m_State == StratagemState.Ready)
            {
                m_State = StratagemState.Activating;
                Invoke("DoActivation", m_Info.activation);
            }
        }
    }

    private void DoActivation()
    {
        m_Go.SetActive(false);
        m_State = StratagemState.Cooling;
    }
}