using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cogwheel.Levels
{
    public class StartupManager : LevelManager
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            StartCoroutine(LoadSceneWhenReady());
        }

        IEnumerator LoadSceneWhenReady()
        {
            yield return new WaitUntil(IsSetupCompleted);
            SceneManager.LoadScene("MainMenu");
        }

        bool IsSetupCompleted()
        {
            if (gameManager.setupCompleted) { return true; }
            else { return false; }
        }
    }
}
