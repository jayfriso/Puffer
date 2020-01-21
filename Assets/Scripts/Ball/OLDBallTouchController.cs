using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;
using UnityEngine.Serialization;
using System;
using DG.Tweening;

namespace JFrisoGames.PuffMan
{
    public class OLDBallTouchController : MonoBehaviour
    {
        #region DEPRECATED
        /******* Events *******/
        public delegate void OnBallVelocityChange(int newBallSpeed);
        public event OnBallVelocityChange onBallVelocityChange;
        public delegate void OnBallPositionChange(Vector3 newPosition);
        public event OnBallPositionChange onBallPositionChange;

        /******* Variables & Properties*******/

        [Header("Components")]
        [SerializeField] private BallVisualController _visualController;

        // Movement Variables

        [Header("Movement Physics")]
        [SerializeField] private float _baseMaxVelocity;
        [SerializeField] private float _swipeMagnitude;
        [SerializeField] private float _swipeForceApplicationTime;
        [SerializeField] private float _boostFadeOutTime;
        [SerializeField] private AnimationCurve _forceApplicationCurve;
        [SerializeField] private List<MaxVelocityBoost> _bounceMaxVelocityBoosts;
        [SerializeField] private float _maxVelocityLerpTime;

        public Rigidbody rigidBody { private set; get; }
        private float _currentInputForceTime;
        private Vector3 _currentInputForce;
        private float _currentMaxVelocity;
        private Tween _maxVelocityLerpTween;

        public List<MaxVelocityBoost> maxVelocityBoosts { get; private set; }

        private float _maxVelocity
        {
            get
            {
                float result = _isInFlight ? _flightMaxSpeed : _currentMaxVelocity;
                for (int i = 0; i < maxVelocityBoosts.Count; i++)
                {
                    MaxVelocityBoost boost = maxVelocityBoosts[i];
                    if (boost.time < _boostFadeOutTime)
                        result += Mathf.Lerp(0, boost.amount, (boost.time / _boostFadeOutTime));
                    else
                        result += boost.amount;
                }
                return result;
            }
        }

        // Rotation Variables

        [Header("Rotation Physics")]
        [SerializeField] private float _maxAngularVelocity;
        [SerializeField] private float _rotationTorqueForce;

        // Compression Release Variables

        [Header("Compression")]
        [SerializeField] private Vector3 _fullyCompressedScale;
        [SerializeField] private float _compressionTime;
        [SerializeField] float _releaseTime;
        [SerializeField] float _minCompressionAmount;
        [SerializeField] [FormerlySerializedAs("_releaseForces")] List<Vector3> _bounceForces;
        [SerializeField] private float _floorDetectionRayDistance;
        [SerializeField] private float _floorTimeToResetReleaseLevel;
        [SerializeField] private Vector3 _jumpForce;

        private Vector3 _startScale;
        private float _currentTime;
        private bool _isCompressing;
        private bool _isReleasing;
        private float _currentFloorTime;
        private int _currentBounceLevel = 0;

        private Tween _jumpTween;
        private float _currentTweenCompressionAmount = 0f;

        private float _currentCompressionAmount
        {
            get
            {
                if (_isCompressing)
                    return _currentTime / _compressionTime;
                else if (_isReleasing)
                    return 1f - (_currentTime / _releaseTime);
                else
                    return _currentTweenCompressionAmount;
            }
        }

        private Vector3 _currentCompressedScale { get { return Vector3.Lerp(_startScale, _fullyCompressedScale, _currentCompressionAmount); } }

        private bool _isCollidingWithFloor
        {
            get
            {
                RaycastHit hit;
                Debug.DrawRay(transform.position, Vector3.down * _floorDetectionRayDistance, Color.yellow);
                // Does the ray intersect any objects excluding the player layer
                return Physics.Raycast(transform.position, Vector3.down, out hit, _floorDetectionRayDistance);
            }
        }

        // Event Variables

        [Header("EventSettings")]
        [SerializeField] private Vector3 _ballPositionChangeMinDelta;
        private float _sqrMaxVelocity { get { return _maxVelocity * _maxVelocity; } }
        private int _lastVelocityAsInt = 0;
        private Vector3 _lastPosition;

