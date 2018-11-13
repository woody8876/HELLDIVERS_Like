using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPackage : InteractiveItem
{
    public int WeaponID { get { return m_WeaponID; } }
    [SerializeField] private int m_WeaponID;
    private Animator m_Animator;

    private void Start()
    {
        m_Animator = this.GetComponent<Animator>();
    }

    public void Initialize(int weaponId)
    {
        m_WeaponID = weaponId;
    }

    public override void OnInteract(Player player)
    {
        m_WeaponID = player.WeaponController._CurrentWeapon;
        int[] ids = player.WeaponController.ActivedWeaponID;
        bool bFillUp = false;
        for (int i = 0; i < ids.Length; i++)
        {
            int currentWeaponId = player.WeaponController.ActivedWeaponID[i];
            int num = Mathf.RoundToInt(player.WeaponController.ActiveWeapon[currentWeaponId].weaponInfo.Start_Mags);
            bFillUp = player.WeaponController.AddMags(currentWeaponId, num);
        }
        if (bFillUp == false) return;

        m_Animator.SetTrigger("Open");
        StartCoroutine(DoDestory());
    }

    private IEnumerator DoDestory()
    {
        yield return new WaitUntil(CheckEndAnimation);
        Destroy(this.gameObject);
        yield break;
    }

    private bool CheckEndAnimation()
    {
        if (m_Animator.IsInTransition(0)) return false;
        AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Open") && stateInfo.normalizedTime >= 1) return true;
        return false;
    }
}