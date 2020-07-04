using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cogwheel.AI
{
    public class AITankController : MonoBehaviour
    {

        [Header("Health")]
        public float maxHitpoints = 100;
        [HideInInspector]
        public float currentHitPoints;
        public Slider healthSlider;

        // Start is called before the first frame update
        void Start()
        {
            currentHitPoints = maxHitpoints;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateUI();
        }

        #region Custom Functions
        protected virtual void UpdateUI()
        {
            if(healthSlider)
            {
                healthSlider.value = currentHitPoints / maxHitpoints;
            }
        }

        public virtual void HandleTakeDamage(float damage)
        {
            currentHitPoints = (currentHitPoints > 0) ? currentHitPoints - damage : 0;
        }


        #endregion
    }
}
