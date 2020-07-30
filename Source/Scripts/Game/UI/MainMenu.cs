using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Cogwheel.Save;

namespace Cogwheel.UI
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject confirmPanel;
        public TextMeshProUGUI confirmText;
        int confirmReason;
        public Button quitButton;
        GameManager gameManager;
        PlayerManager playerManager;
        private int lastSaveSlot;

        public Button loadSaveButton1;
        public Button loadSaveButton2;
        public Button loadSaveButton3;
        public Button loadSaveButton4;

        public TextMeshProUGUI[] newSaveSlotText;
        public TextMeshProUGUI[] loadSaveSlotText;

        // Start is called before the first frame update
        void Start()
        {
            if(GameObject.FindGameObjectWithTag("GameController")) { gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>(); }
            if (GameObject.FindGameObjectWithTag("GameController")) { playerManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerManager>(); }

            if (quitButton)
            {
                quitButton.onClick.AddListener(QuitButtonClicked);
            }

            SetSaveButtonsText();
            SetLoadButtonsInteractable();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void QuitButtonClicked()
        {
            ShowConfirmBox(1, "Quit Game");
        }

        void ShowConfirmBox(int Reason, string Text)
        {
            confirmText.text = Text;
            confirmPanel.GetComponent<UIElement>().Show();
            confirmReason = Reason;
        }

        void SetLoadButtonsInteractable()
        {
            if (loadSaveButton1)
            {
                if (!SaveSystem.CheckFileExsits(1))
                {
                    loadSaveButton1.interactable = false;
                }
            }

            if (loadSaveButton2)
            {
                if (!SaveSystem.CheckFileExsits(2))
                {
                    loadSaveButton2.interactable = false;
                }
            }

            if (loadSaveButton3)
            {
                if (!SaveSystem.CheckFileExsits(3))
                {
                    loadSaveButton3.interactable = false;
                }
            }

            if (loadSaveButton4)
            {
                if (!SaveSystem.CheckFileExsits(4))
                {
                    loadSaveButton4.interactable = false;
                }
            }
        }

        void SetSaveButtonsText()
        {
            if (newSaveSlotText.Length > 0)
            {
                for (int i = 0; i < newSaveSlotText.Length; i++)
                {
                    if(SaveSystem.CheckFileExsits(i + 1))
                    {
                        newSaveSlotText[i].text = "Last Played: " + SaveSystem.GetFileLastWriteTime(i + 1);
                    }
                    else
                    {
                        newSaveSlotText[i].text = "No Save File";
                    }
                }
            }

            if (loadSaveSlotText.Length > 0)
            {
                for (int i = 0; i < loadSaveSlotText.Length; i++)
                {
                    if (SaveSystem.CheckFileExsits(i + 1))
                    {
                        loadSaveSlotText[i].text = "Last Played: " + SaveSystem.GetFileLastWriteTime(i + 1);
                    }
                    else
                    {
                        loadSaveSlotText[i].text = "No Save File";
                    }
                }
            }
        }

        public void ConfirmYes()
        {
            if (confirmReason == 1)
            {
                QuitGame();
            }
            else if(confirmReason == 2)
            {
                SaveSystem.DeleteSaveFile(lastSaveSlot);
                gameManager.currentSaveSlot = lastSaveSlot;
                playerManager.SavePlayer(lastSaveSlot);
                SceneManager.LoadScene("Garage");
            }
        }

        public void NewSave(int saveSlot)
        {
            if (SaveSystem.CheckFileExsits(saveSlot))
            {
                lastSaveSlot = saveSlot;
                ShowConfirmBox(2, "Overwrite Save File");
            }
            else
            {
                gameManager.currentSaveSlot = saveSlot;
                playerManager.SavePlayer(saveSlot);
                SceneManager.LoadScene("Garage");
            }
        }

        public void LoadSave(int saveSlot)
        {
            gameManager.currentSaveSlot = saveSlot;
            playerManager.LoadPlayer(saveSlot);
            SceneManager.LoadScene("Garage");
        }

        void QuitGame()
        {
#if UNITY_STANDALONE
            Application.Quit();
#endif
        }

    }
}
