using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public abstract class AbstractEnvironmentCollider : MonoBehaviour
    {
        /******* Variables & Properties*******/

        protected BallCollider _ballCollider;


        /******* Monobehavior Methods *******/

        public void FixedUpdate()
        {
            if (_ballCollider != null)
            {
                HandleFixedUpdate(_ballCollider.ball);
            }
            _ballCollider = null; // Reset the ballCollider to null until we set it again
        }

        /******* Methods *******/

        protected virtual void HandleFixedUpdate(Ball ball) { }

        // Checks whether the collider is the ball, and sets the ball collider if so
        protected void CheckAndSetBall(Collider collider)
        {
            if (collider.CompareTag(PuffConstants.TAG_BALL))
                _ballCollider = collider.GetComponent<BallCollider>();
        }
    }
}
