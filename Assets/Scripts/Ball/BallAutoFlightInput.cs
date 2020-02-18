using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class BallAutoFlightInput : MonoBehaviour, IFlightInput
    {

        /******* Events *******/

        public event OnFlightStartInput onFlightStartInput;
        public event OnFlightEndInput onFlightEndInput;

        /******* Variables & Properties*******/

        private BallInfo _ballInfo;

        [SerializeField]
        private float _timeInAirToTriggerFlight;

        private bool _isFlightEnabled = false;
        private float _currentTimeInAir = 0f;

        /******* Monobehavior Methods *******/

        public void Update()
        {
            if (!_isFlightEnabled) return;

            // If in air and not flying yet uptick the timeInAir until we start flight
            if (!_ballInfo.isCollidingWithFloor && !_ballInfo.isInFlight)
            {
                _currentTimeInAir += Time.deltaTime;
                if (_currentTimeInAir > _timeInAirToTriggerFlight)
                {
                    onFlightStartInput?.Invoke();
                }
            }

            // If the ball has just collided with the floor reset the timeInAir and end flight if in flight currently
            if (_ballInfo.isCollidingWithFloor && _currentTimeInAir > 0f)
            {
                _currentTimeInAir = 0f;

                if (_ballInfo.isInFlight)
                    onFlightEndInput?.Invoke();
            }
        }

        /******* Methods *******/

        public void InitFlightInput()
        {
            _isFlightEnabled = true;
            _ballInfo = GetComponent<Ball>().ballInfo;
        }
    }
}
