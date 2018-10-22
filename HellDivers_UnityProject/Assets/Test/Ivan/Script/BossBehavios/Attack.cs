using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAttackMode
{
    INITIAL = -1,
    RUSH,
    MISSILE,
    EARTHQUACK,
    THROWSTONE,
    SEEK
}

public enum EItem
{
    MISSILE,
    ROCK,
    CIRCLE,
    RECTANGLE,
    FAN
}
public class Attack : MonoBehaviour
{

    public Transform m_Target;
    public Transform m_Center;
    public float m_Radius = 10;
    public float m_Speed = 0.1f;
    public Transform[] m_Obstacle;

    private Object m_Missile;
    private Object m_Rock;
    private Object m_Circle;
    private Object m_Rectangle;
    private Object m_Fan;

    private Vector3 m_vPos;
    private Vector3 m_vVec;
    private bool m_bMove;
    private bool m_bCheck;
    float timer;
    float randomTime = 1;


    #region Init Function
    private void InitObject()
    {
        m_Circle = Resources.Load("Range") as GameObject;
        m_Rectangle = Resources.Load("Rectangle") as GameObject;
        m_Fan = Resources.Load("Fan") as GameObject;
        m_Missile = Resources.Load("Missile");
        m_Rock = Resources.Load("Rock");
    }
    public void CreateObjectinPool()
    {
        ObjectPool.m_Instance.InitGameObjects(m_Missile, 3, (int)EItem.MISSILE);
        ObjectPool.m_Instance.InitGameObjects(m_Rock, 5, (int)EItem.ROCK);
        ObjectPool.m_Instance.InitGameObjects(m_Circle, 3, (int)EItem.CIRCLE);
        ObjectPool.m_Instance.InitGameObjects(m_Rectangle, 1, (int)EItem.RECTANGLE);
        ObjectPool.m_Instance.InitGameObjects(m_Fan, 5, (int)EItem.FAN);
    }
    #endregion

    private void Start()
    {
        InitObject();
        CreateObjectinPool();
        //for (int i = 0; i < m_Obstacle.Length; i++) { DrawFanAlert(m_Obstacle[i]); }
    }
    private void Update()
    {
        //Phase 1
        /* 
         if (m_bCheck) return;
         if (!m_bMove) TimeCounter();
         else
         {
             StartCoroutine(Rush(m_vVec));
             OnEdge(this.transform);
         }
          */

        //Phase 2
        /*
        if (m_bCheck) return;
        if (!m_bMove) TimeCounter();
        else
        {
            m_bCheck = true;
            StartCoroutine(CreateMissile());
        }*/
        if (m_bCheck) return;
        if (!m_bMove) TimeCounter();
        else
        {
            m_bCheck = true;
            StartCoroutine(CreateRock());
        }
        

    }
    #region Common Behaviors
    public void Idle() { }

    public void TimeCounter()
    {
        timer += Time.deltaTime;
        if(timer < randomTime - 0.5f)
        {
            Seek(m_Target.position);
        }
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
        m_vPos = m_Target.position;
        return vec2Target;
    }
    #endregion

    #region Draw Alert
    public void DrawRectAlert()
    {
        DrawTools.GO = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.RECTANGLE);
        DrawTools.GO.SetActive(true); 
        m_vVec.y = 0;
        DrawTools.DrawRectangleSolid(transform, m_vVec, 10, 2);
    }
    public void DrawFanAlert(Transform t)
    {
        DrawTools.GO = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.FAN);
        DrawTools.GO.SetActive(true);
        float width = t.localScale.x * .5f;
        float dis = (t.position - transform.position).magnitude;
        float angle = Mathf.Tan(width / dis) * Mathf.Rad2Deg * 2;
        Vector3 pos = t.position;
        Vector3 Cpos = transform.position;
        pos.y = Cpos.y = 0f;
        DrawTools.DrawSectorSolid(t, Cpos, pos, angle, 25, width * 2);
        DrawTools.GO.transform.position += Vector3.up*1.1f;
    }
    public IEnumerator DrawCircleAlert()
    {
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.CIRCLE);
        if (go == null) { go = Instantiate(m_Circle, m_vPos, transform.rotation) as GameObject; }
        go.SetActive(true);
        DrawTools.GO = go;
        Vector3 pos = transform.position;
        pos.y = 0;
        DrawTools.DrawCircleSolid(transform, pos, 3);
        m_vPos.y = 1.1f;
        go.transform.position = m_vPos;
        yield return new WaitForSeconds(.8f);
        ObjectPool.m_Instance.UnLoadObjectToPool(2, go);
        m_bCheck = false;
        yield break;
    }
    #endregion

    #region Attack

    //public bool Rush(Vector3 vec)
    //{
    //    m_bCheck = true;
    //    vec.Normalize();
    //    transform.position += vec * Time.deltaTime * m_Speed;

    //    return true;
    //}

    public IEnumerator Rush(Vector3 vec)
    {
        m_vVec = Seek(m_Target.position);
        DrawRectAlert();
        //m_bCheck = true;
        vec.Normalize();
        transform.position += vec * Time.deltaTime * m_Speed;
        yield break;
    }

    public IEnumerator CreateMissile()
    {
        Seek(m_Target.position);
        StartCoroutine(DrawCircleAlert());
        yield return new WaitForSeconds(0.5f);
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.MISSILE);
        if (go == null) { go = Instantiate(m_Missile, m_vPos, transform.rotation) as GameObject;  }
        go.SetActive(true);
        m_vPos.y = 40;
        go.transform.position = m_vPos;
        go.transform.forward = -Vector3.up;
        timer++;
        yield return new WaitWhile(() =>timer < 5);
        timer = 0;
        m_bMove = false;
        yield break;
    }

    public IEnumerator CreateRock()
    {
        Seek(m_Target.position);
        StartCoroutine(DrawCircleAlert());
        yield return new WaitForSeconds(0.5f);
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.ROCK);
        if (go == null) { go = Instantiate(m_Rock, m_vPos, transform.rotation) as GameObject;  }
        go.SetActive(true);
        go.transform.forward = m_vPos - transform.position;
        m_vPos.y = 40;
        go.transform.position = m_vPos;
        timer++;
        yield return new WaitWhile(() =>timer < 5);
        timer = 0;
        m_bMove = false;
        yield break;
    }

    #endregion
    public bool OnEdge(Transform mover)
    {
        if (!m_bCheck) return false;
        if (Mathf.Pow((mover.position.x - m_Center.position.x), 2) + Mathf.Pow((mover.position.z - m_Center.position.z), 2) >= m_Radius * m_Radius)
        {
            m_bMove = false;
            m_bCheck = false;
            DrawTools.GO.SetActive(false);
            return true;
        }
        return false;
    }

    
    #region Fixing Function
    public void PlayAnimation() { }
    public void LineFunction(Transform t){}
    public bool OnCircle(Vector3 pos)
    {
        if (Mathf.Pow((pos.x - m_Center.position.x), 2) + Mathf.Pow((pos.z - m_Center.position.z), 2) == m_Radius * m_Radius)
        {
            return true;
        }
        return false;
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
    #endregion
}