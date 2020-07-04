using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cogwheel.Save;

namespace Cogwheel
{
    public class PlayerManager : MonoBehaviour
    {
        public string selectedTank = "Tank";
        public int playerLevel = 1;
        public int playerExperience = 0;
        public int xpNeededForNextLevel;
        public int xpNeededForEachLevel = 210;
        public int shotsFired = 0;
        public int tanksDestoryed = 0;
        public int objectivesCompleted = 0;
        public bool tutorialCompleted = false;

        bool loadedSave = false;
        bool levelingUp = false;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(playerExperience > xpNeededForNextLevel && !levelingUp && loadedSave)
            {
                levelingUp = true;
            }
        }

        void LevelUp()
        {
            playerExperience = playerExperience = xpNeededForNextLevel;
            playerLevel = playerLevel + 1;
            xpNeededForNextLevel = playerLevel * xpNeededForEachLevel;
            levelingUp = false;
        }

        public void LoadPlayer(int saveSlot)
        {
            if (SaveSystem.CheckFileExsits(saveSlot))
            {
                PlayerData data = SaveSystem.LoadPlayer(saveSlot);

                LoadData(data);
            }
            else
            {
                Debug.Log("System tyied to load a non exsiting save file");
            }
        }

        public void SavePlayer(int saveSlot)
        {
            SaveSystem.SavePlayer(this, saveSlot);
            LoadPlayer(saveSlot);
        }

        public void LoadData(PlayerData data)
        {
            playerLevel = data.playerLevel;
            playerExperience = data.playerExperience;
            shotsFired = data.shotsFired;
            tanksDestoryed = data.tanksDestoryed;
            objectivesCompleted = data.objectivesCompleted;
            tutorialCompleted = data.tutorialCompleted;

            xpNeededForNextLevel = playerLevel * xpNeededForEachLevel;
            loadedSave = true;
        }
    }
}
