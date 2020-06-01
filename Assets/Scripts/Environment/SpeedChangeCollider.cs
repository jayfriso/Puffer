using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class SpeedChangeCollider : AbstractEnvironmentCollider
    {
        /******* Variables & Properties*******/

        [SerializeField] private MaxVelocityBoost _maxVelocityBoost;
        [SerializeField] private Vector3 _boostForce;

        /******* Monobehavior Methods *******/

        public void OnCollisionEnter(Collision collision)
        {
            CheckAndSetBall(collision.collider);
        }

        public void OnTriggerEnter(Collider other)
        {
            CheckAndSetBall(other);
        }

        /******* Methods *******/

        protected override void HandleFixedUpdate(Ball ball)
        {
            //ball.rigidBody.velocity = new Vector3(ball.rigidBody.velocity.x, 0, ball.rigidBody.velocity.z);
            ball.rigidBody.AddForce(_boostForce, ForceMode.Impulse);
            ball.maxVelocityController.maxVelocityBoosts.Add(_maxVelocityBoost.Copy());
        }
    }
}
