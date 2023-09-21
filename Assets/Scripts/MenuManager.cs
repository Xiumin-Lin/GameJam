using System.Collections;
using System.Collections.Generic;
using SDD.Events;
using UnityEngine;

namespace STUDENT_NAME
{
    public class MenuManager : Manager<MenuManager>
    {
        #region Manager implementation

        protected override IEnumerator InitCoroutine()
        {
            yield break;
        }

        #endregion

        [Header("MenuManager")]

        #region Panels

        [Header("Panels")]
        [SerializeField]
        private GameObject m_PanelMainMenu;

        [SerializeField] private GameObject m_PanelInGameMenu;
        [SerializeField] private GameObject m_PanelGameOver;

        private List<GameObject> m_AllPanels;

        #endregion

        #region Events' subscription

        public override void SubscribeEvents()
        {
            base.SubscribeEvents();
        }

        public override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
        }

        #endregion

        #region Monobehaviour lifecycle

        protected override void Awake()
        {
            base.Awake();
            RegisterPanels();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Cancel")) EscapeButtonHasBeenClicked();
        }

        #endregion

        #region Panel Methods

        private void RegisterPanels()
        {
            m_AllPanels = new List<GameObject>();
            m_AllPanels.Add(m_PanelMainMenu);
            m_AllPanels.Add(m_PanelInGameMenu);
            m_AllPanels.Add(m_PanelGameOver);
        }

        private void OpenPanel(GameObject panel)
        {
            foreach (var item in m_AllPanels)
                if (item)
                    item.SetActive(item == panel);
        }

        #endregion

        #region UI OnClick Events

        public void EscapeButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new EscapeButtonClickedEvent());
        }

        public void PlayButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new PlayButtonClickedEvent());
        }

        public void ResumeButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new ResumeButtonClickedEvent());
        }

        public void MainMenuButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
        }

        public void QuitButtonHasBeenClicked()
        {
            EventManager.Instance.Raise(new QuitButtonClickedEvent());
        }

        #endregion

        #region Callbacks to GameManager events

        protected override void GameMenu(GameMenuEvent e)
        {
            OpenPanel(m_PanelMainMenu);
        }

        protected override void GamePlay(GamePlayEvent e)
        {
            OpenPanel(null);
        }

        protected override void GamePause(GamePauseEvent e)
        {
            OpenPanel(m_PanelInGameMenu);
        }

        protected override void GameResume(GameResumeEvent e)
        {
            OpenPanel(null);
        }

        protected override void GameOver(GameOverEvent e)
        {
            OpenPanel(m_PanelGameOver);
        }

        #endregion
    }
}