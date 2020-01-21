using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;

namespace JFrisoGames.PuffMan
{
    public class RollController : AbstractBallController
    {
        /******* Variables & Properties*******/
        [Header("Movement Variables")]
        [SerializeField] private float _swipeMagnitude;
        [SerializeField] private float _swipeForceApplicationTime;
        [SerializeField] private AnimationCurve _forceApplicationCurve;
        [SerializeField, Tooltip("The percantage of the original x velocity to keep when an input of the opposite x direction is made.")]
        private float _oppositeXInputVelocityPercentage = 0.5f;

        [Header("Rotation Physics")]
        [SerializeField] private float _maxAngularVelocity;
        [SerializeField] private float _rotationTorqueForce;

        [Header("Input Settings")]
        private float _minSwipeDistance = 0.1f;

        private float _currentInputForceTime;
        private Vector3 _currentInputForce;

        private SwipeGestureRecognizer _swipeGesture;


        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public override void Init(Ball ball)
        {
            base.Init(ball);

            rigidBody.maxAngularVelocity = _maxAngularVelocity;

            _swipeGesture = new SwipeGestureRecognizer();
            _swipeGesture.MinimumDistanceUnits = _minSwipeDistance;
            _swipeGesture.StateUpdated += HandleSwipe;
            FingersScript.Instance.AddGesture(_swipeGesture);
        }

        public override void ExecuteFixedUpdate()
        {
            base.ExecuteFixedUpdate();

            if (_currentInputForceTime > 0f)
            {
                _currentInputForceTime -= Time.fixedDeltaTime;
                rigidBody.AddForce(_currentInputForce * _forceApplicationCurve.Evaluate(1 - (_currentInputForceTime / _swipeForceApplicationTime)), ForceMode.Force);
            }
        }

        private void HandleSwipe(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended)
            {
                Vector3 swipeDirection = (new Vector3(gesture.FocusX - gesture.StartFocusX, 0, gesture.FocusY - gesture.StartFocusY)).normalized;

                if (swipeDirection.x * rigidBody.velocity.x < 0)
                    rigidBody.velocity = new Vector3(rigidBody.velocity.x * _oppositeXInputVelocityPercentage, rigidBody.velocity.y, rigidBody.velocity.z);
                _currentInputForce = swipeDirection * _swipeMagnitude;
                _currentInputForceTime = _swipeForceApplicationTime;

                Vector3 torque = new Vector3(gesture.FocusY - gesture.StartFocusY, gesture.FocusX - gesture.StartFocusX, 0) * _rotationTorqueForce;
                rigidBody.AddTorque(torque, ForceMode.Impulse);
            }
        }
    }
}
