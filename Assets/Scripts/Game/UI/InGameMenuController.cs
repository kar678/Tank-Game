using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cogwheel.Levels;

namespace Cogwheel.UI
{
    public class InGameMenuController : UIElement
    {
        [Header("UI Elements")]
        public Button resumeButton;
        public Button settingsButton;
        public Button returnToGarageButton;
        public Button quitButton;
        public UIElement settingsMenu;
        LevelManager levelManager;

        // Start is called before the first frame update
        protected void Start()
        {
            if(GameObject.FindGameObjectWithTag("LevelController")) { levelManager = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelManager>(); };
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void HideMenu()
        {
            Hide();
            levelManager.ChangeTimeScale(1f);
            levelManager.ChangeMouseLockState(1);
        }

        public void ShowSettingsMenu()
        {
            settingsMenu.Show();
        }

        public void ReturnToGarage()
        {
            levelManager.ReturnToGarage();
        }

        public void QuitToDesktop()
        {
            levelManager.QuitToDesktop();
        }
    }
}
