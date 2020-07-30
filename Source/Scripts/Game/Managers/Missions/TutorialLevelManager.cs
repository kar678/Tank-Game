using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cogwheel.Localization;
using Cogwheel.UI;

namespace Cogwheel.Levels
{
    public class TutorialLevelManager : LevelManager
    {
        protected override void Awake()
        {
            base.Awake();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            if(hud)
            {
                hud.ShowDialogUI(LocalizationSystem.GetLocalisedValue("text_commander1"), LocalizationSystem.GetLocalisedValue("text_tutstart"));
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }
}
