using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class DeathCollider : AbstractEnvironmentCollider
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
            CheckAndSetBall(other);
        }

        protected override void HandleFixedUpdate(Ball ball)
        {
            PuffSingletonManager.gameManager.SetBallToCheckPoint();
        }
    }
}
