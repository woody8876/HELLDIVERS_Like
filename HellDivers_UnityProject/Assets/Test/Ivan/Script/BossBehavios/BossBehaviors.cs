using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviors : MonoBehaviour
{
    public enum EItem
    {
        MISSILE,
        ROCK,
        CIRCLE,
        RECTANGLE,
        FAN
    }

    public Transform m_Target;
    public Transform m_Center;
    public float m_Radius = 10;
    public float m_Speed = 0.1f;

    private List<GameObject> m_Obstacle = new List<GameObject>();
    private Vector3 m_vPos;
    private Vector3 m_vVec;
    private bool m_bMove;
    private bool m_bCheck;
    float timer;
    float randomTime = 1;


    #region Init Function
    private void InitObject()
    { 
        ObjectPool.m_Instance.InitGameObjects(Resources.Load("Missile"), 3, (int)EItem.MISSILE);
        ObjectPool.m_Instance.InitGameObjects(Resources.Load("Rock"), 5, (int)EItem.ROCK);
        ObjectPool.m_Instance.InitGameObjects(Resources.Load("Range"), 3, (int)EItem.CIRCLE);
        ObjectPool.m_Instance.InitGameObjects(Resources.Load("Rectangle"), 1, (int)EItem.RECTANGLE);
        ObjectPool.m_Instance.InitGameObjects(Resources.Load("Fan"), 5, (int)EItem.FAN);
    }
    #endregion

    #region MonoBehavoirs
    private void Start()
    {
    }
    private void Update()
    {
        #region Phase 1
        /* 
        if (m_bCheck) return;
         if (!m_bMove) TimeCounter();
         else
         {
             StartCoroutine(Rush(m_vVec));
             OnEdge(this.transform);
         }
          */
        #endregion
        #region Phase 2
        //Phase 2
        /*
        if (m_bCheck) return;
        if (m_Obstacle.Count >= 5)
        {
            for (int i = 0; i < m_Obstacle.Count; i++) { DrawFanAlert(m_Obstacle[i].transform); }
            StartCoroutine(Earthquake());
            return;
        }
        if (!m_bMove) TimeCounter();
        else
        {
            m_bCheck = true;
            if (timer >= randomTime - 1) StartCoroutine(ThrowRock());
            else StartCoroutine(Missile());
        }
        */
        #endregion
    }
    #endregion

    #region Doing in Transition
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
            randomTime = Random.Range(2.0f, 6.0f);
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

    #region Boss FSM Function

    public IEnumerator Rush(Vector3 vec)
    {
        m_vVec = Seek(m_Target.position);
        DrawRectAlert();
        //m_bCheck = true;
        vec.Normalize();
        transform.position += vec * Time.deltaTime * m_Speed;
        yield break;
    }
    public IEnumerator Missile()
    {
        Seek(m_Target.position);
        StartCoroutine(DrawCircleAlert());
        yield return new WaitForSeconds(0.5f);
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.MISSILE);
        go.SetActive(true);
        m_vPos.y = 40;
        go.transform.position = m_vPos;
        go.transform.forward = -Vector3.up;
        timer++;
        yield break;
    }
    public IEnumerator ThrowRock()
    {
        Seek(m_Target.position);
        StartCoroutine(DrawCircleAlert());
        yield return new WaitForSeconds(0.5f);
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.ROCK);
        go.SetActive(true);
        Vector3 vec = transform.position;
        m_vPos.y = vec.y = 40;
        go.transform.forward = m_vPos - vec;
        go.transform.position = m_vPos;
        m_Obstacle.Add(go);
        timer = 0;
        randomTime = Random.Range(2.0f, 6.0f);
        m_bMove = false;
        yield break;
    }
    public IEnumerator Earthquake()
    {
        m_bCheck = true;
        m_bMove = false;
        randomTime = 2;
        yield return new WaitForSeconds(randomTime);
        for (int i = 0; i < m_Obstacle.Count; i++)
        {
            GameObject fan = GameObject.Find("Fan(Clone)");
            ObjectPool.m_Instance.UnLoadObjectToPool((int)EItem.ROCK, m_Obstacle[i]);
            ObjectPool.m_Instance.UnLoadObjectToPool((int)EItem.FAN, fan);
        }
        m_Obstacle.Clear();
    }
    #endregion
    
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