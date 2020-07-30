using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Cogwheel.Levels;
using Cogwheel.Player;
using Cogwheel.Localization;

namespace Cogwheel.UI
{
    public class GarageUIController : MonoBehaviour
    {
        PlayerManager player;
        GarageLevelManager levelManager;

        [Header("Player Stats Display")]
        public TextMeshProUGUI levelText;
        public Slider xpSlider;

        [Header("Tutorial Elements")]
        public GameObject TutorialUI;

        [Header("Tank Stats Elements")]
        public TextMeshProUGUI tankHitpointsText;
        public TextMeshProUGUI tankMaxDamageText;
        public TextMeshProUGUI tankShotsBeforeReloadText;
        public TextMeshProUGUI tankReloadTimeText;

        // Start is called before the first frame update
        void Start()
        {
            if(GameObject.FindGameObjectWithTag("GameController") && GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().currentSaveSlot > 0)
            {
                player = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerManager>();
                UpdatePlayerStatsText();
            }

            if(GameObject.FindGameObjectWithTag("LevelController")) { levelManager = GameObject.FindGameObjectWithTag("LevelController").GetComponent<GarageLevelManager>(); }

            if(player && TutorialUI)
            {
                if(player.tutorialCompleted != true)
                {
                    TutorialUI.GetComponent<UIElement>().Show();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void UpdatePlayerStatsText()
        {
            if (player)
            {
                levelText.text = player.playerLevel.ToString();
                xpSlider.value = player.playerExperience / player.xpNeededForNextLevel;
            }
        }

        public void ReturnToMain()
        {
            levelManager.QuitToMain();
        }

        public void QuitToDesktop()
        {
            levelManager.QuitToDesktop();
        }

        public void PlayTutorial()
        {
            SceneManager.LoadScene("Tutorial");
        }

        public void SetTutorialCompleted()
        {
            player.tutorialCompleted = true;
        }

        public void UpdateTankStats(TankController tank)
        {
            if (tankHitpointsText) { tankHitpointsText.text = LocalizationSystem.GetLocalisedValue("text_tankhitpoints") + ": " + tank.maxHitPoints.ToString(); }
            if (tankMaxDamageText) { tankMaxDamageText.text = LocalizationSystem.GetLocalisedValue("text_tankmaxdamage") + ": " + tank.shellMaxDamage.ToString(); }
            if (tankShotsBeforeReloadText) { tankShotsBeforeReloadText.text = LocalizationSystem.GetLocalisedValue("text_tankshotsreload") + ": " + tank.maxAmmo.ToString(); }
            if (tankReloadTimeText) { tankReloadTimeText.text = LocalizationSystem.GetLocalisedValue("text_tankreloadtime") + ": " + tank.reloadTime.ToString() + "s"; }
        }
    }
}
