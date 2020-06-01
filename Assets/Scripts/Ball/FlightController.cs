using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;

namespace JFrisoGames.PuffMan
{
    public class FlightController : AbstractBallController
    {
        /******* Variables & Properties*******/
        [Header("Flight Movement")]
        [SerializeField] private Vector3 _flightActivationImpulseForce;

        [Header("Max Velocity")]
        [SerializeField] private float _flightMinYVelocity;

        [SerializeField] private float _maxVelocityLerpTime;

        private Vector3 _currentVerticalForce = Vector3.zero;

        private IFlightInput _flightInput;

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public override void Init(Ball ball)
        {
            base.Init(ball);

            _flightInput = GetComponent<IFlightInput>();
            _flightInput.InitFlightInput();
            _flightInput.onFlightStartInput += StartFlight;
            _flightInput.onFlightEndInput += EndFlight;
        }

        private void StartFlight()
        {
            if (ballInfo.isInFlight) return;

            ballInfo.isInFlight = true;

            _ball.maxVelocityController.KillMaxVelocityLerp();
            _ball.maxVelocityController.SetMinYVelocity(_flightMinYVelocity);

            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y / 4, rigidBody.velocity.z);
            rigidBody.AddForce(_flightActivationImpulseForce, ForceMode.Impulse);
            
            _ball.visualController.ballVisualState = BallVisualState.Flying;
        }

        private void EndFlight()
        {
            if (!ballInfo.isInFlight) return;

            ballInfo.isInFlight = false;

            _ball.maxVelocityController.SetMaxVelocityLerp(rigidBody.velocity.z, _ball.maxVelocityController.baseMaxVelocity, _maxVelocityLerpTime);
            _ball.maxVelocityController.SetMinYVelocity(Mathf.NegativeInfinity);


            _ball.visualController.ballVisualState = BallVisualState.Regular;
        }
    }
}
