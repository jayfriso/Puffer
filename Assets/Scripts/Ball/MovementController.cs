using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class MovementController : AbstractBallController
    {
        /******* Variables & Properties*******/
        [Header("Movement Variables")]
        [SerializeField] private Vector3 _movementSpeed;

        [Header("Forward Movement")]
        [SerializeField] private Vector3 _groundedForwardForce;
        [SerializeField] private Vector3 _inAirForwardForce;
        [SerializeField] private Vector3 _flightForwardForce;

        private IMovementInput _movementInput;

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public override void Init(Ball ball)
        {
            base.Init(ball);

            _movementInput = GetComponent<IMovementInput>();
            _movementInput.InitMovementInput();
            _movementInput.onMovementInput += HandleMovementInput;
        }

        public override void ExecuteFixedUpdate()
        {
            base.ExecuteFixedUpdate();

            Vector3 forwardForce;
            if (ballInfo.isInFlight)
                forwardForce = _flightForwardForce;
            else if (!ballInfo.isCollidingWithFloor)
                forwardForce = _inAirForwardForce;
            else
                forwardForce = _groundedForwardForce;


            rigidBody.AddForce(forwardForce, ForceMode.Force);
        }

        private void HandleMovementInput(float movementDelta)
        {
            rigidBody.AddForce(_movementSpeed * movementDelta, ForceMode.Force);
        }
    }
}
