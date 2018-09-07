using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyController : MonoBehaviour
{
    public Transform LaunchPos { get { return m_tLaunchPos; } set { m_tLaunchPos = value; } }
    public GameObject Display { get { return m_gDisplay; } set { m_gDisplay = value; } }

    [SerializeField] private GameObject m_gDisplay;
    [SerializeField] private Transform m_tLaunchPos;
    [SerializeField] private SupplyRequester[] m_Requesters = new SupplyRequester[2];

    // Use this for initialization
    private void Start()
    {
        m_tLaunchPos = this.transform.Find("RightHand");
        if (m_tLaunchPos == null)
        {
            m_tLaunchPos = this.transform;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("SupplyRequest"))
        {
            m_cCkeckInput = StartCoroutine(CheckInputCode());
        }

        if (Input.GetButtonUp("SupplyRequest"))
        {
            if (m_cCkeckInput != null)
            {
                StopCoroutine(m_cCkeckInput);
            }
        }
    }

    /*-----------------------------------------
     * Check input key to the requesters codes.
     * Step by step.
     ------------------------------------------*/

    private IEnumerator CheckInputCode()
    {
        m_OpenList.Clear();

        foreach (SupplyRequester s in m_Requesters)
        {
            if (s != null)
            {
                m_OpenList.Add(s);
            }
        }

        int inputCount = 0;
        ERequestCode? input = null;
        while (m_OpenList.Count > 0)
        {
            yield return new WaitUntil(() =>
            {
                GetInputCode(out input);
                return input == null;
            });

            yield return new WaitUntil(() =>
            {
                return GetInputCode(out input);
            });

            inputCount++;

            for (int i = 0; i < m_OpenList.Count; i++)
            {
                if (m_OpenList[i].Data.actCode[inputCount - 1] == input)
                {
                    if (m_OpenList[i].Data.actCode.Length == inputCount)
                    {
                        Instantiate(m_OpenList[i].gameObject, m_tLaunchPos);
                        yield break;
                    }
                    continue;
                }
                else
                {
                    m_OpenList.RemoveAt(i);
                }
            }
        }
    }

    /*---------------------------
     * Get request code by input.
     ----------------------------*/

    private bool GetInputCode(out ERequestCode? input)
    {
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            input = ERequestCode.Up;
            return true;
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            input = ERequestCode.Down;
            return true;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            input = ERequestCode.Left;
            return true;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            input = ERequestCode.Right;
            return true;
        }
        else
        {
            input = null;
            return false;
        }
    }

    private Coroutine m_cCkeckInput;
    private List<SupplyRequester> m_OpenList = new List<SupplyRequester>();
}