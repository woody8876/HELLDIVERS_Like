using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Ray : MonoBehaviour, IDamager {


    [SerializeField] eWeaponType m_Type;
    [SerializeField] int m_ID;
    [SerializeField] LineRenderer m_Line;
    [SerializeField] Animator m_Animator;

    private float m_fRange;
    private float m_Time;
    private bool m_bActive;
    private WeaponController m_WeaponController;
    private Vector3 m_vEndPos;
    private Transform m_StartPos;

    public Player Damager { get; private set; }
    public float Damage { get; private set; }

    void Start () {
        m_fRange = GameData.Instance.WeaponInfoTable[m_ID].Range;
        Damage = GameData.Instance.WeaponInfoTable[m_ID].Damage * Time.fixedDeltaTime;
        m_Animator.SetTrigger("startTrigger");
        Debug.Log(m_Line);
    }

    private void OnEnable()
    {
        if (m_Animator == null) return;
        m_Animator.SetTrigger("startTrigger");
        transform.position = m_StartPos.position;
        transform.forward = m_StartPos.forward;
        transform.SetParent(m_StartPos);
    }
    
    // Update is called once per frame
    void FixedUpdate() {
        if (!m_bActive) return;
        if (!Input.GetButton("Fire1") && Input.GetAxis(PlayerManager.Instance.Players[Damager.SerialNumber].controllerSetting.Fire) == 0)
        {
            StartCoroutine(Unload());
            m_bActive = false;
        }
        Detect();
	}

    public void SetPlayer(Player player) { Damager = player;　}

    public void SetBool(bool active) { m_bActive = active; }

    public void SetParent(Transform t) { m_StartPos = t; }

    private void SetLength(float length)
    {
        m_vEndPos = Vector3.forward * length;
        m_Line.SetPosition(1, m_vEndPos);
    }

    IEnumerator Unload()
    {
        m_Animator.SetTrigger("endTrigger");
        yield return new WaitUntil(() => CheckState());
        transform.SetParent(GameObject.Find("Bullet").transform);
        ObjectPool.m_Instance.UnLoadObjectToPool(m_ID, this.gameObject);
    }
    private bool CheckState()
    {
        AnimatorStateInfo m_StateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (m_StateInfo.IsName("end") && m_StateInfo.normalizedTime > 0.8f) return true;
        return false;
    }

    private void Detect()
    {
        RaycastHit rh;
        GameObject go;
        IDamageable target = null;
        if (Physics.Raycast(transform.position, transform.forward, out rh, m_fRange, 1 << LayerMask.NameToLayer("Enemies") | 1 << LayerMask.NameToLayer("Battle")))
        {
            go = rh.collider.gameObject;
            target = go.GetComponent<IDamageable>();
            target.TakeDamage(this, rh.point);
            SetLength(rh.distance);
        }
        else if (Physics.Raycast(transform.position, transform.forward, out rh, m_fRange, 1 << LayerMask.NameToLayer("Player")))
        {
            go = rh.collider.gameObject;
            target = go.GetComponent<IDamageable>();
            target.TakeDamage(Damage * 0.5f, rh.point);
            SetLength(rh.distance);
        }
        else if (Physics.Raycast(transform.position, transform.forward, out rh, m_fRange, 1 << LayerMask.NameToLayer("Obstcale")))
        {
            SetLength(rh.distance);
        }
        else
        {
            SetLength(m_fRange);
        }
    }


}
