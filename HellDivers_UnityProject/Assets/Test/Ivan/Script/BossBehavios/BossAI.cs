using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour {

    public EnemyData m_data;
    BossFSMSystem m_BFS;
    BossStateFuntion m_BSF;

	// Use this for initialization
	void Awake () {
        m_BFS = new BossFSMSystem(m_data);
        m_BSF = new BossStateFuntion();
        m_BSF.Init();
        m_data.m_Go = this.gameObject;
        m_data.m_bossFSMSystem = m_BFS;
        #region Init FSM State
        BossIdleState idleState = new BossIdleState();
        BossSeekState seekState = new BossSeekState();
        BossDrawAlertState drawAlertState = new BossDrawAlertState();
        BossRushState rushState = new BossRushState();
        BossMissleState missleState = new BossMissleState();
        BossEarthquakeState earthquakeState = new BossEarthquakeState();
        #endregion
        #region Set FSM Map
        idleState.AddTransition(BossFSM.eFSMTransition.G0_SEEK, seekState);

        seekState.AddTransition(BossFSM.eFSMTransition.GO_DRAWALERT, drawAlertState);

        drawAlertState.AddTransition(BossFSM.eFSMTransition.G0_RUSH, rushState);
        drawAlertState.AddTransition(BossFSM.eFSMTransition.GO_MISSILE, missleState);
        drawAlertState.AddTransition(BossFSM.eFSMTransition.GO_EARTHQIAKE, earthquakeState);

        rushState.AddTransition(BossFSM.eFSMTransition.GO_IDLE, idleState);

        missleState.AddTransition(BossFSM.eFSMTransition.G0_SEEK, seekState);

        earthquakeState.AddTransition(BossFSM.eFSMTransition.GO_IDLE, idleState);


        #endregion

        m_BFS.AddState(idleState);
        m_BFS.AddState(seekState);
        m_BFS.AddState(drawAlertState);
        m_BFS.AddState(rushState);
        m_BFS.AddState(missleState);
        m_BFS.AddState(earthquakeState);
    }

    // Update is called once per frame
    void Update () {
        m_BFS.DoState();
	}
}
