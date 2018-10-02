using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratagemController : MonoBehaviour
{
    [SerializeField] private List<Stratagem> m_Stratagems;

    // Use this for initialization
    private void Start()
    {
        Player p = GetComponent<Player>();

        // Init form player info.
        if (p.Info.StratagemId != null)
        {
            m_Stratagems = new List<Stratagem>();

            for (int i = 0; i < p.Info.StratagemId.Count; i++)
            {
                GameObject go = new GameObject(string.Format("Stratagem ({0})", i));
                Stratagem s = go.AddComponent<Stratagem>();
                s.SetStratagemInfo(p.Info.StratagemId[i], p.Parts.RightHand);
                m_Stratagems.Add(s);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Stratagem"))
        {
            StartCoroutine(CheckInputCode());
        }
        else if (Input.GetButtonUp("Stratagem"))
        {
            StopCoroutine(CheckInputCode());
        }
    }

    private IEnumerator CheckInputCode()
    {
        _Open.Clear();

        foreach (Stratagem s in m_Stratagems)
        {
            if (s != null && s.State == Stratagem.EState.Standby)
                _Open.Add(s);
        }

        int inputCount = 0;
        StratagemInfo.eCode? input = null;
        while (_Open.Count > 0)
        {
            yield return new WaitUntil(() => { return (input = GetInputCode()) == null; });
            yield return new WaitUntil(() => { return (input = GetInputCode()) != null; });
            inputCount++;

            for (int i = 0; i < _Open.Count; i++)
            {
                if (_Open[i].Info.code[inputCount - 1] == input)
                {
                    if (_Open[i].Info.code.Length == inputCount)
                    {
                        _Open[i].GetReady();
                        yield break;
                    }
                    continue;
                }
                else
                { _Open.RemoveAt(i); }
            }
        }
    }

    private StratagemInfo.eCode? GetInputCode()
    {
        if (Input.GetAxisRaw("Vertical") > 0) { return StratagemInfo.eCode.Up; }
        else if (Input.GetAxisRaw("Vertical") < 0) { return StratagemInfo.eCode.Down; }
        else if (Input.GetAxisRaw("Horizontal") < 0) { return StratagemInfo.eCode.Left; }
        else if (Input.GetAxisRaw("Horizontal") > 0) { return StratagemInfo.eCode.Right; }
        else { return null; }
    }

    private List<Stratagem> _Open = new List<Stratagem>();
}