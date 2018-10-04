using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratagemController : MonoBehaviour
{
    [SerializeField] private List<Stratagem> m_Stratagems;
    private Stratagem m_CurrentStratagem;

    #region MonoBehaviour

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
                // Creat stratagem with player info.
                AddNewStratagemObject(p.Info.StratagemId[i], p.Parts.RightHand);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (m_CurrentStratagem != null)
            {
                m_CurrentStratagem.Throw(new Vector3(0.0f, 500.0f, 300.0f));
                m_CurrentStratagem = null;
            }
        }

        if (Input.GetButtonDown("Stratagem"))
        {
            StartCoroutine(CheckInputCode());
        }
        else if (Input.GetButtonUp("Stratagem"))
        {
            StopCoroutine(CheckInputCode());
        }
    }

    #endregion MonoBehaviour

    #region Public Function

    /// <summary>
    /// Add a stratagem object by id key which is in the gamedata.stratagem table.
    /// If the id already in the controller will be pass.
    /// </summary>
    /// <param name="id">The id key which is in the gamedata.stratagem table.</param>
    /// <param name="launchPos">The spawn transform root.</param>
    public void AddNewStratagemObject(int id, Transform launchPos)
    {
        for (int i = 0; i < m_Stratagems.Count; i++)
        {
            if (m_Stratagems[i].Info.id == id) return;
        }

        GameObject go = new GameObject(string.Format("Stratagem ({0})", id));
        Stratagem s = go.AddComponent<Stratagem>();
        s.SetStratagemInfo(id, launchPos);
        m_Stratagems.Add(s);
    }

    #endregion Public Function

    #region Check Input Code

    /*---------------------------------------------------------
     * Cllect all stratagem which has input code.             *
     * Check input with info step by step.                    *
     * The final result have to all match up info with input. *
     * Finaly store in the m_CurrentStratagem.                *
     ---------------------------------------------------------*/

    private IEnumerator CheckInputCode()
    {
        _Open.Clear();

        foreach (Stratagem s in m_Stratagems)
        {
            if (s != null && s.Info != null && s.State == Stratagem.eState.Idle)
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
                        m_CurrentStratagem = _Open[i];
                        m_CurrentStratagem.GetReady();
                        yield break;
                    }
                    continue;
                }
                else
                { _Open.RemoveAt(i); }
            }
        }
    }

    /*---------------------------
     * Define the input result. *
     ---------------------------*/

    private StratagemInfo.eCode? GetInputCode()
    {
        if (Input.GetAxisRaw("Vertical") > 0) { return StratagemInfo.eCode.Up; }
        else if (Input.GetAxisRaw("Vertical") < 0) { return StratagemInfo.eCode.Down; }
        else if (Input.GetAxisRaw("Horizontal") < 0) { return StratagemInfo.eCode.Left; }
        else if (Input.GetAxisRaw("Horizontal") > 0) { return StratagemInfo.eCode.Right; }
        else { return null; }
    }

    private List<Stratagem> _Open = new List<Stratagem>();

    #endregion Check Input Code
}