using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using Steamworks;

namespace Cogwheel
{
    public class GameManager : MonoBehaviour
    {
        public int currentSaveSlot;
        [HideInInspector]
        public bool setupCompleted = false;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
            try
            {
                SteamClient.Init(480);
            }
            catch (System.Exception e)
            {
                Debug.Log("Steam Couldn't be Initalized");
                //Utils.ForceCrash(ForcedCrashCategory.FatalError);
            }
#endif
            setupCompleted = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnDisable()
        {
            Steamworks.SteamClient.Shutdown();
        }
    }
}
