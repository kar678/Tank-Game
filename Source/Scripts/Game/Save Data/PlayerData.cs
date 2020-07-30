using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogwheel.Save
{
    [System.Serializable]
    public class PlayerData
    {
        #region CoreStats
        public int playerLevel;
        public int playerExperience;
        public string selectedTank;
        #endregion

        #region MiscStats
        public int objectivesCompleted;
        public int shotsFired;
        public int tanksDestoryed;
        #endregion

        #region ProgressionStats
        public bool tutorialCompleted;
        #endregion

        public PlayerData (PlayerManager playerManager)
        {
            playerLevel = playerManager.playerLevel;
            playerExperience = playerManager.playerExperience;
            selectedTank = playerManager.selectedTank;
            objectivesCompleted = playerManager.objectivesCompleted;
            shotsFired = playerManager.shotsFired;
            tanksDestoryed = playerManager.tanksDestoryed;
            tutorialCompleted = playerManager.tutorialCompleted;
        }
    }
}
