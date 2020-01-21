using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public abstract class AbstractBallController : MonoBehaviour
    {
        /******* Variables & Properties*******/

        protected Ball _ball;
        protected BallInfo ballInfo { get => _ball.ballInfo; }

        protected Rigidbody rigidBody { get => _ball.rigidBody; }

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public virtual void ExecuteFixedUpdate()
        {
            if (rigidBody == null) return;
        }

        public virtual void Init(Ball ball)
        {
            _ball = ball;
        }

    }
}
