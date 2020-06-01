using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace JFrisoGames.PuffMan
{
    public class Ball : MonoBehaviour
    {
        /******* Variables & Properties*******/

        public MovementController movementController { get; private set; }
        public FlightController flightController { get; private set; }
        public JumpController jumpController { get; private set; }
        public VelocityController maxVelocityController { get; private set; }

        public BallVisualController visualController { get; private set; }
        public BallCanvasController ballCanvasController { get; private set; }

        public StaminaController staminaController { get; private set; }


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

            movementController = GetComponent<MovementController>();
            flightController = GetComponent<FlightController>();
            jumpController = GetComponent<JumpController>();
            maxVelocityController = GetComponent<VelocityController>();
            ballInfo = GetComponent<BallInfo>();
            ballCollider = GetComponent<BallCollider>();
            staminaController = GetComponent<StaminaController>();

            visualController = GetComponentInChildren<BallVisualController>();
            ballCanvasController = GetComponentInChildren<BallCanvasController>();

            // NOTE : This order matters as it changes the execution order of the FixedUpdateExecute calls
            _ballControllers = new List<AbstractBallController>() {
                movementController,
                flightController,
                jumpController,
                maxVelocityController,
                visualController,
                ballInfo,
                ballCollider,
                staminaController,
                ballCanvasController
            };

            _ballControllers.ForEach(ballController => ballController.Init(this));

            ballInfo.onBallStaminaChanged += ballCanvasController.staminaCircleElement.HandleStaminaValueChanged;
        }

        public void Reset(Vector3 startPosition)
        {
            rigidBody.velocity = Vector3.zero;
            transform.position = startPosition;
        }

    }
}
