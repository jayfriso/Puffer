﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;

namespace JFrisoGames.PuffMan
{
    public class BallTouchInput : MonoBehaviour, IMovementInput, IJumpInput, IFlightInput
    {
        /******* Events *******/

        public event OnMovementInput onMovementInput;

        public event OnJumpInput onJumpInput;

        public event OnFlightStartInput onFlightStartInput;
        public event OnFlightEndInput onFlightEndInput;

        /******* Variables & Properties*******/

        /*** Movement Variables ***/

        private bool _isMovementEnabled = false;
        private bool _wasLastTouchPresent = false;

        /*** Jump Variables ***/

        private SwipeGestureRecognizer _jumpSwipeGesture;

        /*** Flight Variables ***/
        private BallInfo _ballInfo;

        [SerializeField] private float _holdTimeForGlideActivation;
        private bool _isFlightEnabled;
        private float _currentAirTime;

        /******* Monobehavior Methods *******/

        public void Update()
        {
            if (!_isMovementEnabled) return;

            bool isThereATouch = FingersScript.Instance.CurrentTouches.Count > 0;
            GestureTouch firstTouch = isThereATouch ? FingersScript.Instance.CurrentTouches[0] : new GestureTouch();

            if (_isMovementEnabled)
            {
                // Movement Logic
                if (isThereATouch)
                {
                    if (_wasLastTouchPresent)
                    {
                        onMovementInput?.Invoke(firstTouch.DeltaX);
                    }
                    _wasLastTouchPresent = true;
                }
                else
                {
                    _wasLastTouchPresent = false;
                }
            }

            if (_isFlightEnabled)
            {
                // If in air, there is a touch, and not flying yet uptick the timeInAir until we start flight
                if (!_ballInfo.isCollidingWithFloor && !_ballInfo.isInFlight && isThereATouch)
                {
                    _currentAirTime += Time.deltaTime;
                    if (_currentAirTime > _holdTimeForGlideActivation)
                    {
                        onFlightStartInput?.Invoke();
                    }
                }

                // If the ball has just collided or the touch was released reset the timeInAir and end flight if in flight currently
                if ((_ballInfo.isCollidingWithFloor || !isThereATouch) && _currentAirTime > 0f)
                {
                    _currentAirTime = 0f;

                    if (_ballInfo.isInFlight)
                        onFlightEndInput?.Invoke();
                }
            }
        }

        /******* Methods *******/

        public void InitMovementInput()
        {
            _isMovementEnabled = true;
        }

        public void InitJumpInput()
        {
            _jumpSwipeGesture = new SwipeGestureRecognizer();
            _jumpSwipeGesture.Direction = SwipeGestureRecognizerDirection.Up;
            _jumpSwipeGesture.MinimumDistanceUnits = 0.75f;
            _jumpSwipeGesture.MinimumSpeedUnits = 2f;
            _jumpSwipeGesture.FailOnDirectionChange = false;

            _jumpSwipeGesture.StateUpdated += HandleJumpSwipe;
            FingersScript.Instance.AddGesture(_jumpSwipeGesture);
        }

        private void HandleJumpSwipe(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended)
            {
                onJumpInput?.Invoke(1f);
            }
        }

        public void InitFlightInput()
        {
            _isFlightEnabled = true;

            _ballInfo = GetComponent<Ball>().ballInfo;
        }
    }
}
