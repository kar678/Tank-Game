using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Cogwheel.UI;

[System.Serializable]
public struct objectiveStruct
{
    public string objective;
    public bool mainObjective;
    public bool optionalObjective;
}

namespace Cogwheel.Levels
{
    public class LevelManager : MonoBehaviour
    {
        public bool lockMouseOnPlay = true;
        [HideInInspector]
        public GameManager gameManager;
        [HideInInspector]
        public PlayerManager playerManager;
        [HideInInspector]
        public HudController hud;
        public AudioMixer masterMixer;
        public objectiveStruct[] objectiveStruct;

        protected virtual void Awake()
        {
            if (lockMouseOnPlay) { ChangeMouseLockState(1); };
            
            if (GameObject.FindGameObjectWithTag("GameController"))
            {
                gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
                playerManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerManager>();
            }

            if (GameObject.FindGameObjectWithTag("Hud")) { hud = GameObject.FindGameObjectWithTag("Hud").GetComponent<HudController>(); }
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            if(hud)
            {
                for(int i = 0; i < objectiveStruct.Length; i++)
                {
                    hud.AddObjectiveToList(objectiveStruct[i], i, false);
                }
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        protected virtual void OnDisable()
        {
            EndGame();
        }

        #region Custom Functions
        protected virtual void EndGame()
        {
            ChangeMouseLockState(0);
            if (playerManager) { playerManager.SavePlayer(gameManager.currentSaveSlot); };
        }

        public virtual void ReturnToGarage()
        {
            EndGame();
            SceneManager.LoadScene("Garage");
        }

        public virtual void ChangeMouseLockState(int lockState)
        {
            if(lockState == 0)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            if(lockState == 1)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public virtual void ChangeTimeScale(float newTimeScale)
        {
            if(newTimeScale == 1)
            {
                masterMixer.SetFloat("GameVol", 0);
            }
            else if (newTimeScale == 0)
            {
                masterMixer.SetFloat("GameVol", -80);
            }

            Time.timeScale = newTimeScale;
        }

        public virtual void QuitToMain()
        {
            if (playerManager) { playerManager.SavePlayer(gameManager.currentSaveSlot); };
            SceneManager.LoadScene("MainMenu");
        }

        public virtual void QuitToDesktop()
        {
            if (playerManager) { playerManager.SavePlayer(gameManager.currentSaveSlot); };
            Application.Quit();
        }

        #endregion
    }
}