        // Input Control Variables

        [Header("Control Settings")]
        [SerializeField] private float _holdGestureActivationTime;

        private SwipeGestureRecognizer _swipeGesture;
        private LongPressGestureRecognizer _longPressGesture;
        private TapGestureRecognizer _tapGesture;

        // Flight variables

        [Header("Flight Variables")]
        [SerializeField] private float _flightMaxSpeed;
        [SerializeField] private Vector3 _flightForwardForce;
        [SerializeField] private float _flightHoldSpeed;

        private bool _isInFlight = false;
        

        /******* Monobehavior Methods *******/

        private void FixedUpdate()
        {
            if (rigidBody == null) return;

            // Add the input force if it exists
            //if (_currentInputForceTime > 0f)
            //{
            //    _currentInputForceTime -= Time.fixedDeltaTime;
            //    rigidBody.AddForce(_currentInputForce * _forceApplicationCurve.Evaluate(1 - (_currentInputForceTime / _swipeForceApplicationTime)), ForceMode.Force);
            //}

            // Check whether colided with ground
            //if (_isInFlight && _isCollidingWithFloor)
            //    EndFlight();

            //if (_isInFlight && _visualController.ballVisualState == BallVisualState.Regular)
            //    _visualController.ballVisualState = BallVisualState.Flying;
            //else if (!_isInFlight && _visualController.ballVisualState == BallVisualState.Flying)
            //    _visualController.ballVisualState = BallVisualState.Regular;

            // Apply flight force if in flight
            //if (_isInFlight)
            //{
            //    rigidBody.AddForce(_flightForwardForce, ForceMode.Force);
            //}

            // Apply the max velocity
            //Vector3 currentXZVelocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
            //if (currentXZVelocity.sqrMagnitude > _sqrMaxVelocity)
            //{
            //    rigidBody.velocity = new Vector3(
            //        currentXZVelocity.normalized.x * _maxVelocity,
            //        rigidBody.velocity.y,
            //        currentXZVelocity.normalized.z * _maxVelocity
            //    );
            //}


            // Check current z speed and handle onSpeedChange
            //int currentVelocity = Mathf.FloorToInt(rigidBody.velocity.z);
            //if (_lastVelocityAsInt != currentVelocity)
            //{
            //    _lastVelocityAsInt = currentVelocity;
            //    if (onBallVelocityChange != null)
            //        onBallVelocityChange.Invoke(currentVelocity);
            //}

            // Check current position to see if the position threshhold is made to update the position
            //Vector3 currentDiff = transform.position - _lastPosition;
            //if (Mathf.Abs(currentDiff.x) > Mathf.Abs(_ballPositionChangeMinDelta.x)
            //    && Mathf.Abs(currentDiff.y) > Mathf.Abs(_ballPositionChangeMinDelta.y)
            //    && Mathf.Abs(currentDiff.z) > Mathf.Abs(_ballPositionChangeMinDelta.z))
            //{
            //    if (onBallPositionChange != null)
            //        onBallPositionChange.Invoke(transform.position);
            //}

            // Handle the releasing
            if (_isReleasing)
            {
                _currentTime += Time.fixedDeltaTime;
                if (_currentTime > _releaseTime)
                {
                    _isReleasing = false;
                }
            }

            // Set the scale to whatever the current scale is
            //_visualController.transform.localScale = _currentCompressedScale;

            // Update the boost times
            //for (int i = 0; i < maxVelocityBoosts.Count; i++)
            //{
            //    MaxVelocityBoost boost = maxVelocityBoosts[i];
            //    if (!float.IsPositiveInfinity(boost.time))
            //    {
            //        boost.time -= Time.fixedDeltaTime;
            //    }
            //}
            //maxVelocityBoosts.RemoveAll((boost) => boost.time < 0);

            if (_visualController.ballVisualState != BallVisualState.Flying && _currentCompressionAmount > _minCompressionAmount && _isCompressing)
            {
                _visualController.ballVisualState = BallVisualState.Flying;
            }
        }

