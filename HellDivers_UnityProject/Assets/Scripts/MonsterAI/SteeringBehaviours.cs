using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SteeringBehaviours
{

    /// <summary>
    /// Find the target and calculate the move force and turn force.
    /// </summary>
    /// <param name="data">Object data</param>
    /// <returns></returns>
    static public bool Seek(AIData data)
    {
        Vector3 curPos = data.m_Go.transform.position;
        Vector3 curDir = data.m_vTarget - curPos;
        float fDist = curDir.magnitude;
        if (fDist < data.m_Speed * Time.deltaTime)
        {
            Vector3 vFinal = data.m_vTarget;
            vFinal.y = curPos.y;
            data.m_Go.transform.position = vFinal;
            data.m_fMoveForce = 0.0f;
            data.m_fTempTurnForce = 0.0f;
            data.m_Speed = 0.0f;
            data.m_bMove = false;
            return false;
        }
        Vector3 vForward = data.m_Go.transform.forward;
        Vector3 vRight = data.m_Go.transform.right;
        curDir.Normalize();
        float fDotForward = Vector3.Dot(curDir, vForward);
        if (fDotForward > 0.96f)
        {
            fDotForward = 1.0f;
            data.m_Go.transform.forward = curDir;
            data.m_fTempTurnForce = 0.0f;
            data.m_fRot = 0.0f;
        }
        else
        {
            if (fDotForward < -1.0f) fDotForward = -1.0f;
            float fDotRight = Vector3.Dot(curDir, vRight);
            if (fDotForward < 0.0f)
            {
                if (fDotRight > 0.0f) fDotRight = 1.0f;
                else fDotRight = -1.0f;
            }
            if (fDist < 1.0f) fDotRight *= fDist;

            data.m_fTempTurnForce = fDotRight;
        }
        data.m_fMoveForce = fDotForward;
        data.m_bMove = true;
        return true;
    }

    /// <summary>
    /// Let NPC move to his target
    /// </summary>
    /// <param name="data">GameObject Data</param>
    static public void Move(AIData data)
    {
        if (data.m_bMove == false) { return; }
        Transform tMover = data.m_Go.transform;
        Vector3 curPos = tMover.position;
        Vector3 vRight = tMover.right;
        Vector3 vForward = tMover.forward;
        //Rotate direction
        if (data.m_fTempTurnForce > data.m_fMaxRot) data.m_fTempTurnForce = data.m_fMaxRot;
        else if (data.m_fTempTurnForce < -data.m_fMaxRot) data.m_fTempTurnForce = -data.m_fMaxRot;
        Vector3 vOriForward = vForward;
        vForward = vForward + vRight * data.m_fTempTurnForce;
        vForward.Normalize();
        tMover.forward = vForward;

        if (data.m_bCol == false)
        {   //Check if mover will collision, if is ture, set mover translate direction bacl to origine to reduce turn angle.
            if (SteeringBehaviours.Seek(data)) tMover.forward = vOriForward;
        }
        if (data.m_Speed < 0.001f) data.m_Speed = 0.001f;
        else if (data.m_Speed >= data.m_fMaxSpeed * data.m_fMoveForce * 0.5f) data.m_Speed = data.m_fMaxSpeed;
        else { data.m_Speed = data.m_Speed + data.m_fMoveForce * Time.deltaTime; }
        curPos = curPos + tMover.forward * data.m_Speed;
        tMover.position = curPos;
    }

    static public bool CollisionAvoid(AIData data)
    {
        //List<Obstacle> m_AvoidTargets = Main.m_Instance.GetObstacles();
        Transform tMover = data.m_Go.transform;
        Vector3 curPos = tMover.position;
        Collider[] m_AvoidTargets = Physics.OverlapSphere(curPos, 30.0f, 1<< LayerMask.NameToLayer("Terrain"));
        Vector3 curForward = tMover.forward;
        Vector3 vMover2Obs;

        float fFinalDotDist;
        float fFinalProjDist;

        Vector3 vFinalVec = Vector3.forward;

        Collider oFinal = null;

        float fDist = 0.0f;
        float fDot = 0.0f;
        float fFinalDot = 0.0f;
        int iCount = m_AvoidTargets.Length;

        float fMinDist = 100.0f;
        for (int i = 0; i < iCount; i++)
        {
            vMover2Obs = m_AvoidTargets[i].transform.position - curPos;
            vMover2Obs.y = 0.0f;
            fDist = vMover2Obs.magnitude;
            vMover2Obs.Normalize();
            fDot = Vector3.Dot(vMover2Obs, curForward);
            if (fDist < data.m_fProbeLength + m_AvoidTargets[i].bounds.size.magnitude && fDot > 0.5f)
            {
                float fProjDist = fDist * fDot;
                float fDotDist = Mathf.Sqrt(fDist * fDist - fProjDist * fProjDist);
                if (fDotDist > m_AvoidTargets[i].bounds.size.magnitude + data.m_fRadius)
                {
                    continue;
                }

                if (fDist < fMinDist)
                {
                    fMinDist = fDist;
                    fFinalDotDist = fDotDist;
                    fFinalProjDist = fProjDist;
                    vFinalVec = vMover2Obs;
                    oFinal = m_AvoidTargets[i];
                    fFinalDot = fDot;
                }
            }
        }
        if (oFinal != null)
        {
            Vector3 vCross = Vector3.Cross(curForward, vFinalVec);
            float fTurnMag = fFinalDot;
            if (vCross.y > 0.0f)
            {
                fTurnMag = -fTurnMag;
            }
            data.m_fTempTurnForce = fTurnMag;

            float fTotalLen = data.m_fProbeLength + oFinal.bounds.size.magnitude;
            float fRatio = fMinDist / fTotalLen;
            if (fRatio > 1.0f)
            {
                fRatio = 1.0f;
            }
            fRatio = 1.0f - fRatio;
            data.m_fMoveForce = -fRatio;
            data.m_bCol = true;
            data.m_bMove = true;
            return true;
        }
        data.m_bCol = false;
        return false;

    }
}