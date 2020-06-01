using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public enum BallVisualState
    {
        Regular = 0,
        Flying = 1
    }

    public class BallVisualController : AbstractBallController
    {
        /******* Variables & Properties*******/

        [SerializeField] private GameObject _glider;

        private Animator _animator;

        private BallVisualState _ballVisualState;
        public BallVisualState ballVisualState 
        { 
            get { return _ballVisualState; }
            set
            {
                _ballVisualState = value;
                _glider.SetActive(value == BallVisualState.Flying);
            }
        }

        /******* Monobehavior Methods *******/

        // TODO : Fix to use the fixed update method of base class
        public void FixedUpdate()
        {
            _animator.SetFloat("xVelocity", rigidBody.velocity.x);
        }

        /******* Methods *******/

        public override void Init(Ball ball)
        {
            base.Init(ball);

            _animator = GetComponent<Animator>();

            ballVisualState = BallVisualState.Regular;
        }
    }
}
