using UnityEngine;

namespace JFrisoGames.PuffMan
{
    public class BallInfo : AbstractBallController
    {
        /******* Events *******/
        public delegate void OnBallVelocityChange(Vector3 newBallVelocity);
        public event OnBallVelocityChange onBallVelocityChange;

        public delegate void OnBallPositionChange(Vector3 newPosition);
        public event OnBallPositionChange onBallPositionChange;

        public delegate void OnBallStaminaChange(float newStaminaValue);
        public event OnBallStaminaChange onBallStaminaChanged;

        /******* Variables & Properties*******/
        [Header("Info Variables")]
        [SerializeField] private float _floorDetectionRayDistance;

        public bool isInFlight { get; set; } // TODO add actual logic

        public bool isCollidingWithFloor
        {
            get => _ball.ballCollider.isCollidingWithFloor;
        }

        // Stamina settings
        private float _stamina = 1f;
        public float stamina 
        {
            get => _stamina;
            set
            {
                _stamina = Mathf.Clamp(value, 0f, 1f);
                if (Mathf.Abs(_stamina - _lastStaminaValueSinceEventTrigger) > _minBallStaminaChangeValueForEventTrigger)
                {
                    _lastStaminaValueSinceEventTrigger = _stamina;
                    onBallStaminaChanged?.Invoke(_stamina);
                }
            }
        }
        public bool isOutOfStamina => stamina <= 0f;

        public Vector3 velocity => rigidBody.velocity;

        [Header("EventSettings")]
        [SerializeField] private Vector3 _ballPositionChangeMinDelta;
        [SerializeField] private float _ballVelocityChangeMinDelta;
        [SerializeField] private float _minBallStaminaChangeValueForEventTrigger;

        private float _lastVelocityMagnitude = 0;
        private Vector3 _lastPosition;
        private float _lastStaminaValueSinceEventTrigger = 0f;
        private int _floorLayerMask;

        /******* Monobehavior Methods *******/

        private void Awake()
        {
            _floorLayerMask = LayerMask.GetMask();
        }

        /******* Methods *******/

        public override void ExecuteFixedUpdate()
        {
            base.ExecuteFixedUpdate();

            // Handle Velocity Changes
            float currentVelocityMagnitude = rigidBody.velocity.magnitude;
            if (Mathf.Abs(currentVelocityMagnitude - _lastVelocityMagnitude) > _ballVelocityChangeMinDelta)
            {
                _lastVelocityMagnitude = currentVelocityMagnitude;
                if (onBallVelocityChange != null)
                    onBallVelocityChange.Invoke(rigidBody.velocity);
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

        public bool CheckCollisionWithFloorWithDistance(float rayDistance)
        {
            int layerMask = LayerMask.GetMask(PuffConstants.LAYER_FLOOR);

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            return Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance, layerMask);
        }
    }
}
