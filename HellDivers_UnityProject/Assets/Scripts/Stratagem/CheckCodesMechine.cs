using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCode
{
    Up, Down, Left, Right
}

public class CheckCodesMechine : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// Codes map in the processing.
    /// </summary>
    public Dictionary<Object, eCode[]> CheckCodeMap { get { return m_CodeMap; } }

    /// <summary>
    /// Repersent of the mehcine is checking code.
    /// </summary>
    public bool IsChecking { get; private set; }

    /// <summary>
    /// Repersent of the step on checking code.
    /// </summary>
    public int Step { get; private set; }

    /// <summary>
    /// Get the object which's codes all fit with inputs.
    /// </summary>
    public Object Result { get; private set; }

    #endregion Properties

    #region Events

    public delegate void CheckCodeEventHolder();

    /// <summary>
    /// On get result success.
    /// </summary>
    public event CheckCodeEventHolder OnGetResult;

    /// <summary>
    /// On the begining of check code process.
    /// </summary>
    public event CheckCodeEventHolder OnStart;

    /// <summary>
    /// On the check codes process. Call back once get an input.
    /// </summary>
    public event CheckCodeEventHolder OnChecking;

    /// <summary>
    /// On the end of check code process.
    /// </summary>
    public event CheckCodeEventHolder OnStop;

    /// <summary>
    /// On the failed in check code process.
    /// </summary>
    public event CheckCodeEventHolder OnFaild;

    /// <summary>
    /// Input vertical , get Up, Down.
    /// </summary>
    public string InputVertical { get { return m_InputVertical; } set { m_InputVertical = value; } }

    /// <summary>
    /// Input horizontal , get Left, Right.
    /// </summary>
    public string InputHorizontal { get { return m_InputHorizontal; } set { m_InputHorizontal = value; } }

    #endregion Events

    #region Input Define

    private string m_InputVertical = "StratagemVertical";
    private string m_InputHorizontal = "StratagemHorizontal";

    #endregion Input Define

    #region Private Variable

    private Dictionary<Object, eCode[]> m_CodeMap;

    // A temporary container collect the object which's codes not fit inputs.
    private List<Object> m_Close;

    #endregion Private Variable

    #region MonoBehaviour

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

    #endregion MonoBehaviour

    #region Public Function

    #region Management

    /// <summary>
    /// Add target codes and the owner who want to check codes.
    /// </summary>
    /// <param name="o">The codes owner</param>
    /// <param name="codes">The codes for checking</param>
    public void AddCodes(Object o, eCode[] codes)
    {
        if (m_CodeMap.ContainsKey(o)) return;
        m_CodeMap.Add(o, codes);
    }

    /// <summary>
    /// Remove check code target.
    /// </summary>
    /// <param name="o"></param>
    public void RemoveCodes(Object o)
    {
        if (m_CodeMap.ContainsKey(o))
        {
            m_CodeMap.Remove(o);
        }
    }

    /// <summary>
    /// Clear all codes and process.
    /// </summary>
    public void Clear()
    {
        m_CodeMap.Clear();
    }

    #endregion Management

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

    #region Check Code Process

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
                        if (OnGetResult != null) OnGetResult();
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
        if (OnFaild != null) OnFaild();
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

    #endregion Check Code Process
}