using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimationsContorller))]
[RequireComponent(typeof(CharacterController))]
public class JoystickController : MonoBehaviour
{
    #region Private Variable

    private CharacterController m_Controller;
    private Transform m_Cam;
    private Vector3 m_CamForward;
    private Vector3 m_Move;
    private Vector3 m_Fall;
    private Vector3 m_Direction;
    private PlayerAnimationsContorller m_PAC;

    private bool bRun = false;
    private bool bInBattle = false;

    private PlayerFSMData m_FSMData;
    private PlayerFSMSystem m_PlayerFSM;

    #endregion Private Variable

    #region MonoBehaviour
    private void Awake()
    {

    }
    private void Start()
    {
        m_PAC = this.GetComponent<PlayerAnimationsContorller>();
        m_Controller = this.GetComponent<CharacterController>();

        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }

        #region PlayerFSM

        m_FSMData = new PlayerFSMData();
        m_PlayerFSM = new PlayerFSMSystem(m_FSMData);
        m_FSMData.m_PlayerFSMSystem = m_PlayerFSM;

        PlayerFSMGunState m_GunState = new PlayerFSMGunState();
        PlayerFSMReloadState m_RelodaState = new PlayerFSMReloadState();
        PlayerFSMStratagemState m_StratagemState = new PlayerFSMStratagemState();
        PlayerFSMThrowState m_ThrowState = new PlayerFSMThrowState();


        m_GunState.AddTransition(ePlayerFSMTrans.Go_Stratagem, m_StratagemState);
        m_GunState.AddTransition(ePlayerFSMTrans.Go_Reload, m_RelodaState);

        m_RelodaState.AddTransition(ePlayerFSMTrans.Go_Gun, m_GunState);

        m_StratagemState.AddTransition(ePlayerFSMTrans.Go_Throw, m_ThrowState);
        m_StratagemState.AddTransition(ePlayerFSMTrans.Go_Gun, m_GunState);

        m_ThrowState.AddTransition(ePlayerFSMTrans.Go_Gun, m_GunState);

        m_PlayerFSM.AddState(m_GunState);
        m_PlayerFSM.AddState(m_RelodaState);
        m_PlayerFSM.AddState(m_StratagemState);
        m_PlayerFSM.AddState(m_ThrowState);

        #endregion
    }

    private void FixedUpdate()
    {
        SelectMotionState();
        m_PlayerFSM.DoState();
    }

    #endregion MonoBehaviour

    #region Character Behaviour

    private void SelectMotionState()
    {
        if (m_FSMData.m_NowAnimation.Equals("Origin"))
        {
            BasicMove();
            return;
        }
        if (m_FSMData.m_NowAnimation.Equals("Stratagem"))
        {
            m_PAC.Move(Vector3.zero, this.transform.forward, false, false);
            return;
        }
        else if (m_FSMData.m_NowAnimation.Equals("Throw"))
        {
            ThrowMove();
            return;
        }
        else if (m_FSMData.m_NowAnimation.Equals("Throwing"))
        {
            ThrowingMove();
            return;
        }
        return;
    }

    private void BasicMove()
    {
        Move();

        if (Input.GetButton("JoystickHorizontal") || Input.GetButton("JoystickVertical"))
        {
            FaceDirection();
            bInBattle = true;
        }
        else bInBattle = false;

        m_PAC.Move(m_Move, m_Direction, bRun, bInBattle);


    }

    private void ThrowMove()
    {
        Move();
        FaceDirection();
        bInBattle = true;
        bRun = false;

        m_PAC.Move(m_Move, m_Direction, bRun, bInBattle);
    }

    private void ThrowingMove()
    {
        Move();
        FaceDirection();
        bInBattle = true;
        bRun = false;

        m_PAC.Move(m_Move, this.transform.forward, bRun, bInBattle);
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (m_Cam != null)
        {
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            m_Move = v * Vector3.forward + h * Vector3.right;
        }
        if (m_Move.magnitude > 1) m_Move.Normalize();

        if (m_Controller.isGrounded == false)
        {
            m_Fall += Physics.gravity * Time.deltaTime;
        }
        m_Controller.Move(m_Fall);
    }

    private void FaceDirection()
    {
        float h = Input.GetAxis("JoystickHorizontal");
        float v = Input.GetAxis("JoystickVertical");

        if (m_Cam != null)
        {
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Direction = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            m_Direction = v * Vector3.forward + h * Vector3.right;
        }

        if (m_Direction.magnitude < 0.1f) return;
        if (m_Direction.magnitude > 1) m_Direction.Normalize();
        
    }

    #endregion Character Behaviour





#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2.0f);
    }

#endif
}
