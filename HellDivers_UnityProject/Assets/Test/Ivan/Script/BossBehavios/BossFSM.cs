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
    public virtual void DoBeforeEnter(EnemyData data)
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
    public override void DoBeforeEnter(EnemyData data)
    {
        m_fCurrentTime = 0.0f;
        if (data.m_iCurHP < data.m_iMaxHP * 0.2f) fIdleTime = 0.1f;
        else fIdleTime = Random.Range(1.0f, 3.0f);
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
    public override void DoBeforeEnter(EnemyData data)
    {
        if (data.m_bMissiling) fSeekTime = 0.2f;
        else if (data.m_iCurHP < data.m_iMaxHP * 0.2f) fSeekTime = 1.0f;
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
    public override void DoBeforeEnter(EnemyData data)
    {
        fDrawTime = 0.5f;
        m_fCurrentTime = 0.0f;
        bDraw = false;
    }
    public override void DoBeforeLeave(EnemyData data)
    {
    }
    public override void Do(EnemyData data)
    {
        m_fCurrentTime += Time.fixedDeltaTime;
        if (bDraw) return;
        if (data.m_Obstacle.Count >= 5)
        {
            for (int i = 0; i < data.m_Obstacle.Count; i++) { m_BSF.DrawFanAlert(data.m_Obstacle[i].transform, data.m_vCenter); }
        }
        else if (data.m_iCurHP >= data.m_iMaxHP * 0.7f)
        {
           m_BSF.DrawRectAlert(data.m_Go.transform, out data.m_curActive);
        }
        else if (data.m_iCurHP >= data.m_iMaxHP * 0.2f)
        {
            m_BSF.DrawCircleAlert(data.m_vCurPos, data.m_Go.transform, out data.m_curActive);
        }
        else
        {
            if (!data.m_bRushing) m_BSF.DrawCircleAlert(data.m_vCurPos, data.m_Go.transform, out data.m_curActive);
            else m_BSF.DrawRectAlert(data.m_Go.transform, out data.m_curActive);
        }
        bDraw = true;
    }
    public override void CheckCondition(EnemyData data)
    {
        if (m_fCurrentTime < fDrawTime) return;
        if (data.m_Obstacle.Count >= 5)
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
            if (!data.m_bRushing) data.m_bossFSMSystem.PerformTransition(eFSMTransition.GO_MISSILE);
            else data.m_bossFSMSystem.PerformTransition(eFSMTransition.G0_RUSH);
        }
    }
}

public class BossRushState : BossFSM
{
    float fRushTime;
    float fcurRush;
    float fRushCount;
    public BossRushState()
    {
        m_StateID = eFSMStateID.RUSH;
    }
    public override void DoBeforeEnter(EnemyData data)
    {
        m_fCurrentTime = 0.0f;
        fRushTime = 0.1f;
        if (data.m_iCurHP < data.m_iMaxHP * 0.2f) fRushCount = Random.Range(3, 7);
        else fRushCount = 1;
    }
    public override void DoBeforeLeave(EnemyData data)
    {
        ObjectPool.m_Instance.UnLoadObjectToPool((int)BossStateFuntion.EItem.RECTANGLE, data.m_curActive);
        fcurRush++;
    }
    public override void Do(EnemyData data)
    {
        m_BSF.Rush(data.m_vCurFace, data.m_Go.transform);
        m_fCurrentTime += Time.fixedDeltaTime;
        
    }
    public override void CheckCondition(EnemyData data)
    {
        if (m_fCurrentTime < fRushTime) return;
        if (fcurRush >= fRushCount)
        {
            data.m_bRushing = false;
            fcurRush = 0;
        }
        if (m_BSF.OnEdge(data.m_Go.transform, data.m_vCenter))
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
    public override void DoBeforeEnter(EnemyData data)
    {
        if (!data.m_bMissiling)
        {
            fCount = Random.Range(3, 7);
            fMissile = 0;
        }
        data.m_bMissiling = true;
    }
    public override void DoBeforeLeave(EnemyData data)
    {
        ObjectPool.m_Instance.UnLoadObjectToPool((int)BossStateFuntion.EItem.CIRCLE, data.m_curActive);
    }
    public override void Do(EnemyData data)
    {
        if (fMissile < fCount)
        {
            m_BSF.Missile(data.m_vCurPos);
            fMissile++;
        }
        else
        {
            m_BSF.ThrowRock(data.m_vCenter, data.m_vCurPos, data);
            data.m_bMissiling = false;
            data.m_bRushing = true;
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
    public override void DoBeforeEnter(EnemyData data)
    {
        m_fCurrentTime = 0.0f;
        earthquakeTime = 3.0f;
    }
    public override void DoBeforeLeave(EnemyData data)
    {
        m_BSF.AfterEarthquake(data);
        data.m_bEarthquake = false;
    }
    public override void Do(EnemyData data)
    {
        if (!data.m_bEarthquake) m_BSF.JumpUP(data.m_Go.transform, data.m_vCenter, 50, data);
        else  m_BSF.FallDown(data.m_Go.transform, data.m_vCenter, 100, data);
        
        if (data.m_bEarthquake &&(data.m_Go.transform.position - data.m_vCenter.position).sqrMagnitude < 1)
        {
            m_fCurrentTime += Time.fixedDeltaTime;
        }
    }
    public override void CheckCondition(EnemyData data)
    {
        if (m_fCurrentTime > earthquakeTime)
        {
            data.m_bossFSMSystem.PerformTransition(eFSMTransition.GO_IDLE);
        }
    }
}




