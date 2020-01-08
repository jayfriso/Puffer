using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JFrisoGames.PuffMan
{
    public class CheckPointCollider : MonoBehaviour
    {
        public Action onCheckPointPasssed;

        /******* Variables & Properties*******/
        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PuffConstants.TAG_BALL))
                onCheckPointPasssed.Invoke();
        }

        /******* Monobehavior Methods *******/

        /******* Methods *******/
    }
}
