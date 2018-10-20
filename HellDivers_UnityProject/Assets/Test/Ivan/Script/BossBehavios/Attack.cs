using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAttackMode
{
    RUSH,
    MISSILE,
    EARTHQUACK,
    THROWSTONE,
    SEEK
}

public class Attack : MonoBehaviour
{

    public Transform m_Target;
    public Transform m_Center;
    public float m_Radius = 10;
    public float m_Speed = 0.1f;

    private Object m_Rock;
    private Vector3 m_Goal;
    private bool m_bMove;
    private bool m_bCheck;
    float timer;
    float randomTime = 1;
    private void Start()
    {
        DrawTools.GO = GameObject.Find("Range");
        m_Rock = Resources.Load("Rock");
        ObjectPool.m_Instance.InitGameObjects(m_Rock, 10, 1);   
    }

    private void Update()
    {
        //Phase 1
        /*
         if (!Rush(m_Goal)) TimeCounter();
         if (m_bMove) OnEdge(this.transform);
         */
        if (m_bMove && !m_bCheck)
        {
            m_bCheck = true;
            StartCoroutine(CreateStone());
        }
        else
        {
           TimeCounter();
        }
    }

    #region Common Behaviors
    public void Idle() { }

    //public void TimeCounter()
    //{
    //    m_bCheck = false;
    //    timer += Time.deltaTime;
    //    if (timer < randomTime - 0.5f)
    //    {
    //        Seek(m_Target.position);
    //        m_Goal = m_Target.position;
    //    }
    //    if (timer > randomTime - 0.5f)
    //    {
    //        DrawTools.GO.SetActive(true);
    //    }
    //    if (timer >= randomTime)
    //    {
    //        m_bMove = true;
    //        timer = 0;
    //        randomTime = Random.Range(1.0f, 3.0f);
    //    }
    //}

    public void TimeCounter()
    {
//        m_bCheck = false;
        timer += Time.deltaTime;
        if(timer < randomTime - 0.5f)
        {
            Seek(m_Target.position);
        }
        //if (timer > randomTime - 0.5f)
        //{
        //    DrawAlert(transform, EAttackMode.MISSILE);
        //}
        if (timer >= randomTime)
        {
            m_bMove = true;
            timer = 0;
            randomTime = Random.Range(3.0f, 5.0f);
        }
    }

    public Vector3 Seek(Vector3 target)
    {
        Vector3 vec2Target = target - transform.position;
        Vector3 curForward = transform.forward;
        vec2Target.y = 0;
        Quaternion face = Quaternion.LookRotation(vec2Target, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, face, 5);
        m_Goal = m_Target.position;
        return vec2Target;
    }

    public void DrawAlert(Transform t, EAttackMode attackMode)
    {
        switch (attackMode)
        {
            case EAttackMode.RUSH:
                DrawTools.DrawRectangleSolid(t, t.position, 10, 2);
                break;
            case EAttackMode.MISSILE:
                DrawTools.DrawCircleSolid(t, m_Goal, 3);
                break;
        }
    }
    #endregion
        #region Attack

    public bool Rush(Vector3 vec)
    {
        if (!m_bMove) return false;
        m_bCheck = true;
        vec.Normalize();
        transform.position += vec * Time.deltaTime * m_Speed;

        return true;
    }

    public IEnumerator CreateStone()
    {
        Seek(m_Target.position);
        DrawAlert(transform, EAttackMode.MISSILE);
        yield return new WaitForSeconds(0.2f);
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool(1);
        if (go == null) { go = Instantiate(m_Rock, m_Goal, transform.rotation) as GameObject;  }
        go.SetActive(true);
        m_Goal.y = 70;
        go.transform.position = m_Goal;
        go.transform.forward = -Vector3.up;
        timer++;
        m_bCheck = false;
        yield return new WaitWhile(() =>timer < 5);
        timer = 0;
        m_bMove = false;
        yield break;
    }

    #endregion



    public void PlayAnimation()
    {

    }

    public bool OnEdge(Transform mover)
    {
        if (!m_bCheck) return false;
        if (Mathf.Pow((mover.position.x - m_Center.position.x), 2) + Mathf.Pow((mover.position.z - m_Center.position.z), 2) >= m_Radius * m_Radius)
        {
            m_bMove = false;
            DrawTools.GO.SetActive(false);
            return true;
        }
        return false;
    }












    public bool OnCircle(Vector3 pos)
    {
        if (Mathf.Pow((pos.x - m_Center.position.x), 2) + Mathf.Pow((pos.z - m_Center.position.z), 2) == m_Radius * m_Radius)
        {
            return true;
        }
        return false;
    }
    public void LineFunction(Transform t)
    {

    }
    public float Interscetion(Transform t)
    {
        float PowRadius = m_Radius * m_Radius;
        Vector3 dis = t.position - m_Center.position;
        float x1 = t.forward.x;
        float z1 = t.forward.z;
        float a = x1 * (dis.x) - z1 * (dis.z);
        float d = x1 * (dis.z) - z1 * (dis.x);
        float f = x1 * x1 + z1 * z1;
        float power1 = Mathf.Sqrt(((PowRadius * f) - (d * d)) / (f * f)) - (a / f);
        float power2 = -Mathf.Sqrt(((PowRadius * f) - (d * d)) / (f * f)) - (a / f);

        float length = t.forward.magnitude * ((power1 > power2) ? power1 : power2);

        return length;
    }
}