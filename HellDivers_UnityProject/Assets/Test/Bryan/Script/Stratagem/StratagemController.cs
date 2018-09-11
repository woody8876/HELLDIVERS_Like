using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratagemController : MonoBehaviour
{
    private Transform LaunchPos { get; set; }
    private GameObject _Display;
    private StratagemData[] datas = new StratagemData[2];

    public bool SetStratage(int index, StratagemData data)
    {
        datas[index] = data;
        return true;
    }

    // Use this for initialization
    private void Start()
    {
        LaunchPos = this.transform.Find("RightHand");
        if (LaunchPos == null) LaunchPos = this.transform;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private IEnumerator CheckInputCode()
    {
        _Open.Clear();

        foreach (StratagemData s in datas)
        {
            if (s != null) _Open.Add(s);
        }

        int inputCount = 0;
        StratagemCode? input = null;
        while (_Open.Count > 0)
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

            for (int i = 0; i < _Open.Count; i++)
            {
                if (_Open[i].Code[inputCount - 1] == input)
                {
                    if (_Open[i].Code.Length == inputCount)
                    {
                        Instantiate(_Display, LaunchPos);
                        yield break;
                    }
                    continue;
                }
                else
                {
                    _Open.RemoveAt(i);
                }
            }
        }
    }

    private bool GetInputCode(out StratagemCode? input)
    {
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            input = StratagemCode.Up;
            return true;
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            input = StratagemCode.Down;
            return true;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            input = StratagemCode.Left;
            return true;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            input = StratagemCode.Right;
            return true;
        }
        else
        {
            input = null;
            return false;
        }
    }

    private List<StratagemData> _Open = new List<StratagemData>();
}