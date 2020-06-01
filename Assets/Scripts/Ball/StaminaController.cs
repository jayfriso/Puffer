using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class StaminaController : AbstractBallController
    {    
        /******* Events *******/
    
        /******* Variables & Properties*******/
        [SerializeField] private float _staminaRegenRate;
        [SerializeField] private float _staminaDepletionRate;

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public override void ExecuteFixedUpdate()
        {
            base.ExecuteFixedUpdate();
            if (ballInfo.isInFlight)
                ballInfo.stamina = ballInfo.stamina - Time.fixedDeltaTime * _staminaDepletionRate;
            else
                ballInfo.stamina = ballInfo.stamina + Time.fixedDeltaTime * _staminaRegenRate;
        }
    }
}
