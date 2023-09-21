using System.Collections;
using SDD.Events;
using UnityEngine;

namespace STUDENT_NAME
{
    public enum GameState
    {
        gameMenu,
        gamePlay,
        gameNextLevel,
        gamePause,
        gameOver,
        gameVictory
    }

    public class GameManager : Manager<GameManager>
    {
        #region Time

        private void SetTimeScale(float newTimeScale)
        {
            Time.timeScale = newTimeScale;
        }

        #endregion

        #region Manager implementation

        protected override IEnumerator InitCoroutine()
        {
            Menu();
            InitNewGame(); // essentiellement pour que les statistiques du jeu soient mise à jour en HUD
            yield break;
        }

        #endregion

        #region Game flow & Gameplay

        //Game initialization
        private void InitNewGame(bool raiseStatsEvent = true)
        {
            SetScore(0);
        }

        #endregion

        #region Callbacks to events issued by Score items

        private void ScoreHasBeenGained(ScoreItemEvent e)
        {
            if (IsPlaying)
                IncrementScore(e.eScore);
        }

        #endregion

        #region Game State

        private GameState m_GameState;
        public bool IsPlaying => m_GameState == GameState.gamePlay;

        #endregion

        //LIVES

        #region Lives

        [Header("GameManager")] [SerializeField]
        private int m_NStartLives;

        public int NLives { get; private set; }

        private void DecrementNLives(int decrement)
        {
            SetNLives(NLives - decrement);
        }

        private void SetNLives(int nLives)
        {
            NLives = nLives;
            EventManager.Instance.Raise(new GameStatisticsChangedEvent
                { eBestScore = BestScore, eScore = m_Score, eNLives = NLives });
        }

        #endregion


        #region Score

        private float m_Score;

        public float Score
        {
            get => m_Score;
            set
            {
                m_Score = value;
                BestScore = Mathf.Max(BestScore, value);
            }
        }

        public float BestScore
        {
            get => PlayerPrefs.GetFloat("BEST_SCORE", 0);
            set => PlayerPrefs.SetFloat("BEST_SCORE", value);
        }

        private void IncrementScore(float increment)
        {
            SetScore(m_Score + increment);
        }

        private void SetScore(float score, bool raiseEvent = true)
        {
            Score = score;

            if (raiseEvent)
                EventManager.Instance.Raise(new GameStatisticsChangedEvent
                    { eBestScore = BestScore, eScore = m_Score, eNLives = NLives });
        }

        #endregion


        #region Events' subscription

        public override void SubscribeEvents()
        {
            base.SubscribeEvents();

            //MainMenuManager
            EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
            EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
            EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
            EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
            EventManager.Instance.AddListener<QuitButtonClickedEvent>(QuitButtonClicked);

            //Score Item
            EventManager.Instance.AddListener<ScoreItemEvent>(ScoreHasBeenGained);
        }

        public override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            //MainMenuManager
            EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
            EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
            EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
            EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
            EventManager.Instance.RemoveListener<QuitButtonClickedEvent>(QuitButtonClicked);

            //Score Item
            EventManager.Instance.RemoveListener<ScoreItemEvent>(ScoreHasBeenGained);
        }

        #endregion

        #region Callbacks to Events issued by MenuManager

        private void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
        {
            Menu();
        }

        private void PlayButtonClicked(PlayButtonClickedEvent e)
        {
            Play();
        }

        private void ResumeButtonClicked(ResumeButtonClickedEvent e)
        {
            Resume();
        }

        private void EscapeButtonClicked(EscapeButtonClickedEvent e)
        {
            if (IsPlaying) Pause();
        }

        private void QuitButtonClicked(QuitButtonClickedEvent e)
        {
            Application.Quit();
        }

        #endregion

        #region GameState methods

        private void Menu()
        {
            SetTimeScale(1);
            m_GameState = GameState.gameMenu;
            if (MusicLoopsManager.Instance) MusicLoopsManager.Instance.PlayMusic(Constants.MENU_MUSIC);
            EventManager.Instance.Raise(new GameMenuEvent());
        }

        private void Play()
        {
            InitNewGame();
            SetTimeScale(1);
            m_GameState = GameState.gamePlay;

            if (MusicLoopsManager.Instance) MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC);
            EventManager.Instance.Raise(new GamePlayEvent());
        }

        private void Pause()
        {
            if (!IsPlaying) return;

            SetTimeScale(0);
            m_GameState = GameState.gamePause;
            EventManager.Instance.Raise(new GamePauseEvent());
        }

        private void Resume()
        {
            if (IsPlaying) return;

            SetTimeScale(1);
            m_GameState = GameState.gamePlay;
            EventManager.Instance.Raise(new GameResumeEvent());
        }

        private void Over()
        {
            SetTimeScale(0);
            m_GameState = GameState.gameOver;
            EventManager.Instance.Raise(new GameOverEvent());
            if (SfxManager.Instance) SfxManager.Instance.PlaySfx2D(Constants.GAMEOVER_SFX);
        }

        #endregion
    }
}