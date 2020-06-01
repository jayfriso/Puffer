using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrisoGames.PuffMan
{
    [System.Serializable]
    public struct CameraTransformPreset
    {
        public Vector3 localPosition;

        public Vector3 eulerRotation;
        public Quaternion rotation => Quaternion.Euler(eulerRotation.x, eulerRotation.y, eulerRotation.z);

        public bool isUnitPresent => localPosition == Vector3.zero && eulerRotation == Vector3.zero;

        public static CameraTransformPreset unitPresent => new CameraTransformPreset()
        {
            localPosition = Vector3.zero,
            eulerRotation = Vector3.zero
        };


        public static CameraTransformPreset Lerp(CameraTransformPreset ctp1, CameraTransformPreset ctp2, float t)
        {
            return new CameraTransformPreset()
            {
                localPosition = Vector3.Lerp(ctp1.localPosition, ctp2.localPosition, t),
                eulerRotation = Vector3.Lerp(ctp1.eulerRotation, ctp2.eulerRotation, t)
            };
        }

        public void Add(CameraTransformPreset presentToAdd)
        {
            localPosition = localPosition + presentToAdd.localPosition;
            eulerRotation = eulerRotation + presentToAdd.eulerRotation;
        }
    }

    public class CameraBehavior : MonoBehaviour
    {
        /******* Variables & Properties*******/
        [SerializeField]
        private GameObject _ballObject;

        [Header("Horizontal Camera Behavior")]
        [SerializeField] private float _xPanUpdateIntervalTime;
        [SerializeField] private float _maxXVelocity;
        [SerializeField] private CameraTransformPreset _leftPresent;
        [SerializeField] private CameraTransformPreset _rightPresent;
        [SerializeField] private float _xPanSpeed;

        private float _currentXPanUpdateIntervalTime;
        private float _targetXPanTValue;
        private float _currentXPanTValue = 0.5f;


        [Header("Vertical Camera Behavior")]
        [SerializeField] float _floorDetectionDistance;
        [SerializeField] 
        private float _preInterpolationTimeGroundToAir;
        [SerializeField]
        private float _preInterpolationTimeAirToGround;
        [SerializeField] private float _interpolationTime;
        [SerializeField] private float _airToGroundTimeMultiplier;
        [SerializeField] private float _groundToAirTimeMultiplier;
        [SerializeField] private CameraTransformPreset _onGroundPreset;
        [SerializeField] private CameraTransformPreset _offGroundPresent;

        private float _currentInterpolationTime = 0f;
        private float _currentPreInterpolationTime = 0f;
        private bool _currentTargetIsCollidingWithFloor;

        private Vector3 _lastBallPosition;
        private Vector3 _defaultRotationEuler;

        private BallInfo _ballInfo;
        private Camera _camera;

        /******* Monobehavior Methods *******/
        private void Awake()
        {
            _lastBallPosition = _ballObject.transform.localPosition;
            _defaultRotationEuler = transform.localRotation.eulerAngles;
        }

        private void FixedUpdate()
        {
            if (_camera == null) return;

            // Ball following

            if (_lastBallPosition != null)
            {
                Vector3 ballPosDiff = _ballObject.transform.localPosition - _lastBallPosition;
                transform.localPosition += ballPosDiff;
            }
            _lastBallPosition = _ballObject.transform.localPosition;

            CameraTransformPreset finalTransform = CameraTransformPreset.unitPresent;

            /******* Horizontal Panning *******/

            _currentXPanUpdateIntervalTime += Time.fixedDeltaTime;
            if (_currentXPanUpdateIntervalTime > _xPanUpdateIntervalTime)
            {
                _xPanUpdateIntervalTime = 0f;
                float xVelocity = Mathf.Clamp(_ballInfo.velocity.x, -_maxXVelocity, _maxXVelocity);
                _targetXPanTValue = (xVelocity + _maxXVelocity) / (_maxXVelocity * 2); // T value is calculated as t = 0 at left position (xvelocity is negative max) and t = 1 at right position (xvelocity is positive max)
            }

            if (_currentXPanTValue < _targetXPanTValue)
            {
                _currentXPanTValue = Mathf.Clamp(_currentXPanTValue + Time.fixedDeltaTime * _xPanSpeed, 0f, _targetXPanTValue);
            }
            else if (_currentXPanTValue > _targetXPanTValue)
            {
                _currentXPanTValue = Mathf.Clamp(_currentXPanTValue - Time.fixedDeltaTime * _xPanSpeed, _targetXPanTValue, 1f);
            }
            finalTransform.Add(CameraTransformPreset.Lerp(_leftPresent, _rightPresent, _currentXPanTValue));
            

            /******* Vertical Panning *******/

            bool isCollidingWithFloor = _ballInfo.CheckCollisionWithFloorWithDistance(_floorDetectionDistance);
            if (_currentTargetIsCollidingWithFloor == isCollidingWithFloor)
            {
                _currentPreInterpolationTime = 0f;
            }
            else
            {
                _currentPreInterpolationTime += Time.fixedDeltaTime;
                bool hasFinishedPreInterpolation = _currentPreInterpolationTime > (_currentTargetIsCollidingWithFloor ? _preInterpolationTimeGroundToAir : _preInterpolationTimeAirToGround);

                if (hasFinishedPreInterpolation)
                {
                    _currentTargetIsCollidingWithFloor = isCollidingWithFloor;
                }
            }

            float timeWithMultiplier = _currentTargetIsCollidingWithFloor ? -Time.fixedDeltaTime * _airToGroundTimeMultiplier : Time.fixedDeltaTime * _groundToAirTimeMultiplier;
            _currentInterpolationTime = Mathf.Clamp(_currentInterpolationTime + timeWithMultiplier, 0, _interpolationTime);
            float interpolationTValue = _currentInterpolationTime / _interpolationTime;
            finalTransform.Add(CameraTransformPreset.Lerp(_onGroundPreset, _offGroundPresent, interpolationTValue));

            _camera.transform.localPosition = finalTransform.localPosition;
            _camera.transform.localRotation = finalTransform.rotation;
        }

        /******* Methods *******/

        public void Init(BallInfo ballInfo)
        {
            _ballInfo = ballInfo;
            _camera = GetComponentInChildren<Camera>();
        }

        public void HandleBallVelocityChanged(Vector3 newVelocity)
        {

        }

    }
}
