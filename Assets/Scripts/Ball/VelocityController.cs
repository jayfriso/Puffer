using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DigitalRubyShared;

namespace JFrisoGames.PuffMan
{
    [System.Serializable]
    public class MaxVelocityBoost
    {
        public float amount; // Amount to boost by
        public float time; // The time the boost is allowed for

        public MaxVelocityBoost(float amount, float time = float.PositiveInfinity)
        {
            this.amount = amount;
            this.time = time;
        }

        public MaxVelocityBoost Copy() { return new MaxVelocityBoost(this.amount, this.time); }
    }


    public class VelocityController : AbstractBallController
    {
            /******* Variables & Properties*******/
        [SerializeField] private float _baseMaxVelocity;
        public float baseMaxVelocity { get { return _baseMaxVelocity; } }
        [SerializeField] private float _boostFadeOutTime;

        public List<MaxVelocityBoost> maxVelocityBoosts { get; private set; }

        private float _currentMaxVelocity;
        private Tween _maxVelocityLerpTween;

        private float _maxVelocity
        {
            get
            {
                float result = _currentMaxVelocity;
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
        private float _sqrMaxVelocity { get { return _maxVelocity * _maxVelocity; } }

        private float _maxYVelocity = Mathf.Infinity;
        private float _minYVelocity = Mathf.NegativeInfinity;

        private SwipeGestureRecognizer _swipeGesture;

        /******* Methods *******/

        public override void Init(Ball ball)
        {
            base.Init(ball);
            _currentMaxVelocity = _baseMaxVelocity;
            maxVelocityBoosts = new List<MaxVelocityBoost>();
        }

        public void Update()
        {
            // Apply the max velocity
            Vector3 currentXZVelocity = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);

            bool applyMaxXZVelocity = currentXZVelocity.sqrMagnitude > _sqrMaxVelocity;
            float xVelocity = applyMaxXZVelocity ? currentXZVelocity.normalized.x * _maxVelocity : rigidBody.velocity.x;
            float zVelocity = applyMaxXZVelocity ? currentXZVelocity.normalized.z * _maxVelocity : rigidBody.velocity.z;
            float yVelocity = Mathf.Clamp(rigidBody.velocity.y, _minYVelocity, _maxYVelocity);

            rigidBody.velocity = new Vector3(xVelocity, yVelocity, zVelocity);

            // Update the boost times
            for (int i = 0; i < maxVelocityBoosts.Count; i++)
            {
                MaxVelocityBoost boost = maxVelocityBoosts[i];
                if (!float.IsPositiveInfinity(boost.time))
                {
                    boost.time -= Time.fixedDeltaTime;
                }
            }
            maxVelocityBoosts.RemoveAll((boost) => boost.time < 0);
        }

        public void SetMaxVelocity(float mVToSet) { _currentMaxVelocity = mVToSet; }
        public void SetMaxYVelocity(float mVToSet) { _maxYVelocity = mVToSet; }
        public void SetMinYVelocity(float mVToSet) { _minYVelocity = mVToSet; }


        public void SetMaxVelocityLerp(float mVToSetNow, float targetMV, float lerpTime)
        {
            _currentMaxVelocity = mVToSetNow;
            _maxVelocityLerpTween = DOTween.To(() => _currentMaxVelocity, (value) => _currentMaxVelocity = value, targetMV, lerpTime);
        }

        public void KillMaxVelocityLerp()
        {
            if (_maxVelocityLerpTween != null && _maxVelocityLerpTween.IsActive())
                _maxVelocityLerpTween.Kill();
        }
    }
}
