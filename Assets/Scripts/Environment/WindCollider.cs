using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class WindCollider : AbstractEnvironmentCollider
    {
        /******* Variables & Properties*******/
        [SerializeField] private Vector3 _windForce;

        /******* Monobehavior Methods *******/

        public void OnTriggerStay(Collider other)
        {
            CheckAndSetBall(other);
        }

        /******* Methods *******/

        protected override void HandleFixedUpdate(Ball ball)
        {
            if (ball.ballInfo.isInFlight)
                ball.rigidBody.AddForce(_windForce);
        }
    }
}
