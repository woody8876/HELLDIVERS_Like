using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimationsContorller))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
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

    #endregion Private Variable

    public PlayerFSMSystem m_PlayerFSM;
    public float m_fAnimatorTime;
    public bool bIsDead = false;

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

        #region PlayerFSMMap

        m_FSMData = new PlayerFSMData();
        m_PlayerFSM = new PlayerFSMSystem(m_FSMData);
        m_FSMData.m_PlayerFSMSystem = m_PlayerFSM;
        m_FSMData.m_PlayerController = this;
        m_FSMData.m_AnimationController = m_PAC;
        m_FSMData.m_Animator = m_PAC.Animator;
        m_FSMData.m_WeaponController = GetComponent<WeaponController>();
        m_FSMData.m_StratagemController = GetComponent<StratagemController>();

        PlayerFSMGunState m_GunState = new PlayerFSMGunState();
        PlayerFSMReloadState m_RelodaState = new PlayerFSMReloadState();
        PlayerFSMStratagemState m_StratagemState = new PlayerFSMStratagemState();
        PlayerFSMThrowState m_ThrowState = new PlayerFSMThrowState();
        PlayerFSMSwitchWeaponState m_SwitchWeaponState = new PlayerFSMSwitchWeaponState();
        PlayerFSMPickUpState m_PickUpState = new PlayerFSMPickUpState();


        m_GunState.AddTransition(ePlayerFSMTrans.Go_Reload, m_RelodaState);
        m_GunState.AddTransition(ePlayerFSMTrans.Go_Stratagem, m_StratagemState);
        m_GunState.AddTransition(ePlayerFSMTrans.Go_SwitchWeapon, m_SwitchWeaponState);
        m_GunState.AddTransition(ePlayerFSMTrans.Go_PickUp, m_PickUpState);

        m_RelodaState.AddTransition(ePlayerFSMTrans.Go_Gun, m_GunState);

        m_StratagemState.AddTransition(ePlayerFSMTrans.Go_Throw, m_ThrowState);
        m_StratagemState.AddTransition(ePlayerFSMTrans.Go_Gun, m_GunState);

        m_SwitchWeaponState.AddTransition(ePlayerFSMTrans.Go_Gun, m_GunState);

        m_PickUpState.AddTransition(ePlayerFSMTrans.Go_Gun, m_GunState);

        m_ThrowState.AddTransition(ePlayerFSMTrans.Go_Gun, m_GunState);

        PlayerFSMDeadState m_DeadState = new PlayerFSMDeadState();
        
        m_DeadState.AddTransition(ePlayerFSMTrans.Go_Gun, m_GunState);

        m_PlayerFSM.AddGlobalTransition(ePlayerFSMTrans.Go_Dead, m_DeadState);

        m_PlayerFSM.AddState(m_GunState);
        m_PlayerFSM.AddState(m_RelodaState);
        m_PlayerFSM.AddState(m_StratagemState);
        m_PlayerFSM.AddState(m_ThrowState);
        m_PlayerFSM.AddState(m_DeadState);


        #endregion
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            PerformPlayerDead();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            PerformPlayerHurt();
        }
        
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
        else if (m_FSMData.m_NowAnimation.Equals("Stratagem"))
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
        else if (m_FSMData.m_NowAnimation.Equals("Dead"))
        {
            return;
        }
    }

    private void BasicMove()
    {
        Move();

        if (Input.GetButton("Run")) bRun = true;
        else bRun = false;

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            FaceDirection();
            bInBattle = true;
        }

        #region Joystick
        //if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        //{
        //    bRun = false;
        //}
        else if (Input.GetAxis("DirectionHorizontal") != 0 || Input.GetAxis("DirectionVertical") != 0)
        {
            FaceDirection();
            bInBattle = true;
        }
        else bInBattle = false;
        #endregion
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
        float vHigh = Camera.main.transform.position.y - this.transform.position.y;
        Ray MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 vMouseRay = MouseRay.direction;
        vMouseRay.Normalize();

        float angle = Vector3.Dot(new Vector3(0, -1, 0), vMouseRay);
        float distance = vHigh / angle;
        Vector3 endPoint = MouseRay.GetPoint(distance);

        m_Direction = endPoint - this.transform.position;
        m_Direction.y = 0.0f;
        if (m_Direction.magnitude < 0.1f) return;
        if (m_Direction.magnitude > 1) m_Direction.Normalize();

        if (Input.GetAxis("DirectionHorizontal") != 0 || Input.GetAxis("DirectionVertical") != 0)
        {
            #region Joystick
            float h = Input.GetAxis("DirectionHorizontal");
            float v = Input.GetAxis("DirectionVertical");
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
            #endregion
        }



    }

    public void PerformPlayerDead()
    {
        m_PlayerFSM.PerformGlobalTransition(ePlayerFSMTrans.Go_Dead);
    }

    public bool PerformPlayerHurt()
    {
        AnimatorStateInfo info = m_FSMData.m_Animator.GetCurrentAnimatorStateInfo(2);
        if (info.IsName("GetGurt"))
        {
            return false;
        }
        m_FSMData.m_Animator.SetTrigger("GetHurt");
        return true;
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