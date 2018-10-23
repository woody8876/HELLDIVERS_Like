using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateFuntion {

    public enum EItem
    {
        MISSILE,
        ROCK,
        CIRCLE,
        RECTANGLE,
        FAN
    }

    //public Transform m_Target;
    public Transform m_Center;
    public float m_Radius = 10;
    public float m_Speed = 0.1f;

    private List<GameObject> m_Obstacle = new List<GameObject>();
    //private Vector3 m_vPos;
    //private Vector3 m_vVec;
    private bool m_bMove;
    private bool m_bCheck;
    float timer;
    float randomTime = 1;


    #region Init Function
    public BossStateFuntion()
    {
        ObjectPool.m_Instance.InitGameObjects(Resources.Load("Missile"), 3, (int)EItem.MISSILE);
        ObjectPool.m_Instance.InitGameObjects(Resources.Load("Rock"), 5, (int)EItem.ROCK);
        ObjectPool.m_Instance.InitGameObjects(Resources.Load("Range"), 3, (int)EItem.CIRCLE);
        ObjectPool.m_Instance.InitGameObjects(Resources.Load("Rectangle"), 1, (int)EItem.RECTANGLE);
        ObjectPool.m_Instance.InitGameObjects(Resources.Load("Fan"), 5, (int)EItem.FAN);
    }
    #endregion

    #region MonoBehavoirs
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
            //Seek(m_Target.position);
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

    public void DrawRectAlert(Vector3 target, Transform user)
    {
        DrawTools.GO = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.RECTANGLE);
        DrawTools.GO.SetActive(true);
        target.y = 0;
        DrawTools.DrawRectangleSolid(user, target, 10, 2);
    }
    public void DrawFanAlert(Transform target, Transform user)
    {
        DrawTools.GO = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.FAN);
        DrawTools.GO.SetActive(true);
        float width = target.localScale.x * .5f;
        float dis = (target.position - user.position).magnitude;
        float angle = Mathf.Tan(width / dis) * Mathf.Rad2Deg * 2;
        Vector3 pos = target.position;
        Vector3 Cpos = user.position;
        pos.y = Cpos.y = 0f;
        DrawTools.DrawSectorSolid(target, Cpos, pos, angle, 25, width * 2);
        DrawTools.GO.transform.position += Vector3.up * 1.1f;
    }
    public void DrawCircleAlert(Vector3 target, Transform user)
    {
        DrawTools.GO = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.CIRCLE);
        DrawTools.GO.SetActive(true);
        Vector3 pos = user.position;
        pos.y = 0;
        DrawTools.DrawCircleSolid(user, pos, 3);
        target.y = 1.1f;
        DrawTools.GO.transform.position = target;
    }
    #endregion

    #region Boss FSM Function
    public void Idle() { }

    public Vector3 Seek(Transform user, Vector3 target)
    {
        Vector3 vec2Target = target - user.position;
        vec2Target.y = 0;
        Quaternion face = Quaternion.LookRotation(vec2Target, Vector3.up);
        user.rotation = Quaternion.RotateTowards(user.rotation, face, 5);
        return vec2Target;
    }

    public void Rush(Vector3 vec, Transform user)
    {
        vec.Normalize();
        user.position += vec * Time.fixedDeltaTime * m_Speed;
    }
    public void Missile(Vector3 target)
    {
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.MISSILE);
        go.SetActive(true);
        target.y = 40;
        go.transform.position = target;
        go.transform.forward = -Vector3.up;
    }
    public void ThrowRock(Transform user, Vector3 target)
    {
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.ROCK);
        go.SetActive(true);
        Vector3 vec = user.position;
        target.y = vec.y = 40;
        go.transform.forward = target - vec;
        go.transform.position = target;
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
