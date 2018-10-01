using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratagemController : MonoBehaviour
{
    private Player m_Player;
    private PlayerInfo m_playerInfo;
    private GameObject m_Display = null;

    private List<Stratagem> m_Stratagems;

    // Use this for initialization
    private void Start()
    {
        m_Player = GetComponent<Player>();
        if (m_Player != null)
        {
            m_playerInfo = m_Player.Info;

            if (m_Player.Info.StratagemId != null)
            {
                m_Stratagems = new List<Stratagem>();

                for (int i = 0; i < m_Player.Info.StratagemId.Count; i++)
                {
                    GameObject go = new GameObject("Stratagem");
                    Stratagem s = go.AddComponent<Stratagem>();
                    s.SetStratagemInfo(m_Player.Info.StratagemId[i], m_Player.Parts.RightHand);
                    m_Stratagems.Add(s);
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private IEnumerator CheckInputCode()
    {
        _Open.Clear();

        foreach (Stratagem s in m_Stratagems)
        {
            if (s != null)
                _Open.Add(s);
        }

        int inputCount = 0;
        StratagemInfo.eCode? input = null;
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
                if (_Open[i].Info.code[inputCount - 1] == input)
                {
                    if (_Open[i].Info.code.Length == inputCount)
                    {
                        /// Show Stratagem Agent
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

    private bool GetInputCode(out StratagemInfo.eCode? input)
    {
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            input = StratagemInfo.eCode.Up;
            return true;
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            input = StratagemInfo.eCode.Down;
            return true;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            input = StratagemInfo.eCode.Left;
            return true;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            input = StratagemInfo.eCode.Right;
            return true;
        }
        else
        {
            input = null;
            return false;
        }
    }

    private List<Stratagem> _Open = new List<Stratagem>();
}