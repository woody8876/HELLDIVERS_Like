using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyController : MonoBehaviour
{
    /// <summary>
    /// Set supply requester spawn position. (Right hand / Left hand)
    /// </summary>
    public Transform LaunchPos { get { return _LaunchPos; } set { _LaunchPos = value; } }

    public GameObject Display { get { return _Display; } set { _Display = value; } }

    [SerializeField] private GameObject _Display;
    [SerializeField] private Transform _LaunchPos;
    [SerializeField] private SupplyRequester[] _Requesters = new SupplyRequester[2];

    // Use this for initialization
    private void Start()
    {
        _LaunchPos = this.transform.Find("RightHand");
        if (_LaunchPos == null)
        {
            _LaunchPos = this.transform;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("SupplyRequest"))
        {
            _CkeckInput = StartCoroutine(CheckInputCode());
        }

        if (Input.GetButtonUp("SupplyRequest"))
        {
            if (_CkeckInput != null)
            {
                StopCoroutine(_CkeckInput);
            }
        }
    }

    /*-----------------------------------------
     * Check input key to the requesters codes.
     * Step by step.
     ------------------------------------------*/

    private IEnumerator CheckInputCode()
    {
        _OpenList.Clear();

        foreach (SupplyRequester s in _Requesters)
        {
            if (s != null)
            {
                _OpenList.Add(s);
            }
        }

        int inputCount = 0;
        ERequestCode? input = null;
        while (_OpenList.Count > 0)
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

            for (int i = 0; i < _OpenList.Count; i++)
            {
                if (_OpenList[i].Data.actCode[inputCount - 1] == input)
                {
                    if (_OpenList[i].Data.actCode.Length == inputCount)
                    {
                        Instantiate(_OpenList[i].gameObject, _LaunchPos);
                        yield break;
                    }
                    continue;
                }
                else
                {
                    _OpenList.RemoveAt(i);
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

    private Coroutine _CkeckInput;
    private List<SupplyRequester> _OpenList = new List<SupplyRequester>();
}