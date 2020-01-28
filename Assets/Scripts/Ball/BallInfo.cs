using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class BallInfo : AbstractBallController
    {
        /******* Events *******/
        public delegate void OnBallVelocityChange(int newBallSpeed);
        public event OnBallVelocityChange onBallVelocityChange;
        public delegate void OnBallPositionChange(Vector3 newPosition);
        public event OnBallPositionChange onBallPositionChange;

        /******* Variables & Properties*******/
        [Header("Info Variables")]
        [SerializeField] private float _floorDetectionRayDistance;

        public bool isInFlight { get; set; } // TODO add actual logic

        public bool isCollidingWithFloor
        {
            get
            {
                RaycastHit hit;
                Debug.DrawRay(transform.position, Vector3.down * _floorDetectionRayDistance, Color.yellow);
                // Does the ray intersect any objects excluding the player layer
                return Physics.Raycast(transform.position, Vector3.down, out hit, _floorDetectionRayDistance);
            }
        }

        [Header("EventSettings")]
        [SerializeField] private Vector3 _ballPositionChangeMinDelta;
        private int _lastVelocityAsInt = 0;
        private Vector3 _lastPosition;

        /******* Monobehavior Methods *******/

        /******* Methods *******/


        public override void ExecuteFixedUpdate()
        {
            base.ExecuteFixedUpdate();

            // Handle Velocity Changes
            int currentVelocity = Mathf.FloorToInt(rigidBody.velocity.z);
            if (_lastVelocityAsInt != currentVelocity)
            {
                _lastVelocityAsInt = currentVelocity;
                if (onBallVelocityChange != null)
                    onBallVelocityChange.Invoke(currentVelocity);
            }

            // Handle Position Changes
            Vector3 currentDiff = transform.position - _lastPosition;
            if (Mathf.Abs(currentDiff.x) > Mathf.Abs(_ballPositionChangeMinDelta.x)
                && Mathf.Abs(currentDiff.y) > Mathf.Abs(_ballPositionChangeMinDelta.y)
                && Mathf.Abs(currentDiff.z) > Mathf.Abs(_ballPositionChangeMinDelta.z))
            {
                if (onBallPositionChange != null)
                    onBallPositionChange.Invoke(transform.position);
            }
        }
    }
}
