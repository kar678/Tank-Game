using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cogwheel.Player;
using Cogwheel.Levels;
using Cogwheel.CustomInput;
using TMPro;

namespace Cogwheel.UI
{
    public class HudController : MonoBehaviour
    {
        #region Core Vars
        TankController playerTank;
        [HideInInspector]
        public LevelManager levelManager;
        PlayerInputActions inputActions;
        public bool bDebug;
        private bool showingPauseUI = false;
        #endregion

        #region UI Elements
        [Header("UI Elements")]
        public Slider healthBar;
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI ammoText;
        public TextMeshProUGUI reloadingText;
        public GameObject dialogUI;
        public UIElement pauseUI;
        public GameObject[] objectives;

        [Header("Dialog Elements")]
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI bodyText;
        #endregion

        protected virtual void Awake()
        {
            levelManager = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelManager>();
            inputActions = new PlayerInputActions();
            inputActions.UI.CloseDialog.Enable();
            inputActions.UI.Pause.Enable();
        }

        // Start is called before the first frame update
        void Start()
        {
            playerTank = GameObject.FindGameObjectWithTag("Player").GetComponent<TankController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (healthBar)
            {
                healthBar.value = playerTank.currentHitPoints / playerTank.maxHitPoints;
            }

            if (healthText)
            {
                healthText.text = string.Format("{0} / {1}", playerTank.currentHitPoints, playerTank.maxHitPoints);
            }

            if(ammoText)
            {
                ammoText.text = string.Format("Ammo: {0} / {1}", playerTank.currentAmmo, playerTank.maxAmmo);
            }

            if(reloadingText)
            {
                if(playerTank.isReloading)
                {
                    if(!reloadingText.gameObject.activeInHierarchy)
                    {
                        reloadingText.gameObject.SetActive(true);
                    }

                    reloadingText.text = string.Format("Reloading: {0}", playerTank.reloadTimeLeft.ToString("0.0"));
                }
                else
                {
                    if (reloadingText.gameObject.activeInHierarchy)
                    {
                        reloadingText.gameObject.SetActive(false);
                    }
                }
            }

            if(inputActions.UI.Pause.triggered && !showingPauseUI && pauseUI)
            {
                levelManager.ChangeMouseLockState(0);
                pauseUI.Show();
                levelManager.ChangeTimeScale(0f);
            }
            else if(inputActions.UI.Pause.triggered && showingPauseUI)
            {
                pauseUI.Hide();
                levelManager.ChangeTimeScale(1f);
                levelManager.ChangeMouseLockState(1);
            }
        }

        private void OnGUI()
        {
            if(bDebug)
            {
                if (GUI.Button(new Rect(50, 40, 150, 35), "Test Dialog"))
                    ShowDialogUI("Commander Karl", "Welcome to the training course cadet, here you'll be learning all the basics about your tank, your enemys and your objectives. After you have completed this course you'll be promoted to tank commander and your next assignment will be revealed to you.");
            }
        }

        private void OnDisable()
        {
            inputActions.UI.Submit.Disable();
            inputActions.UI.Pause.Disable();
        }

        #region Custom Functions
        public virtual void ShowDialogUI(string title, string body)
        {
            titleText.text = title;
            bodyText.text = body;
            dialogUI.SetActive(true);
            levelManager.ChangeTimeScale(0f);
            StartCoroutine(DialogHandler());
        }

        protected virtual void HideDialogUI()
        {
            levelManager.ChangeTimeScale(1f);
            dialogUI.SetActive(false);
        }

        IEnumerator DialogHandler()
        {
            yield return new WaitUntil(IsConfirmPressed);
            HideDialogUI();
        }

        bool IsConfirmPressed()
        {
            if (inputActions.UI.CloseDialog.triggered) { return true; }
            else { return false; }
        }

        public virtual void AddObjectiveToList(objectiveStruct objective, int index, bool extraObjextive)
        {
            if(objective.mainObjective == true)
            {
                Objective script = objectives[0].GetComponent<Objective>();
                script.lable.text = objective.objective;
                objectives[0].SetActive(true);
            }
            else if(objective.optionalObjective == true && index != 0)
            {
                Objective script = objectives[index].GetComponent<Objective>();
                script.lable.text = objective.objective;
                objectives[index].SetActive(true);
            }
            else if(extraObjextive == true)
            {
                Objective script = objectives[3].GetComponent<Objective>();
                script.lable.text = objective.objective;
                objectives[3].SetActive(true);
            }
        }

        public virtual void RemoveObjectiveFromList(int index)
        {
            objectives[index].SetActive(false);
        }

        public virtual void CompleteObjective(int index)
        {
            Objective script = objectives[index].GetComponent<Objective>();
            script.toggle.isOn = true;
        }
        #endregion
    }
}