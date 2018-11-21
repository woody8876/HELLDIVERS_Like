using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI
{
    public class UIPlayerRewardDetail : MonoBehaviour
    {
        [SerializeField] private Text m_textLabel;
        [SerializeField] private Text m_textNumber;
        private int m_iNumber;

        public void Initialize(string label, int number)
        {
            m_textLabel.text = label;
            m_iNumber = number;
            m_textNumber.text = m_iNumber.ToString();
        }

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}