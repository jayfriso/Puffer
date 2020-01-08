using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class DeathCollider : MonoBehaviour
    {
        /******* Variables & Properties*******/

        /******* Monobehavior Methods *******/

        public void OnCollisionEnter(Collision collision)
        {
            HandleCollision(collision.collider);
        }

        public void OnTriggerEnter(Collider other)
        {
            HandleCollision(other);
        }

        /******* Methods *******/

        private void HandleCollision(Collider other)
        {
            if (other.CompareTag(PuffConstants.TAG_BALL))
                PuffSingletonManager.gameManager.SetBallToCheckPoint();
        }
    }
}
