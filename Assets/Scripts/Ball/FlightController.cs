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
        [SerializeField] private Vector3 _flightForwardForce;
        [SerializeField] private float _flightHoldSpeed;

        [Header("Max Velocity")]
        [SerializeField] private float _flightMaxVelocity;
        [SerializeField] private float _maxVelocityLerpTime;

        [Header("Control Settings")]
        [SerializeField] private float _holdGestureActivationTime;

        private LongPressGestureRecognizer _longPressGesture;

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public override void Init(Ball ball)
        {
            base.Init(ball);
            _longPressGesture = new LongPressGestureRecognizer();
            _longPressGesture.MinimumDurationSeconds = _holdGestureActivationTime;
            _longPressGesture.StateUpdated += HandleHold;
            FingersScript.Instance.AddGesture(_longPressGesture);
        }

        public override void ExecuteFixedUpdate()
        {
            base.ExecuteFixedUpdate();

            // End flight if collided with floor
            if (ballInfo.isInFlight && ballInfo.isCollidingWithFloor)
                EndFlight();

            // Apply flight force if in flight
            if (ballInfo.isInFlight)
                rigidBody.AddForce(_flightForwardForce, ForceMode.Force);
        }

        private void HandleHold(GestureRecognizer gesture)
        {
            switch (gesture.State)
            {
                case GestureRecognizerState.Began:
                {
                    if (!ballInfo.isCollidingWithFloor)
                        StartFlight();
                    break;
                }
                case GestureRecognizerState.Executing:
                {
                    if (!ballInfo.isInFlight)
                        return;

                    float horizontalMoveAmount = gesture.DeltaX * _flightHoldSpeed;
                    rigidBody.AddForce(new Vector3(horizontalMoveAmount, 0, 0), ForceMode.Force);
                    break;
                }
                case GestureRecognizerState.Ended:
                {
                    EndFlight();
                    break;
                }
            }
        }

        private void StartFlight()
        {
            ballInfo.isInFlight = true;

            _ball.maxVelocityController.KillMaxVelocityLerp();
            _ball.maxVelocityController.SetMaxVelocity(_flightMaxVelocity);
            
            _ball.visualController.ballVisualState = BallVisualState.Flying;
        }

        private void EndFlight()
        {
            ballInfo.isInFlight = false;

            _ball.maxVelocityController.SetMaxVelocityLerp(rigidBody.velocity.z, _ball.maxVelocityController.baseMaxVelocity, _maxVelocityLerpTime);

            _ball.visualController.ballVisualState = BallVisualState.Regular;
        }
    }
}
