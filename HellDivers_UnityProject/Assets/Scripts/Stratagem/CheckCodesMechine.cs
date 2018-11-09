using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCode
{
    Up, Down, Left, Right
}

public class CheckCodesMechine : MonoBehaviour
{
    public Dictionary<Object, eCode[]> CheckCodeMap { get { return m_CodeMap; } }
    public bool IsChecking { get; private set; }
    public int Step { get; private set; }
    public Object Result { get; private set; }

    public delegate void CheckCodeEventHolder();

    public event CheckCodeEventHolder OnGet;

    public event CheckCodeEventHolder OnStart;

    public event CheckCodeEventHolder OnChecking;

    public event CheckCodeEventHolder OnStop;

    private string m_InputVertical = "StratagemVertical";
    private string m_InputHorizontal = "StratagemHorizontal";
    private Dictionary<Object, eCode[]> m_CodeMap;
    private List<Object> m_Close;

    private void Awake()
    {
        m_CodeMap = new Dictionary<Object, eCode[]>();
        m_Close = new List<Object>();
        IsChecking = false;
        Step = 0;
    }

    private void Start()
    {
        this.enabled = false;
    }

    #region Management

    public void AddCodes(Object o, eCode[] codes)
    {
        if (m_CodeMap.ContainsKey(o)) return;
        m_CodeMap.Add(o, codes);
    }

    public void RemoveCodes(Object o)
    {
        if (m_CodeMap.ContainsKey(o))
        {
            m_CodeMap.Remove(o);
        }
    }

    public void Clear()
    {
        m_CodeMap.Clear();
    }

    #endregion Management

    #region Public Function

    public void StartCheckCodes()
    {
        StopAllCoroutines();

        if (m_CodeMap.Count > 0)
        {
            this.enabled = true;
            StartCoroutine(CheckCodes());
        }

        if (OnStart != null) OnStart();
    }

    public void StopCheckCodes()
    {
        StopAllCoroutines();
        IsChecking = false;
        this.enabled = false;

        if (OnStop != null) OnStop();
    }

    #endregion Public Function

    #region Check Code

    private IEnumerator CheckCodes()
    {
        IsChecking = true;
        Step = 0;
        m_Close.Clear();
        eCode? input = null;

        while (m_CodeMap.Count > 0)
        {
            yield return new WaitUntil(() => { return (input = GetInput()) == null; });
            yield return new WaitUntil(() => { return (input = GetInput()) != null; });
            Step++;

            foreach (KeyValuePair<Object, eCode[]> element in m_CodeMap)
            {
                int index = Step - 1;
                if (element.Value[index] == input)
                {
                    if (element.Value.Length == Step)
                    {
                        Result = element.Key;
                        IsChecking = false;
                        this.enabled = false;
                        if (OnGet != null) OnGet();
                        yield break;
                    }
                    else continue;
                }
                else
                {
                    m_Close.Add(element.Key);
                }
            }

            foreach (Object o in m_Close)
            {
                m_CodeMap.Remove(o);
            }

            if (OnChecking != null) OnChecking();
        }

        IsChecking = false;
        yield break;
    }

    private eCode? GetInput()
    {
        if (Input.GetAxisRaw(m_InputVertical) == 1) { return eCode.Up; }
        else if (Input.GetAxisRaw(m_InputVertical) == -1) { return eCode.Down; }
        else if (Input.GetAxisRaw(m_InputHorizontal) == -1) { return eCode.Left; }
        else if (Input.GetAxisRaw(m_InputHorizontal) == 1) { return eCode.Right; }
        else { return null; }
    }

    #endregion Check Code
}