using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Ray : MonoBehaviour {

    [HideInInspector]
    public Transform StartPos;
    [HideInInspector]
    public bool m_bActive;
    [SerializeField] private eWeaponType m_Type;
    [SerializeField] private int m_ID;

    private float m_fRange;
    private float m_fDamage;
    private float m_Time;
    private Animator m_animator;
    private Vector3 m_vEndPos;
    private LineRenderer m_Line;
    private bool m_bTrigger;

    void Start () {
        m_fRange = GameData.Instance.WeaponInfoTable[m_ID].Range;
        m_fDamage = GameData.Instance.WeaponInfoTable[m_ID].Damage * Time.fixedDeltaTime;
        m_animator = this.GetComponent<Animator>();
        m_animator.SetTrigger("startTrigger");
        m_Line = this.GetComponentInChildren<Transform>(true).GetComponentInChildren<LineRenderer>();
        Debug.Log(m_Line);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!m_bActive) return;
        if (!m_bTrigger)
        {
            m_animator.SetTrigger("startTrigger");
            m_bTrigger = true;
        }
        if (!Input.GetButton("Fire1"))
        {
            m_animator.SetTrigger("endTrigger");
            m_bActive = m_bTrigger = false;
            ObjectPool.m_Instance.UnLoadObjectToPool(m_ID, this.gameObject);
        }
        SetPosition();
        Detect();

	}

    private void SetPosition()
    {
        transform.position = StartPos.position;
        transform.forward = StartPos.forward;
    }

    private void SetLength(float length)
    {
        m_vEndPos = Vector3.forward * length;
        m_Line.SetPosition(1, m_vEndPos);
    }

    private void Detect()
    {
        RaycastHit rh;
        if (Physics.Raycast(transform.position, transform.forward, out rh, m_fRange, 1 << LayerMask.NameToLayer("Enemies")))
        {
            IDamageable target = rh.transform.gameObject.GetComponent<IDamageable>();
            target.TakeDamage(m_fDamage, rh.point);
            SetLength(rh.distance);
        }
        else if (Physics.Raycast(transform.position, transform.forward, out rh, m_fRange, 1 << LayerMask.NameToLayer("Terrain")))
        {
            SetLength(rh.distance);
        }
        else
        {
            SetLength(m_fRange);
        }
    }


}
