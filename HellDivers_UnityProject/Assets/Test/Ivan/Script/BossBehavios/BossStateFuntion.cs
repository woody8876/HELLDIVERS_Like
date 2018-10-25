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

    public float m_Radius = 25;
    public float m_Speed = 100f;
    #region Init Function
    public void Init()
    {
        ObjectPool.m_Instance.InitGameObjects(Resources.Load("Missile"), 3, (int)EItem.MISSILE);
        ObjectPool.m_Instance.InitGameObjects(Resources.Load("Rock"), 5, (int)EItem.ROCK);
        ObjectPool.m_Instance.InitGameObjects(Resources.Load("Range"), 3, (int)EItem.CIRCLE);
        ObjectPool.m_Instance.InitGameObjects(Resources.Load("Rectangle"), 1, (int)EItem.RECTANGLE);
        ObjectPool.m_Instance.InitGameObjects(Resources.Load("Fan"), 5, (int)EItem.FAN);
    }
    #endregion
    
    #region CheckCondition
    public bool OnEdge(Transform mover, Transform center)
    {
        if (Mathf.Pow((mover.position.x - center.position.x), 2) + Mathf.Pow((mover.position.z - center.position.z), 2) >= m_Radius * m_Radius)
        {
            return true;
        }
        return false;
    }
    #endregion
    
    #region Draw Alert
    public void DrawRectAlert(Transform user, out GameObject go)
    {
        DrawTools.GO = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.RECTANGLE);
        DrawTools.GO.SetActive(true);
        go = DrawTools.GO;
        Vector3 pos = user.position;
        pos.y = 1.1f;
        DrawTools.DrawRectangleSolid(user, pos, 10, 2);
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
    public void DrawCircleAlert(Vector3 target, Transform user, out GameObject go)
    {
        DrawTools.GO = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.CIRCLE);
        DrawTools.GO.SetActive(true);
        go = DrawTools.GO;
        target.y = 1.1f;
        DrawTools.DrawCircleSolid(user, target, 3);
    }
    #endregion
    
    #region Boss FSM Function
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
    public void ThrowRock(Transform user, Vector3 target, EnemyData data)
    {
        GameObject go = ObjectPool.m_Instance.LoadGameObjectFromPool((int)EItem.ROCK);
        go.SetActive(true);
        Vector3 vec = user.position;
        target.y = vec.y = 40;
        go.transform.forward = target - vec;
        go.transform.position = target;
        data.m_Obstacle.Add(go);
    }
    public void AfterEarthquake(EnemyData data)
    {
        for (int i = 0; i < data.m_Obstacle.Count; i++)
        {
            GameObject fan = GameObject.Find("Fan(Clone)");
            ObjectPool.m_Instance.UnLoadObjectToPool((int)EItem.ROCK, data.m_Obstacle[i]);
            ObjectPool.m_Instance.UnLoadObjectToPool((int)EItem.FAN, fan);
        }
        data.m_Obstacle.Clear();
    }
    #endregion

}
