using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DigitalRubyShared;

namespace JFrisoGames.PuffMan
{
    public class JumpController : AbstractBallController
    {
        /******* Variables & Properties*******/
        [Header("Compression")]
        [SerializeField] private Vector3 _fullyCompressedScale;
        [SerializeField] private float _compressionTime;

        private Vector3 _startScale;

        [Header("Jump Movement")]
        [SerializeField] private Vector3 _jumpForce;


        [Header("Control Settings")]
        [SerializeField] private float _maxTapDistance = 0.3f;

        private Tween _jumpTween;

        private float _currentCompressionAmount = 0f;

        private Vector3 _currentCompressedScale { get { return Vector3.Lerp(_startScale, _fullyCompressedScale, _currentCompressionAmount); } }

        private TapGestureRecognizer _tapGesture;

        /******* Monobehavior Methods *******/

        /******* Methods *******/

        public override void Init(Ball ball)
        {
            base.Init(ball);

            _startScale = _ball.visualController.transform.localScale;

            _tapGesture = new TapGestureRecognizer();
            _tapGesture.ThresholdUnits = _maxTapDistance;
            _tapGesture.StateUpdated += HandleTap;
            FingersScript.Instance.AddGesture(_tapGesture);
        }

        public override void ExecuteFixedUpdate()
        {
            base.ExecuteFixedUpdate();

            _ball.visualController.transform.localScale = _currentCompressedScale;
        }

        private void HandleTap(GestureRecognizer gesture)
        {
            if (gesture.State == GestureRecognizerState.Ended && ballInfo.isCollidingWithFloor)
            {
                if (_jumpTween != null && _jumpTween.IsActive())
                    _jumpTween.Kill();

                _jumpTween = DOTween.To(() => _currentCompressionAmount, (value) => _currentCompressionAmount = value, 1f, _compressionTime).From(true);

                rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z); // Reset the y velocity
                rigidBody.AddForce(_jumpForce, ForceMode.Impulse);
            }
        }
    }
}
