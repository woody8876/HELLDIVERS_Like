using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HELLDIVERS.UI.InGame
{
    public class UIStratagemInfo : MonoBehaviour
    {
        public Player CurrentPlayer { get; private set; }
        public Stratagem CurrentStratagem { get; private set; }

        [SerializeField] private Image m_ImgIcon;
        [SerializeField] private Text m_TextUses;
        [SerializeField] private Text m_TextTitle;
        [SerializeField] private Transform m_ArrowsGroup;
        [SerializeField] private Image m_ImgArrow;
        [SerializeField] private Color m_BrightColor;
        [SerializeField] private Color m_DarkColor;
        private Animator m_Animator;
        private List<Image> m_ArrowsMap = new List<Image>();

        public void Init(Player player, Stratagem stratagem)
        {
            CurrentPlayer = player;
            CurrentStratagem = stratagem;

            string fileName = string.Format("icon_{0}", CurrentStratagem.Info.ID);
            m_ImgIcon.sprite = ResourceManager.m_Instance.LoadSprite(typeof(Sprite), UIHelper.StratagemIconFolder, fileName);

            m_TextTitle.text = stratagem.Info.Title;
            m_TextUses.text = stratagem.Info.Uses.ToString();

            CreateArrows();
            SubscribePlayerEvent();
        }

        private void CreateArrows()
        {
            for (int i = 0; i < CurrentStratagem.Info.Codes.Length; i++)
            {
                Image arrow = Instantiate(m_ImgArrow, m_ArrowsGroup).GetComponent<Image>();

                switch (CurrentStratagem.Info.Codes[i])
                {
                    case StratagemInfo.eCode.Up:
                        arrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                        break;

                    case StratagemInfo.eCode.Down:
                        arrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                        break;

                    case StratagemInfo.eCode.Left:
                        arrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                        break;

                    case StratagemInfo.eCode.Right:
                        arrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        break;
                }

                m_ArrowsMap.Add(arrow);
            }

            m_ImgArrow.gameObject.SetActive(false);
        }

        private void Awake()
        {
            m_Animator = this.GetComponent<Animator>();
        }

        private void OnDestroy()
        {
            UnsubscribePlayerEvent();
        }

        private void SubscribePlayerEvent()
        {
            CurrentPlayer.StratagemController.OnStartCheckingCode += StartUI;
            CurrentPlayer.StratagemController.OnCheckingCode += DoCheckCodes;
            CurrentPlayer.StratagemController.OnGetReady += DoReady;
            CurrentPlayer.StratagemController.OnStopCheckingCode += StopUI;
            CurrentPlayer.OnStartDeathNotify += StopUI;
            CurrentStratagem.OnThrow += StopUI;
        }

        private void UnsubscribePlayerEvent()
        {
            CurrentPlayer.StratagemController.OnStartCheckingCode -= StartUI;
            CurrentPlayer.StratagemController.OnCheckingCode -= DoCheckCodes;
            CurrentPlayer.StratagemController.OnGetReady -= DoReady;
            CurrentPlayer.StratagemController.OnStopCheckingCode -= StopUI;
            CurrentPlayer.OnStartDeathNotify -= StopUI;
            CurrentStratagem.OnThrow -= StopUI;
        }

        private void StartUI()
        {
            this.gameObject.SetActive(true);
            m_ArrowsGroup.gameObject.SetActive(true);
            m_TextTitle.gameObject.SetActive(false);
            m_ImgIcon.color = m_BrightColor;
            RefershUses();
            DoCheckCodes();
            m_Animator.SetTrigger("Open");
        }

        private void StopUI()
        {
            this.gameObject.SetActive(false);
        }

        private void DoCheckCodes()
        {
            foreach (Image img in m_ArrowsMap)
            {
                img.color = m_DarkColor;
            }

            if (CurrentPlayer.StratagemController.StratagemsOnCheckingCode.Contains(CurrentStratagem))
            {
                for (int i = 0; i < CurrentPlayer.StratagemController.InputCodeStep; i++)
                {
                    m_ArrowsMap[i].color = m_BrightColor;
                }
            }
            else
            {
                m_ImgIcon.color = m_DarkColor;
            }
        }

        private void DoReady()
        {
            if (CurrentPlayer.StratagemController.CurrentStratagem == CurrentStratagem)
            {
                m_ArrowsGroup.gameObject.SetActive(false);
                m_TextTitle.gameObject.SetActive(true);
                RefershUses();
            }
            else
            {
                StopUI();
            }
        }

        private void RefershUses()
        {
            int uses = CurrentStratagem.Info.Uses - CurrentStratagem.UsesCount;
            m_TextUses.text = uses.ToString();
        }
    }
}