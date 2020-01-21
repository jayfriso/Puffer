using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace JFrisoGames.PuffMan
{
    public class Ball : MonoBehaviour
    {
        /******* Variables & Properties*******/

        public RollController rollController { get; private set; }
        public FlightController flightController { get; private set; }
        public JumpController jumpController { get; private set; }
        public MaxVelocityController maxVelocityController { get; private set; }

        public BallVisualController visualController { get; private set; }

        public BallInfo ballInfo { get; private set; }
        
        public BallCollider ballCollider { get; private set; }

        private List<AbstractBallController> _ballControllers;

        public Rigidbody rigidBody { get; private set; }

        /******* Monobehavior Methods *******/

        public void FixedUpdate()
        {
            if (_ballControllers != null)
                _ballControllers.ForEach(ballController => ballController.ExecuteFixedUpdate());
        }

        /******* Methods *******/

        public void Init()
        {
            rigidBody = GetComponent<Rigidbody>();

            rollController = GetComponent<RollController>();
            flightController = GetComponent<FlightController>();
            jumpController = GetComponent<JumpController>();
            maxVelocityController = GetComponent<MaxVelocityController>();
            visualController = GetComponentInChildren<BallVisualController>();
            ballInfo = GetComponent<BallInfo>();
            ballCollider = GetComponentInChildren<BallCollider>();

            // NOTE : This order matters as it changes the execution order of the FixedUpdateExecute calls
            _ballControllers = new List<AbstractBallController>() {
                rollController,
                flightController,
                jumpController,
                maxVelocityController,
                visualController,
                ballInfo,
                ballCollider
            };

            _ballControllers.ForEach(ballController => ballController.Init(this));
        }

        public void Reset(Vector3 startPosition)
        {
            rigidBody.velocity = Vector3.zero;
            transform.position = startPosition;
        }

    }
}
