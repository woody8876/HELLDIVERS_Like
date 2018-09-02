using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyController : MonoBehaviour
{
    [SerializeField] private GameObject source;

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
            core = StartCoroutine(CheckInputCode());
        }

        if (Input.GetButtonUp("SupplyRequest"))
        {
            StopCoroutine(core);
            Debug.Log("Stop check code input.");
        }
    }

    private IEnumerator CheckInputCode()
    {
        Debug.Log("Start check code input.");

        ERequestCode? input = null;
        codeInputCount = 0;
        Open.Clear();
        foreach (SupplyRequester s in requesters)
        {
            Open.Add(s);
        }
        yield return new WaitUntil(() => { return GetInputCode(out input); });

        codeInputCount++;
        Debug.Log("Get input succeed.");
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

    private Coroutine core;
    private int codeInputCount;
    private List<SupplyRequester> Open = new List<SupplyRequester>();

    [SerializeField] private SupplyRequester[] requesters;
}