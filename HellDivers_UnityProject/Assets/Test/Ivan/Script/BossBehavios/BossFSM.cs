using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFSM {

    public enum eFSMTransition
    {
        NULLTRANSITION = -1,
        GO_IDLE,
        G0_SEEK,
        GO_DRAWALERT,
        G0_RUSH,
        GO_MISSILE,
        GO_EARTHQIAKE
    }
    public enum eFSMStateID
    {
        INITIAL = -1,
        IDLE,
        SEEK,
        DRAWALERT,
        RUSH,
        MISSILE,
        EARTHQUACK,
    }
    #region Boss FSM Field
    public eFSMStateID m_StateID;
    protected Dictionary<eFSMTransition, BossFSM> m_Map;
    protected BossStateFuntion m_BSF;
    public float m_fCurrentTime;
    protected bool m_bMissiling;
    #endregion
    public BossFSM()
    {
        m_StateID = eFSMStateID.INITIAL;
        m_fCurrentTime = 0.0f;
        m_Map = new Dictionary<eFSMTransition, BossFSM>();
        m_BSF = new BossStateFuntion();
    }
    public void AddTransition(eFSMTransition trans, BossFSM toState)
    {
        if (m_Map.ContainsKey(trans)) return;
        m_Map.Add(trans, toState);
    }
    public void DeletTransition(eFSMTransition trans)
    {
        if (m_Map.ContainsKey(trans)) m_Map.Remove(trans);
    }
    public BossFSM TransitionTo(eFSMTransition trans)
    {
        if (!m_Map.ContainsKey(trans)) return null;
        return m_Map[trans];

    }
    public virtual void DoBeforeEnter()
    {

    }
    public virtual void DoBeforeLeave(EnemyData data)
    {

    }
    public virtual void Do(EnemyData data)
    {

    }
    public virtual void CheckCondition(EnemyData data)
    {

    }
}

public class BossIdleState : BossFSM{
    float fIdleTime;
    public BossIdleState()
    {
        m_StateID = eFSMStateID.IDLE;
        fIdleTime = 1.0f;
    }
    public override void DoBeforeEnter()
    {
        m_fCurrentTime = 0.0f;
        fIdleTime = Random.Range(1.0f, 3.0f);
    }
    public override void DoBeforeLeave(EnemyData data)
    {
    }
    public override void Do(EnemyData data)
    {
        Debug.Log("Idle");
        m_fCurrentTime += Time.fixedDeltaTime;
    }
    public override void CheckCondition(EnemyData data)
    {
        Debug.Log(m_fCurrentTime);
        if (m_fCurrentTime > fIdleTime)
        {
            data.m_bossFSMSystem.PerformTransition(eFSMTransition.G0_SEEK);
        }
    }
}

public class BossSeekState :BossFSM
{
    float fSeekTime = 0.0f;
    Vector3 vec;
    public BossSeekState()
    {
        m_StateID = eFSMStateID.SEEK;
    }
    public override void DoBeforeEnter()
    {
        if (m_bMissiling) fSeekTime = 0.3f;
        else fSeekTime = Random.Range(3.0f, 6.0f);
        m_fCurrentTime = 0.0f;
    }
    public override void DoBeforeLeave(EnemyData data)
    {
        data.m_vCurFace = vec;
        data.m_vCurPos = data.m_vTarget.position;
    }
    public override void Do(EnemyData data)
    {
        vec = m_BSF.Seek(data.m_Go.transform, data.m_vTarget.position);
        m_fCurrentTime += Time.fixedDeltaTime;

    }
    public override void CheckCondition(EnemyData data)
    {
        if (m_fCurrentTime > fSeekTime)
        {
            data.m_bossFSMSystem.PerformTransition(eFSMTransition.GO_DRAWALERT);
        }
    }
}

