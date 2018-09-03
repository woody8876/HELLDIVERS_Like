using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyController : MonoBehaviour
{
    [SerializeField] private GameObject source;
    [SerializeField] private SupplyRequester[] requesters;
    [SerializeField] private Transform launchPos;

    public void Init()
    {
        requesters = new SupplyRequester[2];
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("SupplyRequest"))
        {
            ckeckInputCore = StartCoroutine(CheckInputCode());
        }

        if (Input.GetButtonUp("SupplyRequest"))
        {
            StopCoroutine(ckeckInputCore);
        }
    }

    private IEnumerator CheckInputCode()
    {
        Open.Clear();

        foreach (SupplyRequester s in requesters)
        {
            Open.Add(s);
        }

        int codeInputCount = 0;
        bool GetSucceed = false;
        SupplyRequester SRActived = null;
        ERequestCode? input = null;

        while (Open.Count > 0 && GetSucceed == false)
        {
            yield return new WaitUntil(() =>
            {
                GetInputCode(out input);
                return input == null;
            });
            yield return new WaitUntil(() => { return GetInputCode(out input); });

            codeInputCount++;
            for (int i = 0; i < Open.Count; i++)
            {
                if (Open[i].Data.RequestCode[codeInputCount - 1] == input)
                {
                    if (Open[i].Data.RequestCode.Length == codeInputCount)
                    {
                        SRActived = Open[i];
                        GetSucceed = true;
                        Debug.Log("succeed.");

                        break;
                    }
                    continue;
                }
                else
                {
                    Open.RemoveAt(i);
                }
            }
        }

        if (SRActived != null)
        {
            Instantiate(SRActived.gameObject, launchPos);
        }
    }

    private bool GetInputCode(out ERequestCode? input)
    {
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            input = ERequestCode.Up;
            return true;
        }
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            input = ERequestCode.Down;
            return true;
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            input = ERequestCode.Left;
            return true;
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            input = ERequestCode.Right;
            return true;
        }

        input = null;
        return false;
    }

    private Coroutine ckeckInputCore;
    private List<SupplyRequester> Open = new List<SupplyRequester>();
}