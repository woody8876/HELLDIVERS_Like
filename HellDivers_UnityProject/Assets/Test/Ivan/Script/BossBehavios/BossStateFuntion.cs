using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateFuntion : MonoBehaviour {

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
        InitObject();
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

    #region CheckCondition
    public void TimeCounter()
    {
        timer += Time.deltaTime;
        if (timer < randomTime - 0.5f)
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
        DrawTools.GO.transform.position += Vector3.up * 1.1f;
    }
    public void DrawCircleAlert()
    {
        DrawTools.GO = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.CIRCLE);
        DrawTools.GO.SetActive(true);
        Vector3 pos = transform.position;
        pos.y = 0;
        DrawTools.DrawCircleSolid(transform, pos, 3);
        m_vPos.y = 1.1f;
        DrawTools.GO.transform.position = m_vPos;
    }
    #endregion

    #region Boss FSM Function
    public void Idle() { }

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

    public void Rush(Vector3 vec)
    {
        m_vVec = Seek(m_Target.position);
        vec.Normalize();
        transform.position += vec * Time.deltaTime * m_Speed;
    }
    public void Missile()
    {
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.MISSILE);
        go.SetActive(true);
        m_vPos.y = 40;
        go.transform.position = m_vPos;
        go.transform.forward = -Vector3.up;
    }
    public void ThrowRock()
    {
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.ROCK);
        go.SetActive(true);
        Vector3 vec = transform.position;
        m_vPos.y = vec.y = 40;
        go.transform.forward = m_vPos - vec;
        go.transform.position = m_vPos;
        m_Obstacle.Add(go);
    }
    public void AfterEarthquake()
    {
        for (int i = 0; i < m_Obstacle.Count; i++)
        {
            GameObject fan = GameObject.Find("Fan(Clone)");
            ObjectPool.m_Instance.UnLoadObjectToPool((int)EItem.ROCK, m_Obstacle[i]);
            ObjectPool.m_Instance.UnLoadObjectToPool((int)EItem.FAN, fan);
        }
        m_Obstacle.Clear();
    }
    #endregion

}
