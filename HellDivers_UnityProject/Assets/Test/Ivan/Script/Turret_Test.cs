using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponController))]
public class Turret_Test : MonoBehaviour {


    [SerializeField] Transform m_GunPos;

    private WeaponController m_weaponController;

    private GameObject m_CurrentTarget;
    Collider[] targets;
    float m_fTimer = .4f;

    private void Start()
    {
        m_weaponController = GetComponent<WeaponController>();
        m_weaponController.AddWeapon(1901, m_GunPos, null);
    }
	
	// Update is called once per frame
	void Update () {
        if (m_fTimer > 0)
        {
            m_fTimer -= Time.fixedDeltaTime;
            return;
        }
        else if (!Attack()) { m_fTimer = .4f; }
        else {
            CheckTargetDead();
            if (!Turning()) return;
            m_weaponController.ShootState();
        }

    }

    private bool EnemyInSight()
    {
        targets = Physics.OverlapSphere(this.transform.position, 15f, 1 << LayerMask.NameToLayer("Enemies"));
        if (targets.Length == 0) return false;
        return true;            
    }

    private GameObject SetNextTarget()
    {
        GameObject nextTarget = null;
        if (EnemyInSight())
        {
            float nextDot = -1;
            for (int i = 0; i < targets.Length; i++)
            {
                Vector3 targetVec = targets[i].transform.position - this.transform.position;
                Vector3 normalVec = targetVec.normalized;
                float dot = Vector3.Dot(normalVec, this.transform.forward);
                if (dot > nextDot)
                {
                    nextDot = dot;
                    nextTarget = targets[i].gameObject;
                }
            }
        }
        return nextTarget;
    }

    private bool Attack()
    {
        if (m_CurrentTarget != null) return true;
        m_CurrentTarget = SetNextTarget();
        if (m_CurrentTarget != null) return true;
        return false;
    }

    private bool Turning()
    {
        Vector3 vec = m_CurrentTarget.transform.position - this.transform.position;
        vec.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, vec, 0.03f);
        Vector3 normal = vec.normalized;
        if (Vector3.Dot(transform.forward, normal) > .97f) return true;
        return false;
    }

    private void CheckTargetDead()
    {
        IDamageable target = m_CurrentTarget.GetComponent<IDamageable>();
        if (target.IsDead) m_CurrentTarget = null;
    }



}
