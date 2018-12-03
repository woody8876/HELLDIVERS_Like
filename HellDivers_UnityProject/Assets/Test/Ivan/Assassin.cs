using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : Character {

    [SerializeField] float m_fRaidus;
    [SerializeField] float m_fAngle;
    [SerializeField] float m_Speed;
    [SerializeField] Transform Target;

    private delegate void AssassinState();
    private AssassinState Detected;

    Transform m_tTarget;
    Vector3 m_Forward;
    Vector3 prePos;
    Vector3 curPos;
    Vector3 predictedPos;
    float m_2Radius;
    float m_fFallingTime;
    bool m_bActive;
    bool m_bCountDown;
    bool m_bAttack;
    bool m_bReady;
    bool m_b;

    protected override void Start()
    {
        base.Start();
        m_2Radius = m_fRaidus * m_fRaidus;
    }

    private Vector3 CalculatePos()
    {
        Vector3 vec = Vector3.zero;
        vec = Target.position;
        //foreach (Player player in InGamePlayerManager.Instance.Players)
        //{
        //    vec += player.transform.position;
        //}
        //vec /= InGamePlayerManager.Instance.Players.Count;
        return vec;
    }

    private bool Active()
    {
        float radius = (CalculatePos() - transform.position).sqrMagnitude;
        if (radius < m_2Radius) return true;
        else return false;
    }

    private bool Left()
    {
        float radius = (CalculatePos() - transform.position).sqrMagnitude;
        if (radius > m_2Radius * 2) return true;
        else return false;
    }

    IEnumerator CountDown()
    {
        float randomTime = Random.Range(1.0f, 3.0f);
        yield return new WaitForSeconds(randomTime);
        m_bReady = true;
    }

    IEnumerator Prediction()
    {
        prePos = m_tTarget.position;
        yield return new WaitForSeconds(.1f);
        curPos = m_tTarget.position;
        SetPopUp();
        m_bAttack = true;
    }

    private void LockDown()
    {
        float distance = m_2Radius;
        Transform target = null;
        //foreach (Player player in InGamePlayerManager.Instance.Players)
        //{
        //    float dis = (player.transform.position - transform.position).sqrMagnitude;
        //    if (dis < distance)
        //    {
        //        distance = dis;
        //        target = player.transform;
        //    }
        //}
        //m_tTarget = target;
        m_tTarget = Target;
    }

    private void SetPopUp()
    {
        Vector3 forward = PredictedPos() - transform.position;
        m_Forward = forward.normalized;
        forward.y = Mathf.Tan(m_fAngle * Mathf.Deg2Rad);
        transform.forward = forward;
    }

    private void PopUP()
    {
        var v0 = m_Speed * Mathf.Sin(m_fAngle * Mathf.Deg2Rad);
        var v = v0 - 40f * m_fFallingTime;
        transform.position += Vector3.up * v * Time.fixedDeltaTime;
    }

    private void Moving()
    {
        var vF = m_Speed * Mathf.Cos(m_fAngle * Mathf.Deg2Rad);
        transform.position += m_Forward * vF * Time.fixedDeltaTime;
    }

    private void Explosion()
    {
        if(Physics.Raycast(transform.position, Vector3.down, 10, 1 << LayerMask.NameToLayer("Terrain")))
        {
            //DoDamage(transform.position);
            Debug.Log("Boom");
            Destroy(this.gameObject);
        }
    }

    private void DoDamage(Vector3 pos)
    {
        var enemies = Physics.OverlapSphere(pos, 5, 1 << LayerMask.NameToLayer("Enemies") | 1 << LayerMask.NameToLayer("Player"));
        foreach (var enemy in enemies)
        {
            IDamageable target = enemy.gameObject.GetComponent<IDamageable>();
            target.TakeDamage(50, enemy.transform.position);
        }
    }

    private Vector3 PredictedPos()
    {
        Vector3 vec = curPos - prePos;
        float d = (curPos - transform.position).magnitude;
        float t = d / m_Speed;
        Vector3 nextPos = curPos + vec * t;
        return nextPos;
    }
    
    private void FixedUpdate()
    {
        if (!m_bActive)
        {
            m_bActive = Active();
            return;
        }
        if (!m_bCountDown)
        {
            StartCoroutine(CountDown());
            m_bCountDown = true;
        }
        if (!m_bAttack)
        {
            if (!m_b)
            {
                if (Left() || m_bReady)
                {
                    LockDown();
                    StartCoroutine(Prediction());
                    m_b = true;
                    predictedPos = PredictedPos();
                }
            }
            return;
        }
        PopUP();
        Moving();
        Vector3 vec = predictedPos - transform.position;
        transform.forward = Vector3.Lerp(transform.forward, vec.normalized, 0.01f);
        m_fFallingTime += Time.fixedDeltaTime;
        //Explosion();
    }
}