public class BossDrawAlertState : BossFSM
{
    float fDrawTime;
    bool bDraw; 
    public BossDrawAlertState()
    {
        m_StateID = eFSMStateID.DRAWALERT;
    }
    public override void DoBeforeEnter()
    {
        fDrawTime = 0.5f;
        m_fCurrentTime = 0.0f;
        bDraw = false;
    }
    public override void DoBeforeLeave(EnemyData data)
    {
        //curActive = DrawTools.GO;
    }
    public override void Do(EnemyData data)
    {
        m_fCurrentTime += Time.fixedDeltaTime;
        if (bDraw) return;
        if (m_BSF.m_Obstacle.Count >= 5)
        {
            for (int i = 0; i < m_BSF.m_Obstacle.Count; i++) { m_BSF.DrawFanAlert(m_BSF.m_Obstacle[i].transform, data.m_Go.transform); }
        }
        else if (data.m_iCurHP >= data.m_iMaxHP * 0.7f)
        {
           m_BSF.DrawRectAlert(data.m_vCurFace, data.m_Go.transform);
        }
        else if (data.m_iCurHP >= data.m_iMaxHP * 0.2f)
        {
            m_BSF.DrawCircleAlert(data.m_vCurPos, data.m_Go.transform);
        }
        else
        {
        }
        bDraw = true;
    }
    public override void CheckCondition(EnemyData data)
    {
        if (m_fCurrentTime < fDrawTime) return;
        if (m_BSF.m_Obstacle.Count >= 5)
        {
            data.m_bossFSMSystem.PerformTransition(eFSMTransition.GO_EARTHQIAKE);
        }
        else if(data.m_iCurHP >= data.m_iMaxHP * 0.7f)
        {
            data.m_bossFSMSystem.PerformTransition(eFSMTransition.G0_RUSH);
        }
        else if (data.m_iCurHP >= data.m_iMaxHP * 0.2f)
        {
            data.m_bossFSMSystem.PerformTransition(eFSMTransition.GO_MISSILE);
        }
        else
        {
        }
    }
}

public class BossRushState : BossFSM
{
    public BossRushState()
    {
        m_StateID = eFSMStateID.RUSH;
    }
    public override void DoBeforeEnter()
    {
    }
    public override void DoBeforeLeave(EnemyData data)
    {
       // ObjectPool.m_Instance.UnLoadObjectToPool((int)BossStateFuntion.EItem.RECTANGLE, curActive);
    }
    public override void Do(EnemyData data)
    {
        m_BSF.Rush(data.m_vCurFace, data.m_Go.transform);
    }
    public override void CheckCondition(EnemyData data)
    {
        if (m_BSF.OnEdge(data.m_Go.transform))
        {
            data.m_bossFSMSystem.PerformTransition(eFSMTransition.GO_IDLE);
        }
    }
}

public class BossMissleState : BossFSM
{
    float fCount;
    float fMissile;
    public BossMissleState()
    {
        m_StateID = eFSMStateID.MISSILE;
    }
    public override void DoBeforeEnter()
    {
        if (!m_bMissiling)
        {
            fCount = Random.Range(3, 7);
            fMissile = 0;
        }
        m_bMissiling = true;
    }
    public override void DoBeforeLeave(EnemyData data)
    {
    }
    public override void Do(EnemyData data)
    {
        if (fMissile < fCount)
        {
            m_BSF.Missile(data.m_vCurPos);
            fCount++;
        }
        else
        {
            m_BSF.ThrowRock(data.m_Go.transform, data.m_vCurPos);
            m_bMissiling = false;
        }
    }
    public override void CheckCondition(EnemyData data)
    {
        data.m_bossFSMSystem.PerformTransition(eFSMTransition.G0_SEEK);
    }
}

public class BossEarthquakeState : BossFSM
{
    float earthquakeTime;
    public BossEarthquakeState()
    {
        m_StateID = eFSMStateID.EARTHQUACK;
    }
    public override void DoBeforeEnter()
    {
        m_fCurrentTime = 0.0f;
        earthquakeTime = 3.0f;
    }
    public override void DoBeforeLeave(EnemyData data)
    {
        m_BSF.AfterEarthquake();
    }
    public override void Do(EnemyData data)
    {
        m_fCurrentTime += Time.fixedDeltaTime;
    }
    public override void CheckCondition(EnemyData data)
    {
        if (m_fCurrentTime > earthquakeTime)
        {
            data.m_bossFSMSystem.PerformTransition(eFSMTransition.GO_IDLE);
        }
    }
}




