using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private float _currentInputForceTime;
        private Vector3 _currentInputForce;

        private IRollInput _rollInput;

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public override void Init(Ball ball)
        {
            base.Init(ball);

            rigidBody.maxAngularVelocity = _maxAngularVelocity;

            _rollInput = GetComponent<IRollInput>();
            _rollInput.InitRollInput();
            _rollInput.onRollInput += HandleRollInput;
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

        private void HandleRollInput(Vector3 swipeDirection)
        {
            if (swipeDirection.x * rigidBody.velocity.x < 0)
                rigidBody.velocity = new Vector3(rigidBody.velocity.x * _oppositeXInputVelocityPercentage, rigidBody.velocity.y, rigidBody.velocity.z);
            _currentInputForce = swipeDirection * _swipeMagnitude;
            _currentInputForceTime = _swipeForceApplicationTime;

            Vector3 torque = new Vector3(swipeDirection.z, swipeDirection.x, 0) * _rotationTorqueForce;
            rigidBody.AddTorque(torque, ForceMode.Impulse);
        }
    }
}