        private void HandleSwipe(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended)
            {
                Vector3 swipeDirection = (new Vector3(gesture.FocusX - gesture.StartFocusX, 0, gesture.FocusY - gesture.StartFocusY)).normalized;

                if (swipeDirection.x * rigidBody.velocity.x < 0)
                    rigidBody.velocity = new Vector3(rigidBody.velocity.x * 0.5f, rigidBody.velocity.y, rigidBody.velocity.z);
                _currentInputForce = swipeDirection * _swipeMagnitude;
                _currentInputForceTime = _swipeForceApplicationTime;

                Vector3 torque = new Vector3(gesture.FocusY - gesture.StartFocusY, gesture.FocusX - gesture.StartFocusX, 0) * _rotationTorqueForce;
                rigidBody.AddTorque(torque, ForceMode.Impulse);
            }
        }

        private void HandleHold(GestureRecognizer gesture)
        {
            switch(gesture.State)
            {
                case GestureRecognizerState.Began :
                {
                    if (_isCollidingWithFloor)
                        return;

                     _isInFlight = true;

                    if (_maxVelocityLerpTween != null && _maxVelocityLerpTween.IsActive())
                        _maxVelocityLerpTween.Kill();
                    break;
                }
                case GestureRecognizerState.Executing:
                {
                    if (!_isInFlight)
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

        private void HandleTap(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended && _isCollidingWithFloor)
            {
                if (_jumpTween != null && _jumpTween.IsActive())
                    _jumpTween.Kill();

                _jumpTween = DOTween.To(() => _currentTweenCompressionAmount, (value) => _currentTweenCompressionAmount = value, 1f, 0.35f).From(true);

                rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
                rigidBody.AddForce(_jumpForce, ForceMode.Impulse);
            }
        }

        void OnCollisionStay(Collision collisionInfo)
        {
            for (int i = 0; i < collisionInfo.contacts.Length; i++)
            {
                ContactPoint contact = collisionInfo.contacts[i];
                if (contact.otherCollider.CompareTag(PuffConstants.TAG_FLOOR))
                {
                    _currentFloorTime += Time.deltaTime;
                    
                    if (_currentFloorTime > _floorTimeToResetReleaseLevel)
                    {
                        _currentBounceLevel = 0;
                        return;
                    }
                }
            }
        }

        /******* Methods *******/

        public void Init()
        {
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.maxAngularVelocity = _maxAngularVelocity;

            //_visualController.Init();
            _startScale = _visualController.transform.localScale;

            _swipeGesture = new SwipeGestureRecognizer();
            _swipeGesture.MinimumDistanceUnits = 0.1f;
            _swipeGesture.StateUpdated += HandleSwipe;
            FingersScript.Instance.AddGesture(_swipeGesture);

            _longPressGesture = new LongPressGestureRecognizer();
            _longPressGesture.MinimumDurationSeconds = _holdGestureActivationTime;
            _longPressGesture.StateUpdated += HandleHold;
            FingersScript.Instance.AddGesture(_longPressGesture);

            _tapGesture = new TapGestureRecognizer();
            _tapGesture.ThresholdUnits = 0.3f;
            _tapGesture.StateUpdated += HandleTap;
            FingersScript.Instance.AddGesture(_tapGesture);

            maxVelocityBoosts = new List<MaxVelocityBoost>();

            _lastPosition = transform.position;

            _currentMaxVelocity = _baseMaxVelocity;
        }

        public void Reset(Vector3 startPosition)
        {
            rigidBody.velocity = Vector3.zero;
            _isCompressing = false;
            _isReleasing = false;
            _currentBounceLevel = 0;
            _currentTime = 0f;
            _currentFloorTime = 0f;
            transform.position = startPosition;
        }

        private void EndFlight()
        {
            _isInFlight = false;
            _currentMaxVelocity = rigidBody.velocity.z;
            _maxVelocityLerpTween = DOTween.To(() => _currentMaxVelocity, (value) => _currentMaxVelocity = value, _baseMaxVelocity, _maxVelocityLerpTime);
        }
        #endregion
    }
}
