using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cogwheel.Player;
using Cogwheel.UI;

namespace Cogwheel.Levels
{
    public class GarageLevelManager : LevelManager
    {
        public Transform tankSpawnPoint;
        public GameObject defaultTank;
        GameObject selectedTank;
        GameObject activeTank;
        GarageUIController garageUI;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            if (playerManager)
            {
                string tankLocation = string.Format("{0}/{1}", "Prefabs/Tanks", playerManager.selectedTank);
                selectedTank = Resources.Load(tankLocation) as GameObject;
            }

            if(GameObject.FindGameObjectWithTag("Hud"))
            {
                garageUI = GameObject.FindGameObjectWithTag("Hud").GetComponent<GarageUIController>();
            }

            if(selectedTank)
            {
                activeTank = Instantiate(selectedTank, tankSpawnPoint);
                TankController tank = activeTank.GetComponent<TankController>();
                tank.disableControls = true;
            }
            else
            {
                activeTank = Instantiate(defaultTank, tankSpawnPoint);
                TankController tank = activeTank.GetComponent<TankController>();
                tank.disableControls = true;

                if(playerManager)
                {
                    playerManager.selectedTank = defaultTank.name;
                }
            }

            UpdateTankStats();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        public virtual void UpdateTankStats()
        {
            TankController tank = activeTank.GetComponent<TankController>();
            garageUI.UpdateTankStats(tank);
        }

        public virtual void ChangeActiveTank(string newTank)
        {
            if (playerManager && playerManager.selectedTank == newTank) { return; }

            Destroy(activeTank);

            string tankLocation = string.Format("{0}/{1}", "Prefabs/Tanks", newTank);

            if (selectedTank = Resources.Load(tankLocation) as GameObject)
            {
                activeTank = Instantiate(selectedTank, tankSpawnPoint);
                TankController tank = activeTank.GetComponent<TankController>();
                tank.disableControls = true;
            }
            else
            {
                activeTank = Instantiate(defaultTank, tankSpawnPoint);
                TankController tank = activeTank.GetComponent<TankController>();
                tank.disableControls = true;
            }

            UpdateTankStats();
        }
    }
}
