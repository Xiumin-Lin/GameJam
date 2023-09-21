using System.Collections;

namespace STUDENT_NAME
{
    public class HudManager : Manager<HudManager>
    {
        #region Manager implementation

        protected override IEnumerator InitCoroutine()
        {
            yield break;
        }

        #endregion

        #region Callbacks to GameManager events

        protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
        {
            //TO DO
        }

        #endregion

        //[Header("HudManager")]

        #region Labels & Values

        // TO DO

        #endregion
    }
}